package com.example.client.network;

import com.example.client.locations.Location;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;

public interface LocationsApi {
    @GET("arenas")
    Call<List<Location>> getLocations();
}