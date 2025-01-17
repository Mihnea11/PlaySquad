package com.example.client.fragments.authentication;

import android.os.Bundle;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentTransaction;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import com.example.client.R;

public class AuthenticationFragment extends Fragment {
    public enum ViewType {
        LOGIN, REGISTER
    }

    public AuthenticationFragment() {
        // Required empty public constructor
    }

    public static AuthenticationFragment newInstance(String param1, String param2) {
        AuthenticationFragment fragment = new AuthenticationFragment();
        Bundle args = new Bundle();
        args.putString("ARG_PARAM1", param1);
        args.putString("ARG_PARAM2", param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        return inflater.inflate(R.layout.fragment_authentication, container, false);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        if (savedInstanceState == null) {
            loadView(ViewType.LOGIN);
        }
    }

    public void switchView(ViewType viewType) {
        loadView(viewType);
    }

    private void loadView(ViewType viewType) {
        Fragment fragment;
        if (viewType == ViewType.LOGIN) {
            fragment = new LoginFragment();
        } else {
            fragment = new RegisterFragment();
        }

        FragmentTransaction transaction = getChildFragmentManager().beginTransaction();
        transaction.replace(R.id.fragmentContainer, fragment);
        transaction.commit();
    }
}
