package com.ninevastudios.locationtracker;

import android.annotation.SuppressLint;
import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.os.Build;
import android.os.IBinder;
import android.os.Looper;
import android.support.annotation.Keep;
import android.support.annotation.NonNull;
import android.support.v4.app.NotificationCompat;
import android.text.TextUtils;
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

import java.util.List;

@Keep
public class NinevaLocationService extends Service {

	private static final String TAG = NinevaLocationService.class.getSimpleName();
	private static final String ACTION_STOP = "com.ninevastudios.locationtracker.StopService";

	protected LocationSettingsRequest locationSettingsRequest;
	private FusedLocationProviderClient fusedLocationClient;
	private LocationCallback locationCallback;
	private LocationRequest locationRequest;
	private Notification notification;

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
		Log.d(TAG, "onStartCommand");

		if (intent.getAction() != null && intent.getAction().equals(ACTION_STOP)) {
			Log.d(TAG, "onStartCommand - STOP");
			Intent stopIntent = new Intent(getApplicationContext(), NinevaLocationService.class);
			stopService(stopIntent);
		} else {
			locationRequest = intent.getParcelableExtra(LocationHelperActivity.EXTRA_LOCATION_REQUEST);
			NotificationData notificationData = intent.getParcelableExtra(LocationHelperActivity.EXTRA_NOTIFICATION_DATA);
			createForegroundNotification(notificationData);
			init();
		}

		return super.onStartCommand(intent, flags, startId);
	}

	private void init() {
		if (RequestPermissionActivity.hasLocationPermission(this.getApplicationContext())) {
			fusedLocationClient = LocationServices.getFusedLocationProviderClient(this);
		} else {
			startRequestPermissionActivity();
		}
		locationCallback = new LocationCallback() {
			@Override
			public void onLocationResult(LocationResult locationResult) {
				super.onLocationResult(locationResult);

				Location location = locationResult.getLastLocation();
				UnityCallbacks.onLocationReceived(JsonUtil.serializeLocation(location));

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
						UnityCallbacks.onCheckLocationSettingsFailed();
					}
				})
				.addOnCanceledListener(new OnCanceledListener() {
					@Override
					public void onCanceled() {
						UnityCallbacks.onCheckLocationSettingsCancelled();
					}
				});
	}

	// TODO move these methods to DbHelper!
	@Keep
	public static int getNumberOfRows(Context context) {
		return DbHelper.getInstance(context.getApplicationContext()).numberOfRows();
	}

	@Keep
	public static String getAllLocations(Context context) {
		List<String> locationsArray = DbHelper.getInstance(context.getApplicationContext()).getAllLocations();
		return TextUtils.join(";", locationsArray);
	}

	@Keep
	public static void deleteAllEntries(Context context) {
		DbHelper.getInstance(context.getApplicationContext()).deleteAllEntries();
	}

	@SuppressLint("MissingPermission")
	private void requestLocationUpdates() {
		if (RequestPermissionActivity.hasLocationPermission(this.getApplicationContext())) {
			fusedLocationClient.requestLocationUpdates(locationRequest, locationCallback, Looper.myLooper());
			startForeground(101, notification);
		} else {
			startRequestPermissionActivity();
		}
	}

	private void startRequestPermissionActivity() {
		Intent requestPermissionIntent = new Intent(this, RequestPermissionActivity.class);
		requestPermissionIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		startActivity(requestPermissionIntent);
		stopSelf();
	}

	@Override
	public void onDestroy() {
		super.onDestroy();

		if (fusedLocationClient != null) {
			fusedLocationClient.removeLocationUpdates(locationCallback);
			Log.d(TAG, "Location Update Callback Removed");
		}
	}

	private void createForegroundNotification(NotificationData notificationData) {
		String CHANNEL_ID = "channel_location_id";
		String CHANNEL_NAME = "channel_location_name";

		NotificationCompat.Builder builder;
		NotificationManager notificationManager = (NotificationManager) getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
			NotificationChannel channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, notificationData.importance);
			channel.setLockscreenVisibility(notificationData.visibility);

			notificationManager.createNotificationChannel(channel);
			builder = new NotificationCompat.Builder(getApplicationContext(), CHANNEL_ID);
			builder.setBadgeIconType(NotificationCompat.BADGE_ICON_NONE);
			builder.setCategory(NotificationCompat.CATEGORY_SERVICE);
		} else {
			builder = new NotificationCompat.Builder(getApplicationContext(), CHANNEL_ID);
		}

		builder.setContentTitle(notificationData.title);
		builder.setContentText(notificationData.content);
		builder.setSmallIcon(R.drawable.common_google_signin_btn_icon_dark);

		if (notificationData.hasStopServiceAction) {
			Intent intentHide = new Intent(this, NinevaLocationService.class);
			intentHide.setAction(ACTION_STOP);
			PendingIntent hide = PendingIntent.getService(this, 0, intentHide, 0);
			builder.addAction(0, notificationData.stopServiceActionTitle, hide);
		}

		PackageManager pm = this.getPackageManager();
		Intent openAppIntent = pm.getLaunchIntentForPackage(getPackageName());

		if (openAppIntent != null) {
			openAppIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP);
			PendingIntent contentIntent = PendingIntent.getActivity(this, 0, openAppIntent, PendingIntent.FLAG_UPDATE_CURRENT);
			builder.setContentIntent(contentIntent);
		}

		notification = builder.build();
	}
}
