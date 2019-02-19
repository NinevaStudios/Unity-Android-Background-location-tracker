package com.ninevastudios.locationtracker;

import com.unity3d.player.UnityPlayer;

public class UnityCallbacks {

	private static final String UNITY_SCENE_HELPER_GAME_OBJECT_NAME = "LocationTrackingSceneHelper";

	public static void onCheckLocationSettingsSuccess() {
		sendMessage("OnCheckLocationSettingsSuccess", "TODO");
	}

	public static void onCheckLocationSettingsFailed() {
		sendMessage("OnCheckLocationSettingsFailed", "TODO");
	}

	public static void onLocationReceived(String locationJson) {
		sendMessage("OnLocationReceived", locationJson);
	}

	private static void sendMessage(String method, String msg) {
		UnityPlayer.UnitySendMessage(UNITY_SCENE_HELPER_GAME_OBJECT_NAME, method, msg);
	}
}
