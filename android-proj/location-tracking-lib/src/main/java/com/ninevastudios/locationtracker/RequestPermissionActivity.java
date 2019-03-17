package com.ninevastudios.locationtracker;

import android.Manifest;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.support.annotation.Keep;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;

@Keep
public class RequestPermissionActivity extends Activity {
	private static final int REQUEST_PERMISSIONS_REQUEST_CODE = 666;
	private static final String IS_FINE_EXTRA = "isFine";

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		boolean isFine = getIntent().getBooleanExtra(IS_FINE_EXTRA, true);
		String permission = isFine ? Manifest.permission.ACCESS_FINE_LOCATION : Manifest.permission.ACCESS_COARSE_LOCATION;
		String[] permissions = {permission};
		ActivityCompat.requestPermissions(this, permissions, REQUEST_PERMISSIONS_REQUEST_CODE);
	}

	@Override
	public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
		if (requestCode == REQUEST_PERMISSIONS_REQUEST_CODE) {
			String resultJson = JsonUtil.serializePermissionResult(permissions[0], grantResults[0], ActivityCompat.shouldShowRequestPermissionRationale(this, permissions[0]));
			UnityCallbacks.onRequestLocationPermissionResult(resultJson);
		}

		finish();
	}

	@Keep
	public static void requestPermissions(Activity unityActivity, boolean isFine) {
		Intent intent = new Intent(unityActivity, RequestPermissionActivity.class);
		intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		intent.putExtra(IS_FINE_EXTRA, isFine);
		unityActivity.startActivity(intent);
	}

	@Keep
	public static boolean hasLocationPermission(Context context) {
		return hasCoarseLocationPermission(context) || hasFineLocationPermission(context);
	}

	private static boolean hasFineLocationPermission(Context context) {
		return ActivityCompat.checkSelfPermission(context, Manifest.permission.ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED;
	}

	private static boolean hasCoarseLocationPermission(Context context) {
		return ActivityCompat.checkSelfPermission(context, Manifest.permission.ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED;
	}
}
