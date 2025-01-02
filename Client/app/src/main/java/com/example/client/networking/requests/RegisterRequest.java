package com.example.client.networking.requests;

public class RegisterRequest {
    private String email;
    private String password;
    private String name;
    private int roleId;

    public RegisterRequest(String email, String password, String name, int roleId) {
        this.email = email;
        this.password = password;
        this.name = name;
        this.roleId = roleId;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getRoleId() {
        return roleId;
    }

    public void setRoleId(int roleId) {
        this.roleId = roleId;
    }
}
