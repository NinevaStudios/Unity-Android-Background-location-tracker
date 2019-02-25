package com.ninevastudios.locationtracker;

import android.annotation.SuppressLint;
import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.location.Location;
import android.os.Build;
import android.os.IBinder;
import android.os.Looper;
import android.support.annotation.Keep;
import android.support.annotation.NonNull;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationCallback;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationResult;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.location.LocationSettingsRequest;
import com.google.android.gms.location.LocationSettingsResponse;
import com.google.android.gms.tasks.OnCanceledListener;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;

import java.util.ArrayList;

@Keep
public class NinevaLocationService extends Service {

	private static final String TAG = NinevaLocationService.class.getSimpleName();
	private static final String ACTION_STOP = "com.ninevastudios.locationtracker.StopService";

	protected LocationSettingsRequest locationSettingsRequest;
	private FusedLocationProviderClient fusedLocationClient;
	private LocationCallback locationCallback;
	private LocationRequest locationRequest;

	@Override
	public void onCreate() {
		super.onCreate();
	}

	@Override
	public IBinder onBind(Intent intent) {
		return null;
	}

	@Override
	public int onStartCommand(Intent intent, int flags, int startId) {
		super.onStartCommand(intent, flags, startId);
		Log.d(TAG, "onStartCommand");
		if (ACTION_STOP.equals(intent.getAction())) {
			Log.d(TAG, "ActionDestroy");
			stopSelf();
			return START_STICKY;
		}


		locationRequest = intent.getParcelableExtra(LocationHelperActivity.EXTRA_LOCATION_REQUEST);

		createForegroundNotification();

		init();

		return START_STICKY;
	}

	private void init() {
		fusedLocationClient = LocationServices.getFusedLocationProviderClient(this);

		locationCallback = new LocationCallback() {
			@Override
			public void onLocationResult(LocationResult locationResult) {
				super.onLocationResult(locationResult);

				Location location = locationResult.getLastLocation();
				UnityCallbacks.onLocationReceived(JsonUtil.serialize(location));

				// TODO save location to sqlite db
				DbHelper.getInstance(getApplicationContext()).saveLocation(location);

				Log.d(TAG, "Location Received " + locationResult);
			}
		};

		LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
		builder.addLocationRequest(locationRequest);
		builder.setAlwaysShow(true);
		locationSettingsRequest = builder.build();

		LocationServices.getSettingsClient(this)
				.checkLocationSettings(locationSettingsRequest)
				.addOnSuccessListener(new OnSuccessListener<LocationSettingsResponse>() {
					@Override
					public void onSuccess(LocationSettingsResponse locationSettingsResponse) {
						requestLocationUpdates();
					}
				})
				.addOnFailureListener(new OnFailureListener() {
					@Override
					public void onFailure(@NonNull Exception e) {
						Log.d(TAG, "checkLocationSettings -> onFailure");
					}
				})
				.addOnCanceledListener(new OnCanceledListener() {
					@Override
					public void onCanceled() {
						Log.d(TAG, "checkLocationSettings -> onCanceled");
					}
				});
	}

	@Keep
	public static int getNumberOfRows(Context context)
	{
		return DbHelper.getInstance(context.getApplicationContext()).numberOfRows();
	}

	@Keep
	public static ArrayList<String> getAllLocations(Context context)
	{
		return DbHelper.getInstance(context.getApplicationContext()).getAllLocations();
	}

	@Keep
	public static void deleteAllEntries(Context context)
	{
		DbHelper.getInstance(context.getApplicationContext()).deleteAllEntries();
	}

	@SuppressLint("MissingPermission")
	private void requestLocationUpdates() {
		if (RequestPermissionActivity.hasLocationPermission(this.getApplicationContext())) {
			fusedLocationClient.requestLocationUpdates(locationRequest, locationCallback, Looper.myLooper());
		} else {
			Log.e(TAG, "You must request location permissions before requesting location updates");
		}
	}

	@Override
	public void onDestroy() {
		super.onDestroy();

		if (fusedLocationClient != null) {
			fusedLocationClient.removeLocationUpdates(locationCallback);
			Log.d(TAG, "Location Update Callback Removed");
		}

		UnityCallbacks.onServiceStopped("The service was successfully stopped!");
	}

	private void createForegroundNotification() {
		String CHANNEL_ID = "channel_location";
		String CHANNEL_NAME = "channel_location";

		NotificationCompat.Builder builder;
		NotificationManager notificationManager = (NotificationManager) getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
			NotificationChannel channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationManager.IMPORTANCE_DEFAULT);
			channel.setLockscreenVisibility(Notification.VISIBILITY_PRIVATE);
			notificationManager.createNotificationChannel(channel);
			builder = new NotificationCompat.Builder(getApplicationContext(), CHANNEL_ID);
			builder.setChannelId(CHANNEL_ID);
			builder.setBadgeIconType(NotificationCompat.BADGE_ICON_NONE);
		} else {
			builder = new NotificationCompat.Builder(getApplicationContext(), CHANNEL_ID);
		}

		builder.setContentTitle("Your title");
		builder.setContentText("You are now online");
		builder.setSmallIcon(R.drawable.common_google_signin_btn_icon_dark);

		Intent intentHide = new Intent(getApplicationContext(), NinevaLocationService.class);
		intentHide.setAction(ACTION_STOP);
		PendingIntent hide = PendingIntent.getBroadcast(getApplicationContext(), 0, intentHide, PendingIntent.FLAG_CANCEL_CURRENT);
		builder.addAction(0, "Stop service", hide);

		// TODO start the app
//		builder.setContentIntent(pendingIntent);
		Notification notification = builder.build();
		startForeground(101, notification);
	}
}
