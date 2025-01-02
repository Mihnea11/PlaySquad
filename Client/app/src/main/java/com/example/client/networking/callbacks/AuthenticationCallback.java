package com.example.client.networking.callbacks;

import com.example.client.networking.responses.UserResponse;

public interface AuthenticationCallback {
    void onSuccess(UserResponse userResponse);
    void onFailure(String errorMessage);
}
