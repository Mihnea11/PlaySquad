package com.example.client.locations;

public class Location {
    private int id;
    private String name;
    private String address;
    private int minPlayers;
    private int maxPlayers;
    private String type;

    public Location(String nameA, String addressA, int mimPlayer, int maxPlayer, String type) {
        name=nameA;
        address=addressA;
        minPlayers=mimPlayer;
        maxPlayers=maxPlayer;
        this.type=type;
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getAddress() {
        return address;
    }

    public int getMinPlayers() {
        return minPlayers;
    }

    public int getMaxPlayers() {
        return maxPlayers;
    }

    public String getType() {
        return type;
    }
}
