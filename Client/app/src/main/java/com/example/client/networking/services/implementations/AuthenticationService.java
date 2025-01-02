package com.example.client.networking.services.implementations;

import androidx.annotation.NonNull;

import com.example.client.networking.RetrofitClient;
import com.example.client.networking.requests.LoginRequest;
import com.example.client.networking.requests.GoogleLoginRequest;
import com.example.client.networking.requests.RegisterRequest;
import com.example.client.networking.responses.UserResponse;
import com.example.client.networking.services.interfaces.AuthenticationApi;
import com.example.client.networking.callbacks.AuthenticationCallback;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class AuthenticationService {
    private final AuthenticationApi authenticationApi;

    public AuthenticationService() {
        authenticationApi = RetrofitClient.getInstance().create(AuthenticationApi.class);
    }

    public void login(String email, String password, AuthenticationCallback callback) {
        LoginRequest loginRequest = new LoginRequest(email, password);

        authenticationApi.normalLogin(loginRequest).enqueue(new Callback<UserResponse>() {
            @Override
            public void onResponse(@NonNull Call<UserResponse> call, @NonNull Response<UserResponse> response) {
                if (response.isSuccessful() && response.body() != null) {
                    callback.onSuccess(response.body());
                } else {
                    callback.onFailure("Login failed: " + response.message());
                }
            }

            @Override
            public void onFailure(@NonNull Call<UserResponse> call, @NonNull Throwable t) {
                callback.onFailure("Error: " + t.getMessage());
            }
        });
    }

    public void googleLogin(String idToken, AuthenticationCallback callback) {
        GoogleLoginRequest googleLoginRequest = new GoogleLoginRequest(idToken);

        authenticationApi.googleLogin(googleLoginRequest).enqueue(new Callback<UserResponse>() {
            @Override
            public void onResponse(@NonNull Call<UserResponse> call, @NonNull Response<UserResponse> response) {
                if (response.isSuccessful() && response.body() != null) {
                    callback.onSuccess(response.body());
                } else {
                    callback.onFailure("Google login failed: " + response.message());
                }
            }

            @Override
            public void onFailure(@NonNull Call<UserResponse> call, @NonNull Throwable t) {
                callback.onFailure("Error: " + t.getMessage());
            }
        });
    }

    public void register(String email, String password, String name, int roleId, AuthenticationCallback callback) {
        RegisterRequest registerRequest = new RegisterRequest(email, password, name, roleId);

        authenticationApi.register(registerRequest).enqueue(new Callback<UserResponse>() {
            @Override
            public void onResponse(@NonNull Call<UserResponse> call, @NonNull Response<UserResponse> response) {
                if (response.isSuccessful() && response.body() != null) {
                    callback.onSuccess(response.body());
                } else {
                    callback.onFailure("Registration failed: " + response.message());
                }
            }

            @Override
            public void onFailure(@NonNull Call<UserResponse> call, @NonNull Throwable t) {
                callback.onFailure("Error: " + t.getMessage());
            }
        });
    }
}