package com.ninevastudios.locationtracker;

import android.Manifest;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.IntentSender;
import android.content.pm.PackageManager;
import android.location.Location;
import android.support.annotation.Keep;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.util.Log;

import com.google.android.gms.common.api.ResolvableApiException;
import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationCallback;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationResult;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.location.LocationSettingsRequest;
import com.google.android.gms.location.LocationSettingsResponse;
import com.google.android.gms.location.SettingsClient;
import com.google.android.gms.tasks.OnCanceledListener;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;

@Keep
public class LocationUnityApi {

	private static String TAG = LocationUnityApi.class.getSimpleName();

	private static final int REQUEST_CHECK_SETTINGS = 666;
	private static FusedLocationProviderClient fusedLocationClient;
	private static LocationRequest locationRequest;
	private static LocationCallback locationCallback;

	@Keep
	public static void init(Context context) {
		fusedLocationClient = LocationServices.getFusedLocationProviderClient(context);
		createLocationRequest(10000, 5000, LocationRequest.PRIORITY_HIGH_ACCURACY);

		locationCallback = new LocationCallback() {
			@Override
			public void onLocationResult(LocationResult locationResult) {
				if (locationResult == null) {
					return;
				}
				for (Location location : locationResult.getLocations()) {
					// Update UI with location data
					// ...
				}
			}
		};
	}


	private static void createLocationRequest(int interval, int fastestInterval, int priority) {
		locationRequest = LocationRequest.create();
		locationRequest.setInterval(interval);
		locationRequest.setFastestInterval(fastestInterval);
		locationRequest.setPriority(priority);
	}

	@Keep
	public static void getLastLocation(Context context) {
		if (hasFineLocationPermission(context) || hasCoarseLocationPermission(context)) {
			// TODO: Consider calling
			//    ActivityCompat#requestPermissions
			// here to request the missing permissions, and then overriding
			//   public void onRequestPermissionsResult(int requestCode, String[] permissions,
			//                                          int[] grantResults)
			// to handle the case where the user grants the permission. See the documentation
			// for ActivityCompat#requestPermissions for more details.
			return;
		}

		fusedLocationClient.getLastLocation().addOnSuccessListener(new OnSuccessListener<Location>() {
			@Override
			public void onSuccess(Location location) {
				// Got last known location. In some rare situations this can be null.
				if (location != null) {
					// Logic to handle location object
				}
			}
		});
	}

	@Keep
	public static void checkLocationSettings(final Activity activity) {
		LocationSettingsRequest request = new LocationSettingsRequest.Builder()
				.setAlwaysShow(true)
				.addLocationRequest(locationRequest).build();

		Task<LocationSettingsResponse> task = LocationServices.getSettingsClient(activity).checkLocationSettings(request);
		task.addOnSuccessListener(new OnSuccessListener<LocationSettingsResponse>() {
			@Override
			public void onSuccess(LocationSettingsResponse locationSettingsResponse) {
				// TODO
			}
		}).addOnFailureListener(new OnFailureListener() {
			@Override
			public void onFailure(@NonNull Exception e) {
				if (e instanceof ResolvableApiException) {
					// Location settings are not satisfied, but this can be fixed
					// by showing the user a dialog.
					try {
						// TODO create another intermedisate activity ???
						// Show the dialog by calling startResolutionForResult(),
						// and check the result in onActivityResult().
						ResolvableApiException resolvable = (ResolvableApiException) e;
						resolvable.startResolutionForResult(activity, REQUEST_CHECK_SETTINGS);
					} catch (IntentSender.SendIntentException sendEx) {
						// Ignore the error.
					}
				} else {
					// TODO onFailure
				}
			}
		}).addOnCanceledListener(new OnCanceledListener() {
			@Override
			public void onCanceled() {
				Log.e(TAG, "checkLocationSettings -> onCanceled");
			}
		});
	}

	@Keep
	public static void removeLocationUpdates(Context context) {
		fusedLocationClient.removeLocationUpdates(locationCallback);
	}

	@Keep
	public static void requestLocationUpdates(Context context) {
		fusedLocationClient.requestLocationUpdates(locationRequest, locationCallback, null /* Looper */);
	}

	@Keep
	public static void requestBackgroundLocationUpdates(Context context) {
		context.startService(getIntent(context));
	}

	@Keep
	public static void stopBackgroundLocationUpdates(Context context) {
		context.stopService(getIntent(context));
	}

	@NonNull
	private static Intent getIntent(Context context) {
		return new Intent(context.getApplicationContext(), NinevaLocationService.class);
	}

	private static boolean hasFineLocationPermission(Context context) {
		return ActivityCompat.checkSelfPermission(context, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED;
	}

	private static boolean hasCoarseLocationPermission(Context context) {
		return ActivityCompat.checkSelfPermission(context, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED;
	}
}
