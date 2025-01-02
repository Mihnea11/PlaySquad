package com.example.client.fragments.authentication;

import android.os.Bundle;
import android.text.TextUtils;
import android.util.Patterns;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.example.client.R;
import com.example.client.networking.callbacks.AuthenticationCallback;
import com.example.client.networking.responses.UserResponse;
import com.example.client.networking.services.implementations.AuthenticationService;

public class RegisterFragment extends Fragment {
    private EditText usernameField;
    private EditText emailField;
    private EditText passwordField;
    private EditText confirmPasswordField;
    private Button registerButton;
    private Button loginRedirectButton;
    private ProgressBar registerSpinner;

    private AuthenticationService authenticationService;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_authentication_register, container, false);

        usernameField = view.findViewById(R.id.usernameField);
        emailField = view.findViewById(R.id.emailField);
        passwordField = view.findViewById(R.id.passwordField);
        confirmPasswordField = view.findViewById(R.id.confirmPasswordField);
        registerButton = view.findViewById(R.id.registerButton);
        loginRedirectButton = view.findViewById(R.id.bottomLoginRedirectButton);
        registerSpinner = view.findViewById(R.id.registerSpinner);

        authenticationService = new AuthenticationService();

        registerButton.setOnClickListener(v -> handleRegister());

        loginRedirectButton.setOnClickListener(v -> {
            AuthenticationFragment parentFragment = (AuthenticationFragment) getParentFragment();
            if (parentFragment != null) {
                parentFragment.switchView(AuthenticationFragment.ViewType.LOGIN);
            }
        });

        return view;
    }

    private void handleRegister() {
        String username = usernameField.getText().toString().trim();
        String email = emailField.getText().toString().trim();
        String password = passwordField.getText().toString().trim();
        String confirmPassword = confirmPasswordField.getText().toString().trim();

        if (TextUtils.isEmpty(username) || TextUtils.isEmpty(email) || TextUtils.isEmpty(password) || TextUtils.isEmpty(confirmPassword)) {
            Toast.makeText(requireContext(), "All fields are required", Toast.LENGTH_SHORT).show();
            return;
        }

        if (!Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
            Toast.makeText(requireContext(), "Invalid email address", Toast.LENGTH_SHORT).show();
            return;
        }

        if (!password.equals(confirmPassword)) {
            Toast.makeText(requireContext(), "Passwords do not match", Toast.LENGTH_SHORT).show();
            return;
        }

        setUiState(false);

        authenticationService.register(email, password, username, 1, new AuthenticationCallback() {
            @Override
            public void onSuccess(UserResponse userResponse) {
                Toast.makeText(requireContext(), "Registration successful! Please log in.", Toast.LENGTH_LONG).show();

                AuthenticationFragment parentFragment = (AuthenticationFragment) getParentFragment();
                if (parentFragment != null) {
                    parentFragment.switchView(AuthenticationFragment.ViewType.LOGIN);
                }

                resetUI();
            }

            @Override
            public void onFailure(String errorMessage) {
                Toast.makeText(requireContext(), errorMessage + "email address already used", Toast.LENGTH_LONG).show();
                resetUI();
            }
        });
    }

    private void setUiState(boolean enabled) {
        registerButton.setEnabled(enabled);
        loginRedirectButton.setEnabled(enabled);

        loginRedirectButton.setAlpha(enabled ? 1.0f : 0.5f);

        registerSpinner.setVisibility(enabled ? View.GONE : View.VISIBLE);
        registerButton.setVisibility(enabled ? View.VISIBLE : View.GONE);
    }

    private void resetUI() {
        setUiState(true);
    }
}
