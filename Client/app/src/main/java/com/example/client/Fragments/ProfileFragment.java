package com.example.client.Fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.example.client.R;

public class ProfileFragment extends Fragment {

    private ImageView profileImageView;
    private TextView userNameTextView;
    private TextView userLocationTextView;
    private TextView userSportsTextView;
    private TextView userLevelTextView;
    private TextView userAvailabilityTextView;
    private Button editProfileButton;

    public ProfileFragment() {
        // Required empty public constructor
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_profile, container, false);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        // Initialize UI components
        profileImageView = view.findViewById(R.id.profileImageView);
        userNameTextView = view.findViewById(R.id.userNameTextView);
        userLevelTextView = view.findViewById(R.id.userLevelTextView);
        editProfileButton = view.findViewById(R.id.editProfileButton);

        // Load user details (Replace with actual data fetching logic)
        loadUserProfile();

        // Set click listener for editing profile
        editProfileButton.setOnClickListener(v -> navigateToEditProfile());
    }

    /**
     * Loads user details into the profile page.
     */
    private void loadUserProfile() {
        // Mock data for demonstration purposes
        userNameTextView.setText("John Doe");
        userLocationTextView.setText("San Francisco, CA");
        userSportsTextView.setText("Soccer, Basketball, Tennis");
        userLevelTextView.setText("Intermediate");
        userAvailabilityTextView.setText("Weekends: 10 AM - 6 PM");

        profileImageView.setImageResource(R.drawable.app_logo);
    }

    /**
     * Navigates to the EditProfileFragment for editing user details.
     */
    private void navigateToEditProfile() {
        // Navigate to edit profile screen (update with proper navigation action)
        // For example, using Navigation Component:
        // Navigation.findNavController(requireView()).navigate(R.id.action_profileFragment_to_editProfileFragment);
    }
}
