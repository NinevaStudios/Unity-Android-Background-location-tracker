package com.ninevastudios.locationtracker;

import android.Manifest;
import android.app.Activity;
import android.content.Context;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.support.v4.app.ActivityCompat;

public class RequestPermissionActivity extends Activity {
	private static final int REQUEST_CODE = 666;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		boolean isFine = getIntent().getBooleanExtra("isFine", true);
		String permission = isFine ? Manifest.permission.ACCESS_FINE_LOCATION : Manifest.permission.ACCESS_COARSE_LOCATION;
		String[] permissions = {permission};
		ActivityCompat.requestPermissions(this, permissions, REQUEST_CODE);
	}

	@Override
	public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
		switch (requestCode) {
			case REQUEST_CODE: {
				if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
// TODO callback to unity
				} else {
// TODO callback to unity

				}
				return;
			}
		}

		finish();
	}

	public static boolean hasLocationPermission(Context context) {
		return hasCoarseLocationPermission(context) || hasFineLocationPermission(context);
	}

	private static boolean hasFineLocationPermission(Context context) {
		return ActivityCompat.checkSelfPermission(context, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED;
	}

	private static boolean hasCoarseLocationPermission(Context context) {
		return ActivityCompat.checkSelfPermission(context, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED;
	}
}
