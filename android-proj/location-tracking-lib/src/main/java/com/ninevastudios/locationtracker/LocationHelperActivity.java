package com.ninevastudios.locationtracker;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.IntentSender;
import android.os.Bundle;
import android.support.annotation.Keep;
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
	private static final String TAG = LocationHelperActivity.class.getSimpleName();
	
	private static final int REQUEST_CHECK_SETTINGS = 666;
	public static final String EXTRA_EXCEPTION = "exception";
	static final String EXTRA_REQUEST_INTERVAL = "com.ninevastudios.locationtracker.RequestInterval";
	static final String EXTRA_REQUEST_FASTEST_INTERVAL = "com.ninevastudios.locationtracker.RequestFastestInterval";
	static final String EXTRA_REQUEST_PRIORITY = "com.ninevastudios.locationtracker.RequestPriority";
	static final String EXTRA_REQUEST_MAX_WAIT_TIME = "com.ninevastudios.locationtracker.RequestMaxWaitTime";
	static final String EXTRA_NOTIFICATION_TITLE = "com.ninevastudios.locationtracker.NotificationTitle";
	static final String EXTRA_NOTIFICATION_CONTENT = "com.ninevastudios.locationtracker.NotificationContent";
	static final String EXTRA_NOTIFICATION_VISIBILITY = "com.ninevastudios.locationtracker.NotificationVisibility";
	static final String EXTRA_NOTIFICATION_IMPORTANCE = "com.ninevastudios.locationtracker.NotificationImportance";
	static final String EXTRA_NOTIFICATION_HAS_STOP_SERVICE_ACTION = "com.ninevastudios.locationtracker.NotificationHasStopServiceAction";
	static final String EXTRA_NOTIFICATION_STOP_SERVICE_ACTION_NAME = "com.ninevastudios.locationtracker.NotificationStopServiceActionName";
	public static final String EXTRA_LOCATION_REQUEST = "com.ninevastudios.locationtracker.LocationRequest";
	public static final String EXTRA_NOTIFICATION_DATA = "com.ninevastudios.locationtracker.NotificationData";

	private LocationRequest locationRequest;
	private NotificationData notificationData;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		SettingsClient settingsClient = LocationServices.getSettingsClient(this);

		Intent intent = getIntent();

		long interval = intent.getLongExtra(EXTRA_REQUEST_INTERVAL, 600 * 1000L);
		long fastestInterval = intent.getLongExtra(EXTRA_REQUEST_FASTEST_INTERVAL,300 * 1000L);
		int priority = intent.getIntExtra(EXTRA_REQUEST_PRIORITY, LocationRequest.PRIORITY_NO_POWER);
		long maxWaitTime = intent.getLongExtra(EXTRA_REQUEST_MAX_WAIT_TIME, 6000 * 1000L);

		locationRequest = new LocationRequest();
		locationRequest.setInterval(interval);
		locationRequest.setFastestInterval(fastestInterval);
		locationRequest.setPriority(priority);
		locationRequest.setMaxWaitTime(maxWaitTime);

		Log.d("LocationRequest: ", locationRequest.toString());

		LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
		builder.addLocationRequest(locationRequest);
		builder.setAlwaysShow(true);
		LocationSettingsRequest locationSettingsRequest = builder.build();

		notificationData = new NotificationData();
		notificationData.title = intent.getStringExtra(EXTRA_NOTIFICATION_TITLE);
		notificationData.content = intent.getStringExtra(EXTRA_NOTIFICATION_CONTENT);
		notificationData.visibility = intent.getIntExtra(EXTRA_NOTIFICATION_VISIBILITY, 0);
		notificationData.importance = intent.getIntExtra(EXTRA_NOTIFICATION_IMPORTANCE, 3);
		notificationData.hasStopServiceAction = intent.getBooleanExtra(EXTRA_NOTIFICATION_HAS_STOP_SERVICE_ACTION, true);
		notificationData.stopServiceActionTitle = intent.getStringExtra(EXTRA_NOTIFICATION_STOP_SERVICE_ACTION_NAME);

		settingsClient
				.checkLocationSettings(locationSettingsRequest)
				.addOnSuccessListener(new OnSuccessListener<LocationSettingsResponse>() {
					@Override
					public void onSuccess(LocationSettingsResponse locationSettingsResponse) {
						requestBackgroundLocationUpdates();
						Log.d(TAG, "onSuccess -> onSuccess");
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
						Log.d(TAG, "checkLocationSettings -> onCanceled");
						finish();
					}
				});
	}

	@Keep
	public void requestBackgroundLocationUpdates() {
		this.startService(createIntent(this));
		UnityCallbacks.onCheckLocationSettingsSuccess();
	}

	@NonNull
	private Intent createIntent(Context context) {
		Intent intent =  new Intent(context.getApplicationContext(), NinevaLocationService.class);
		intent.putExtra(EXTRA_LOCATION_REQUEST, locationRequest);
		intent.putExtra(EXTRA_NOTIFICATION_DATA, notificationData);
		return intent;
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		super.onActivityResult(requestCode, resultCode, data);

		if (resultCode == RESULT_OK) {
			requestBackgroundLocationUpdates();
			Log.d(TAG, "onActivityResult -> OK");
		} else {
			Log.d(TAG, "onActivityResult -> else");
		}

		finish();
	}
}
