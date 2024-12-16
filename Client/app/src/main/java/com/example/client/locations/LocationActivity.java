package com.example.client.locations;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.client.R;
import com.example.client.network.LocationsApi;
import com.example.client.network.RetrofitClient;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LocationActivity extends AppCompatActivity {

    private RecyclerView recyclerView;
    private LocationAdapter adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_locations);

        recyclerView = findViewById(R.id.recycler_communities);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));

        fetchLocations();
    }

    private void fetchLocations() {
        LocationsApi api = RetrofitClient.getInstance().getApi(LocationsApi.class);
        api.getLocations().enqueue(new Callback<List<Location>>() {
            @Override
            public void onResponse(Call<List<Location>> call, Response<List<Location>> response) {
                if (response.isSuccessful() && response.body() != null) {
                    List<Location> locations = response.body();
                    Log.d("messages","messages");
                    for (Location location : locations) {
                        Log.d("LocationActivity", "Location: " + location.getName() +
                                ", Address: " + location.getAddress() +
                                ", Players: " + location.getMinPlayers() + "-" + location.getMaxPlayers() +
                                ", Type: " + location.getType());
                    }
                    adapter = new LocationAdapter(locations);
                    recyclerView.setAdapter(adapter);
                } else {
                    Log.e("LocationActivity", "Response Error: " + response.code());
                }
            }

            @Override
            public void onFailure(Call<List<Location>> call, Throwable t) {
                Log.e("LocationActivity", "API Call Failed: " + t.getMessage());
            }
        });
    }

    public static class LocationAdapter extends RecyclerView.Adapter<LocationAdapter.LocationViewHolder> {
        private final List<Location> locations;

        public LocationAdapter(List<Location> locations) {
            this.locations = locations;
        }

        @Override
        public LocationViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.fragment_card_location, parent, false);
            return new LocationViewHolder(view);
        }

        @Override
        public void onBindViewHolder(LocationViewHolder holder, int position) {
            Location location = locations.get(position);
            holder.name.setText(location.getName());
        }

        @Override
        public int getItemCount() {
            return locations.size();
        }

        static class LocationViewHolder extends RecyclerView.ViewHolder {
            TextView name, address, players, type;

            public LocationViewHolder(View itemView) {
                super(itemView);
                name = itemView.findViewById(R.id.tv_community_name);
            }
        }
    }
}