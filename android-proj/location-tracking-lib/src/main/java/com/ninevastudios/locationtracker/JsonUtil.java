package com.ninevastudios.locationtracker;

import android.location.Location;
import android.os.Build;

import org.json.JSONException;
import org.json.JSONObject;

public class JsonUtil {

	private static final String LAT = "lat";
	private static final String LNG = "lng";
	private static final String ACCURACY = "accuracy";
	private static final String ALTITUDE = "altitude";
	private static final String BEARING = "bearing";
	private static final String BEARING_ACCURACY = "bearingAccuracy";
	private static final String SPEED = "speed";
	private static final String SPEED_ACCURACY = "speedAccuracy";
	private static final String VERTICAL_ACCURACY = "verticalAccuracy";
	private static final String TIME = "time";
	private static final String ELAPSED_TIME = "elapsedTime";
	private static final String HAS_ACCURACY = "hasAccuracy";
	private static final String HAS_ALTITUDE = "hasAltitude";
	private static final String HAS_BEARING = "hasBearing";
	private static final String HAS_BEARING_ACCURACY = "hasBearingAccuracy";
	private static final String HAS_SPEED = "hasSpeed";
	private static final String HAS_SPEED_ACCURACY = "hasSpeedAccuracy";
	private static final String HAS_VERTICAL_ACCURACY = "hasVerticalAccuracy";
	private static final String IS_FROM_MOCK_PROVIDER = "isFromMockProvider";

	public static String serializeLocation(Location location) {
		JSONObject jo = new JSONObject();
		try {
			jo.put(LAT, location.getLatitude());
			jo.put(LNG, location.getLongitude());
			jo.put(ACCURACY, location.getAccuracy());
			jo.put(ALTITUDE, location.getAltitude());
			jo.put(BEARING, location.getBearing());
			jo.put(SPEED, location.getSpeed());
			jo.put(TIME, location.getTime());
			jo.put(HAS_ACCURACY, location.hasAccuracy());
			jo.put(HAS_ALTITUDE, location.hasAltitude());
			jo.put(HAS_BEARING, location.hasBearing());
			jo.put(HAS_SPEED, location.hasSpeed());

			if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1) {
				jo.put(ELAPSED_TIME, location.getElapsedRealtimeNanos());
			}

			if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR2) {
				jo.put(IS_FROM_MOCK_PROVIDER, location.isFromMockProvider());
			}

			if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
				jo.put(HAS_SPEED_ACCURACY, location.hasSpeedAccuracy());
				jo.put(SPEED_ACCURACY, location.getSpeedAccuracyMetersPerSecond());
				jo.put(HAS_BEARING_ACCURACY, location.hasBearingAccuracy());
				jo.put(BEARING_ACCURACY, location.getBearingAccuracyDegrees());
				jo.put(HAS_VERTICAL_ACCURACY, location.hasVerticalAccuracy());
				jo.put(VERTICAL_ACCURACY, location.getVerticalAccuracyMeters());
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}

		return jo.toString();
	}

	private static final String PERMISSION_RESULT = "result";
	private static final String PERMISSION_SHOULD_SHOW_REQUEST_PERMISSION_RATIONALE = "shouldShowRequestPermissionRationale";
	private static final String PERMISSION = "permission";

	static String serializePermissionResult(String permission, int grantResult, boolean shouldShowRequestPermissionRationale) {
		JSONObject resultJson = new JSONObject();
		try {
			resultJson.put(PERMISSION, permission);
			resultJson.put(PERMISSION_RESULT, grantResult);
			resultJson.put(PERMISSION_SHOULD_SHOW_REQUEST_PERMISSION_RATIONALE, shouldShowRequestPermissionRationale);
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return resultJson.toString();
	}
}
