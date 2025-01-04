package com.example.client.networking.wrappers;

import com.example.client.networking.responses.UserResponse;
import com.google.gson.annotations.SerializedName;

public class LoginResponseWrapper {
    @SerializedName("message")
    private String message;

    @SerializedName("user")
    private UserResponse user;

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public UserResponse getUser() {
        return user;
    }

    public void setUser(UserResponse user) {
        this.user = user;
    }
}
