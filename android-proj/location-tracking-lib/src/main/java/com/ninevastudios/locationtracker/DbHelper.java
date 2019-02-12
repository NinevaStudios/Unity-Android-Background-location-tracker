package com.ninevastudios.locationtracker;

import android.content.ContentValues;
import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.location.Location;
import android.support.annotation.Keep;
import android.util.Log;

import org.json.JSONException;
import org.json.JSONObject;

@Keep
public class DbHelper extends SQLiteOpenHelper {
	private static final String DATABASE_NAME = "LocationUpdates";
	private static final String TABLE_NAME = "Locations";

	private static final int DATABASE_VERSION = 1;

	private static final String DATABASE_CREATE = "create table " + TABLE_NAME + " ( _id integer primary key, json text not null);";
	private static DbHelper instance;

	public static DbHelper getInstance(Context ctx) {
		if (instance == null) {
			instance = new DbHelper(ctx.getApplicationContext());
		}
		return instance;
	}

	private DbHelper(Context context) {
		super(context, DATABASE_NAME, null, DATABASE_VERSION);
	}

	@Override
	public void onCreate(SQLiteDatabase db) {
		db.execSQL(DATABASE_CREATE);
	}

	@Override
	public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
		Log.w(DbHelper.class.getName(), "Upgrading database from version " + oldVersion + " to " + newVersion);
	}

	public void saveLocation(Location location) {
		ContentValues values = new ContentValues();
		values.put("text", JsonUtil.serialize(location));
		getWritableDatabase().insert(TABLE_NAME, null, values);
	}
}
