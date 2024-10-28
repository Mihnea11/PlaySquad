package com.example.client.authentication;

import android.os.Bundle;
import android.text.InputType;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.ImageButton;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import com.example.client.R;

public class LoginFragment extends Fragment {

    private EditText passwordField;
    private ImageButton viewPasswordButton;
    private boolean isPasswordVisible = false;

    public LoginFragment() {
        // Required empty public constructor
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        return inflater.inflate(R.layout.fragment_authentication_login, container, false);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        passwordField = view.findViewById(R.id.passwordField);
        viewPasswordButton = view.findViewById(R.id.viewPasswordButton);

        viewPasswordButton.setOnClickListener(v -> togglePasswordVisibility());
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
}
