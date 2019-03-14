package com.ninevastudios.locationtracker;

import android.os.Parcel;
import android.os.Parcelable;

public class NotificationData implements Parcelable {
	public String title;
	public String content;
	public int importance;
	public int visibility;
	public boolean hasStopServiceAction;
	public String stopServiceActionTitle;

	public NotificationData() {

	}

	protected NotificationData(Parcel in) {
		title = in.readString();
		content = in.readString();
		importance = in.readInt();
		visibility = in.readInt();
		hasStopServiceAction = in.readByte() != 0;
		stopServiceActionTitle = in.readString();
	}

	public static final Creator<NotificationData> CREATOR = new Creator<NotificationData>() {
		@Override
		public NotificationData createFromParcel(Parcel in) {
			return new NotificationData(in);
		}

		@Override
		public NotificationData[] newArray(int size) {
			return new NotificationData[size];
		}
	};

	@Override
	public int describeContents() {
		return 0;
	}

	@Override
	public void writeToParcel(Parcel dest, int flags) {
		dest.writeString(title);
		dest.writeString(content);
		dest.writeInt(importance);
		dest.writeInt(visibility);
		dest.writeByte((byte) (hasStopServiceAction ? 1 : 0));
		dest.writeString(stopServiceActionTitle);
	}
}
