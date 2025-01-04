package com.example.client.networking.services.interfaces;

import com.example.client.networking.requests.UserValidationRequest;
import com.example.client.networking.responses.UserResponse;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

public interface ValidationApi {
    @POST("Validation/validate-user")
    Call<UserResponse> validateUser(@Body UserValidationRequest userValidationRequest);
}
