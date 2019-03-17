package com.ninevastudios.locationtracker;

import com.unity3d.player.UnityPlayer;

public class UnityCallbacks {

	private static final String UNITY_SCENE_HELPER_GAME_OBJECT_NAME = "LocationTrackingSceneHelper";
	private static final String EMPTY = "";

	public static void onCheckLocationSettingsSuccess() {
		sendMessage("OnCheckLocationSettingsSuccess", EMPTY);
	}

	public static void onCheckLocationSettingsCancelled() {
		sendMessage("OnCheckLocationSettingsCancelled", EMPTY);
	}

	public static void onCheckLocationSettingsFailed() {
		sendMessage("OnCheckLocationSettingsFailed", EMPTY);
	}

	public static void onLocationReceived(String locationJson) {
		sendMessage("OnLocationReceived", locationJson);
	}

	public static void onPermissionGranted() {
		sendMessage("OnPermissionGranted", "Permission Granted");
	}

	public static void onRequestLocationPermissionResult(String serializedResult) {
		sendMessage("OnRequestLocationPermissionResult", serializedResult);
	}

	public static void onPermissionDenied() {
		sendMessage("OnPermissionDenied", "Permission Denied");
	}

	private static void sendMessage(String method, String msg) {
		UnityPlayer.UnitySendMessage(UNITY_SCENE_HELPER_GAME_OBJECT_NAME, method, msg);
	}
}
