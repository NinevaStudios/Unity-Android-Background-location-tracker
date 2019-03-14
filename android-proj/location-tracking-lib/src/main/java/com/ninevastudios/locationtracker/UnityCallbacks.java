package com.ninevastudios.locationtracker;

import com.unity3d.player.UnityPlayer;

public class UnityCallbacks {

	private static final String UNITY_SCENE_HELPER_GAME_OBJECT_NAME = "LocationTrackingSceneHelper";

	public static void onCheckLocationSettingsSuccess() {
		sendMessage("OnCheckLocationSettingsSuccess", "Location Settings Check Success");
	}

	public static void onCheckLocationSettingsCancelled() {
		sendMessage("OnCheckLocationSettingsCancelled", "Location Settings Check Cancelled");
	}

	public static void onCheckLocationSettingsFailed(String message) {
		sendMessage("OnCheckLocationSettingsFailed", message);
	}

	public static void onLocationReceived(String locationJson) {
		sendMessage("OnLocationReceived", locationJson);
	}

	public static void onServiceStopped(String message) {
		sendMessage("OnServiceStopped", message);
	}

	public static void onPermissionGranted() {
		sendMessage("OnPermissionGranted", "Permission Granted");
	}

	public static void onPermissionDenied() {
		sendMessage("OnPermissionDenied", "Permission Denied");
	}

	private static void sendMessage(String method, String msg) {
		UnityPlayer.UnitySendMessage(UNITY_SCENE_HELPER_GAME_OBJECT_NAME, method, msg);
	}
}
