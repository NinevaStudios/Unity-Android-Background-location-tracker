package com.ninevastudios.locationtracker;

import android.Manifest;
import android.app.Service;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.os.IBinder;
import android.support.annotation.Keep;
import android.support.v4.app.ActivityCompat;
import android.util.Log;

import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.tasks.OnSuccessListener;

@Keep
public class LocationService extends Service {

	private static final String TAG = LocationService.class.getSimpleName();

	private FusedLocationProviderClient fusedLocationClient;
	private LocationRequest locationRequest;

	@Override
	public void onCreate() {
		super.onCreate();
		fusedLocationClient = LocationServices.getFusedLocationProviderClient(this);
	}

	@Override
	public IBinder onBind(Intent intent) {
		return null;
	}

	@Override
	public int onStartCommand(Intent intent, int flags, int startId) {
		Log.e(TAG, "onStartCommand");
		super.onStartCommand(intent, flags, startId);


		return START_STICKY;
	}



}
