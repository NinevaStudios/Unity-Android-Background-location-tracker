package com.ninevastudios.locationtracker;

import android.app.Activity;
import android.content.ComponentName;
import android.content.Intent;
import android.content.IntentSender;
import android.content.ServiceConnection;
import android.os.Bundle;
import android.os.IBinder;
import android.support.annotation.NonNull;
import android.util.Log;

import com.google.android.gms.common.api.ResolvableApiException;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.location.LocationSettingsRequest;
import com.google.android.gms.location.LocationSettingsResponse;
import com.google.android.gms.location.SettingsClient;
import com.google.android.gms.tasks.OnCanceledListener;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;

public class LocationHelperActivity extends Activity {
	private static final int REQUEST_CHECK_SETTINGS = 666;
	public static final String EXTRA_EXCEPTION = "exception";

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		SettingsClient settingsClient = LocationServices.getSettingsClient(this);

		LocationRequest locationRequest = new LocationRequest();
		locationRequest.setInterval(10 * 1000);
		locationRequest.setFastestInterval(5 * 1000);
		locationRequest.setPriority(locationRequest.PRIORITY_HIGH_ACCURACY);

		LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
		builder.addLocationRequest(locationRequest);
		builder.setAlwaysShow(true);
		LocationSettingsRequest locationSettingsRequest = builder.build();

		settingsClient
				.checkLocationSettings(locationSettingsRequest)
				.addOnSuccessListener(new OnSuccessListener<LocationSettingsResponse>() {
					@Override
					public void onSuccess(LocationSettingsResponse locationSettingsResponse) {

						Log.d("XXX", "onSuccess -> onSuccess");

//						requestLocationUpdates();
						finish();
					}
				})
				.addOnFailureListener(new OnFailureListener() {
					@Override
					public void onFailure(@NonNull Exception e) {
						if (e instanceof ResolvableApiException) {
							try {
								ResolvableApiException resolvable = (ResolvableApiException) e;
								resolvable.startResolutionForResult(LocationHelperActivity.this, REQUEST_CHECK_SETTINGS);
							} catch (IntentSender.SendIntentException sendEx) {
								UnityCallbacks.onCheckLocationSettingsFailed();
							}
						} else {
							UnityCallbacks.onCheckLocationSettingsFailed();
						}

						finish();
					}
				})
				.addOnCanceledListener(new OnCanceledListener() {
					@Override
					public void onCanceled() {
						Log.d("XXX", "checkLocationSettings -> onCanceled");

						finish();

					}
				});
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		super.onActivityResult(requestCode, resultCode, data);

		if (resultCode == RESULT_OK) {
			Intent mIntent = new Intent(this, NinevaLocationService.class);
			Log.d("XXX", "onActivityResult -> OK");
		} else {
			Log.d("XXX", "onActivityResult -> else");
		}

		finish();
	}
}
