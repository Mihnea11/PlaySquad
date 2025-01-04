package com.example.client;

import android.os.Bundle;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

public class MainActivity extends AppCompatActivity {

    private View topAppBar;
    private View bottomNavigationBar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        topAppBar = findViewById(R.id.top_app_bar);
        bottomNavigationBar = findViewById(R.id.bottom_navigation_card);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return WindowInsetsCompat.CONSUMED;
        });

        findViewById(R.id.nav_host_fragment).post(() -> {
            try {
                NavController navController = Navigation.findNavController(this, R.id.nav_host_fragment);

                navController.addOnDestinationChangedListener((controller, destination, arguments) -> {
                    if (destination.getId() == R.id.authenticationFragment || destination.getId() == R.id.mainFragment) {
                        topAppBar.setVisibility(View.GONE);
                        bottomNavigationBar.setVisibility(View.GONE);
                    } else {
                        topAppBar.setVisibility(View.VISIBLE);
                        bottomNavigationBar.setVisibility(View.VISIBLE);
                    }
                });
            } catch (Exception e) {
                e.printStackTrace();
            }
        });
    }
}
