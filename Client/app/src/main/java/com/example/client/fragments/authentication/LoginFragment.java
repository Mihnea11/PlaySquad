package com.example.client.fragments.authentication;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.text.InputType;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.activity.result.ActivityResultLauncher;
import androidx.activity.result.IntentSenderRequest;
import androidx.activity.result.contract.ActivityResultContracts;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import com.example.client.R;
import com.example.client.networking.responses.UserResponse;
import com.example.client.networking.callbacks.AuthenticationCallback;
import com.example.client.networking.services.implementations.AuthenticationService;
import com.google.android.gms.auth.api.identity.BeginSignInRequest;
import com.google.android.gms.auth.api.identity.Identity;
import com.google.android.gms.auth.api.identity.SignInClient;
import com.google.android.gms.auth.api.identity.SignInCredential;
import com.google.android.gms.common.api.ApiException;

public class LoginFragment extends Fragment {
    private static final String TAG = "LoginFragment";
    private static final String SERVER_CLIENT_ID = "804214446242-06p0rtt6f6p0gk8l0k1mrs854jtfv5vr.apps.googleusercontent.com";

    private EditText emailField, passwordField;
    private ImageButton viewPasswordButton;
    private View loginButton;
    private LinearLayout googleLoginButton;
    private ProgressBar loadingSpinner;
    private View switchViewButton;
    private boolean isPasswordVisible = false;

    private AuthenticationService authenticationService;
    private SignInClient signInClient;
    private BeginSignInRequest signInRequest;

    private final ActivityResultLauncher<IntentSenderRequest> googleSignInLauncher =
            registerForActivityResult(new ActivityResultContracts.StartIntentSenderForResult(), result -> {
                if (result.getResultCode() == -1 && result.getData() != null) {
                    handleGoogleSignInResult(result.getData());
                } else {
                    Toast.makeText(requireContext(), "Google Sign-In Failed", Toast.LENGTH_SHORT).show();
                }
            });

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        return inflater.inflate(R.layout.fragment_authentication_login, container, false);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        emailField = view.findViewById(R.id.emailField);
        passwordField = view.findViewById(R.id.passwordField);
        viewPasswordButton = view.findViewById(R.id.viewPasswordButton);
        googleLoginButton = view.findViewById(R.id.googleLoginButton);
        loginButton = view.findViewById(R.id.loginButton);
        loadingSpinner = view.findViewById(R.id.loadingSpinner);
        switchViewButton = view.findViewById(R.id.bottomCreateAccountButton);

        authenticationService = new AuthenticationService();

        signInClient = Identity.getSignInClient(requireContext());
        signInRequest = new BeginSignInRequest.Builder()
                .setGoogleIdTokenRequestOptions(
                        BeginSignInRequest.GoogleIdTokenRequestOptions.builder()
                                .setSupported(true)
                                .setServerClientId(SERVER_CLIENT_ID)
                                .setFilterByAuthorizedAccounts(false)
                                .build())
                .build();

        viewPasswordButton.setOnClickListener(v -> togglePasswordVisibility());

        loginButton.setOnClickListener(v -> login(view));

        googleLoginButton.setOnClickListener(v -> signInWithGoogle());

        switchViewButton.setOnClickListener(v -> {
            AuthenticationFragment parentFragment = (AuthenticationFragment) getParentFragment();
            if (parentFragment != null) {
                parentFragment.switchView(AuthenticationFragment.ViewType.REGISTER);
            }
        });
    }

    private void togglePasswordVisibility() {
        if (isPasswordVisible) {
            passwordField.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);
            viewPasswordButton.setImageResource(R.drawable.ic_eye_closed);
        } else {
            passwordField.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_VISIBLE_PASSWORD);
            viewPasswordButton.setImageResource(R.drawable.ic_eye_open);
        }

        passwordField.setSelection(passwordField.length());
        isPasswordVisible = !isPasswordVisible;
    }

    private void login(View view) {
        String email = emailField.getText().toString().trim();
        String password = passwordField.getText().toString().trim();

        if (email.isEmpty() || password.isEmpty()) {
            Toast.makeText(requireContext(), "Please fill in all fields.", Toast.LENGTH_SHORT).show();
            return;
        }

        setUiState(false);

        authenticationService.login(email, password, new AuthenticationCallback() {
            @Override
            public void onSuccess(UserResponse userResponse) {
                saveUserLocally(userResponse);

                setUiState(true);

                NavController navController = Navigation.findNavController(view);
                navController.navigate(R.id.action_authenticationFragment_to_dashboardFragment);
            }

            @Override
            public void onFailure(String errorMessage) {
                setUiState(true);

                Toast.makeText(requireContext(), "Login failed: " + errorMessage, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void setUiState(boolean enabled) {
        loginButton.setEnabled(enabled);
        googleLoginButton.setEnabled(enabled);
        switchViewButton.setEnabled(enabled);

        switchViewButton.setAlpha(enabled ? 1.0f : 0.5f);

        loadingSpinner.setVisibility(enabled ? View.GONE : View.VISIBLE);
        loginButton.setVisibility(enabled ? View.VISIBLE : View.GONE);
        googleLoginButton.setVisibility(enabled ? View.VISIBLE : View.GONE);
    }

    private void signInWithGoogle() {
        signInClient.beginSignIn(signInRequest)
                .addOnSuccessListener(requireActivity(), result -> {
                    try {
                        IntentSenderRequest intentSenderRequest = new IntentSenderRequest.Builder(result.getPendingIntent()).build();
                        googleSignInLauncher.launch(intentSenderRequest);
                    } catch (Exception e) {
                        Log.e(TAG, "Error launching sign-in intent", e);
                        Toast.makeText(requireContext(), "Google Sign-In Failed to launch.", Toast.LENGTH_SHORT).show();
                    }
                })
                .addOnFailureListener(requireActivity(), e -> {
                    Log.e(TAG, "Google Sign-In initialization failed", e);
                    Toast.makeText(requireContext(), "Google Sign-In not available.", Toast.LENGTH_SHORT).show();
                });
    }

    private void handleGoogleSignInResult(Intent data) {
        try {
            SignInCredential credential = signInClient.getSignInCredentialFromIntent(data);
            String idToken = credential.getGoogleIdToken();
            if (idToken != null) {
                sendIdTokenToServer(idToken);
            } else {
                Toast.makeText(requireContext(), "Failed to retrieve ID Token", Toast.LENGTH_SHORT).show();
            }
        } catch (ApiException e) {
            Log.e(TAG, "Google Sign-In failed", e);
            Toast.makeText(requireContext(), "Google Sign-In Failed.", Toast.LENGTH_SHORT).show();
        }
    }

    private void sendIdTokenToServer(String idToken) {
        authenticationService.googleLogin(idToken, new AuthenticationCallback() {
            @Override
            public void onSuccess(UserResponse userResponse) {
                saveUserLocally(userResponse);

                NavController navController = Navigation.findNavController(requireView());
                navController.navigate(R.id.action_authenticationFragment_to_dashboardFragment);
            }

            @Override
            public void onFailure(String errorMessage) {
                Toast.makeText(requireContext(), "Google login failed: " + errorMessage, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void saveUserLocally(UserResponse userResponse) {
        Context context = requireContext();
        context.getSharedPreferences("user_prefs", Context.MODE_PRIVATE)
                .edit()
                .putString("user_data", userResponse.toJson())
                .apply();
    }
}
