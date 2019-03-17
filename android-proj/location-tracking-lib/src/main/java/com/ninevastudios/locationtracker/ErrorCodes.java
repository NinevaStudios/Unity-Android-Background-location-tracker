package com.ninevastudios.locationtracker;

import android.support.annotation.Keep;

@Keep
public enum ErrorCodes {
	LOCATION_PERMISSION_NOT_GRANTED(1),
	LOCATION_DISABLED(2);

	private int code;

	ErrorCodes(int code) {
		this.code = code;
	}
}
