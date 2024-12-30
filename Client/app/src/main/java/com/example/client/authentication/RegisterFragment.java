package com.example.client.authentication;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import com.example.client.R;

public class RegisterFragment extends Fragment {

    private EditText usernameField;
    private EditText emailField;
    private EditText passwordField;
    private EditText confirmPasswordField;
    private Button registerButton;
    private Button loginRedirectButton;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_authentication_register, container, false);

        // Initialize the UI components
        usernameField = view.findViewById(R.id.userNameTextView);
        emailField = view.findViewById(R.id.emailField);
        passwordField = view.findViewById(R.id.passwordField);
        confirmPasswordField = view.findViewById(R.id.confirmPasswordField);
        registerButton = view.findViewById(R.id.registerButton);
        loginRedirectButton = view.findViewById(R.id.bottomLoginRedirectButton);

        // Set up any necessary listeners (if needed)
        registerButton.setOnClickListener(v -> {
            // Handle register button click
        });

        loginRedirectButton.setOnClickListener(v -> {
            // Handle login redirect button click
        });

        return view;
    }
}
