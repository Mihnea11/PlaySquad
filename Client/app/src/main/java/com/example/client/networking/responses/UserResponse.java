package com.example.client.networking.responses;

import com.google.gson.Gson;

public class UserResponse {
    private int id;
    private String email;
    private String name;
    private String pictureUrl;
    private String roleName;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getPictureUrl() {
        return pictureUrl;
    }

    public void setPictureUrl(String pictureUrl) {
        this.pictureUrl = pictureUrl;
    }

    public String getRoleName() {
        return roleName;
    }

    public void setRoleName(String roleName) {
        this.roleName = roleName;
    }

    public String toJson() {
        return new Gson().toJson(this);
    }

    public static UserResponse fromJson(String json) {
        return new Gson().fromJson(json, UserResponse.class);
    }
}
