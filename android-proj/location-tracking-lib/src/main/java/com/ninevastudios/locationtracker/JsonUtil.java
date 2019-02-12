package com.ninevastudios.locationtracker;

import android.location.Location;

import org.json.JSONException;
import org.json.JSONObject;

public class JsonUtil {

	private static final String LAT = "lat";
	private static final String LNG = "lng";
	private static final String ACCURACY = "accuracy";
	private static final String ALTITUDE = "altitude";

	public static String serialize(Location location) {
		JSONObject jo = new JSONObject();
		try {
			jo.put(LAT, location.getLatitude());
			jo.put(LNG, location.getLongitude());
			jo.put(ACCURACY, location.getAccuracy());
			jo.put(ALTITUDE, location.getAltitude());
			// TODO serialize all
		} catch (JSONException e) {
			e.printStackTrace();
		}

		return jo.toString();
	}
}
