package com.example.client.Fragments;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.navigation.fragment.NavHostFragment;

import com.example.client.R;

public class MainFragment extends Fragment {

    private static final String CREDENTIALS_KEY = "user_credentials";
    private static final String SHARED_PREFS_NAME = "app_prefs";
    private static final int DELAY_MILLIS = 5000; // 5 seconds delay

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_main, container, false);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        // Use a Handler to delay the navigation by 5 seconds
        new Handler(Looper.getMainLooper()).postDelayed(() -> {
            // Check for credentials and navigate accordingly
            if (areCredentialsLoaded()) {
                // Navigate to Dashboard if credentials exist
                NavHostFragment.findNavController(this).navigate(R.id.action_mainFragment_to_dashboardFragment);
            } else {
                // Navigate to Authentication if no credentials
                NavHostFragment.findNavController(this).navigate(R.id.action_mainFragment_to_authenticationFragment);
            }
        }, DELAY_MILLIS);
    }

    // Method to check if credentials are loaded
    private boolean areCredentialsLoaded() {
        SharedPreferences sharedPreferences = requireContext().getSharedPreferences(SHARED_PREFS_NAME, getContext().MODE_PRIVATE);
        String credentials = sharedPreferences.getString(CREDENTIALS_KEY, null);
        return credentials != null;
    }
}
