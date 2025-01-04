package com.example.client.networking.services.implementations;

import androidx.annotation.NonNull;

import com.example.client.networking.RetrofitClient;
import com.example.client.networking.requests.UserValidationRequest;
import com.example.client.networking.responses.UserResponse;
import com.example.client.networking.services.interfaces.ValidationApi;
import com.example.client.networking.callbacks.AuthenticationCallback;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ValidationService {
    private final ValidationApi validationApi;

    public ValidationService() {
        validationApi = RetrofitClient.getInstance().create(ValidationApi.class);
    }

    public void validateUser(String email, int id, String name, AuthenticationCallback callback) {
        UserValidationRequest request = new UserValidationRequest(email, id, name);

        validationApi.validateUser(request).enqueue(new Callback<UserResponse>() {
            @Override
            public void onResponse(@NonNull Call<UserResponse> call, @NonNull Response<UserResponse> response) {
                if (response.isSuccessful() && response.body() != null) {
                    callback.onSuccess(response.body());
                } else {
                    callback.onFailure("Validation failed: " + response.message());
                }
            }

            @Override
            public void onFailure(@NonNull Call<UserResponse> call, @NonNull Throwable t) {
                callback.onFailure("Error: " + t.getMessage());
            }
        });
    }
}
