package com.example.client.networking.services.interfaces;

import com.example.client.networking.requests.GoogleLoginRequest;
import com.example.client.networking.requests.LoginRequest;
import com.example.client.networking.requests.RegisterRequest;
import com.example.client.networking.wrappers.LoginResponseWrapper;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

public interface AuthenticationApi {
    @POST("Authentication/login")
    Call<LoginResponseWrapper> normalLogin(@Body LoginRequest loginRequest);

    @POST("Authentication/google-login")
    Call<LoginResponseWrapper> googleLogin(@Body GoogleLoginRequest googleLoginRequest);

    @POST("Authentication/register")
    Call<LoginResponseWrapper> register(@Body RegisterRequest registerRequest);
}
