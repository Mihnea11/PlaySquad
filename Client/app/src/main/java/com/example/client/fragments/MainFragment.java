package com.example.client.fragments;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.navigation.fragment.NavHostFragment;

import com.example.client.R;
import com.example.client.networking.callbacks.AuthenticationCallback;
import com.example.client.networking.responses.UserResponse;
import com.example.client.networking.services.implementations.ValidationService;
import com.google.gson.Gson;

public class MainFragment extends Fragment {

    private static final String USER_PREFS = "user_prefs";
    private static final String USER_DATA_KEY = "user_data";

    private ValidationService validationService;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        return inflater.inflate(R.layout.fragment_main, container, false);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        validationService = new ValidationService();

        SharedPreferences sharedPreferences = requireContext().getSharedPreferences(USER_PREFS, Context.MODE_PRIVATE);
        String userDataJson = sharedPreferences.getString(USER_DATA_KEY, null);

        if (userDataJson == null) {
            navigateToAuthentication();
            return;
        }

        Gson gson = new Gson();
        UserResponse savedUser = gson.fromJson(userDataJson, UserResponse.class);

        validateUser(savedUser);
    }

    private void validateUser(UserResponse savedUser) {
        validationService.validateUser(savedUser.getEmail(), savedUser.getId(), savedUser.getName(), new AuthenticationCallback() {
            @Override
            public void onSuccess(UserResponse userResponse) {
                navigateToDashboard();
            }

            @Override
            public void onFailure(String errorMessage) {
                navigateToAuthentication();
            }
        });
    }

    private void navigateToDashboard() {
        NavHostFragment.findNavController(this).navigate(R.id.action_mainFragment_to_dashboardFragment);
    }

    private void navigateToAuthentication() {
        NavHostFragment.findNavController(this).navigate(R.id.action_mainFragment_to_authenticationFragment);
    }
}
