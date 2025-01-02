package com.example.client.networking.services.interfaces;

import com.example.client.networking.requests.GoogleLoginRequest;
import com.example.client.networking.requests.LoginRequest;
import com.example.client.networking.requests.RegisterRequest;
import com.example.client.networking.responses.UserResponse;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

public interface AuthenticationApi {
    @POST("Authentication/login")
    Call<UserResponse> normalLogin(@Body LoginRequest loginRequest);

    @POST("Authentication/google-login")
    Call<UserResponse> googleLogin(@Body GoogleLoginRequest googleLoginRequest);

    @POST("Authentication/register")
    Call<UserResponse> register(@Body RegisterRequest registerRequest);
}
