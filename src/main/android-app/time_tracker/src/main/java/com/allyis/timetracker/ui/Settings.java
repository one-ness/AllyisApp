package com.allyis.timetracker.ui;

/*
    Settings class is created for checking if user puts in all required fields before saving a clock punch.
    If other settings such as for an edit page, etc. can be intialized here.
*/

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

public class Settings {
    /** Integer value before being set */
    public static final int NOT_SET = -1;

    /** Request to add an event */
    public static final int ADD_REQUEST = 0;
    /** Request to authorize token */
    public static final int AUTHORIZATION_REQUEST = 2;
    /** Request to add an event */
    public static final int ACCOUNT_REQUEST = 3;

    public static int SYNC_INTERVAL_MILLIS = 3600000;

    private Settings() {
        // static class
    }

    //Connectivity Setting - can use if required
    public static boolean isOnline(Context context) {
        ConnectivityManager cm = (ConnectivityManager)context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo netInfo = cm.getActiveNetworkInfo();
        if (netInfo != null && netInfo.isConnectedOrConnecting())
            return true;

        return false;
    }
}
