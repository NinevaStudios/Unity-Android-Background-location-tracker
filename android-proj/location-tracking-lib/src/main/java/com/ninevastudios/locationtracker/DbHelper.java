package com.ninevastudios.locationtracker;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.DatabaseUtils;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.location.Location;
import android.support.annotation.Keep;
import android.util.Log;

import java.util.ArrayList;
import java.util.List;

@Keep
public class DbHelper extends SQLiteOpenHelper {
	private static final String DATABASE_NAME = "LocationUpdates";
	private static final String TABLE_NAME = "Locations";
	private static final String COLUMN_NAME = "Value";

	private static final int DATABASE_VERSION = 1;

	private static final String DATABASE_CREATE = "create table " + TABLE_NAME + " ( _id integer primary key, Value text not null);";
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
		// TODO be consistent with query uppercase-lowercase!
		db.execSQL("DROP TABLE IF EXISTS " + TABLE_NAME);
		onCreate(db);
	}

	public void saveLocation(Location location) {
		ContentValues values = new ContentValues();
		values.put(COLUMN_NAME, JsonUtil.serializeLocation(location));
		SQLiteDatabase db = getWritableDatabase();
		db.insert(TABLE_NAME, null, values);
	}

	public int numberOfRows() {
		SQLiteDatabase db = getReadableDatabase();
		return (int) DatabaseUtils.queryNumEntries(db, TABLE_NAME);
	}

	public List<String> getAllLocations() {
		ArrayList<String> result = new ArrayList<>();
		SQLiteDatabase db = getReadableDatabase();
		Cursor res = db.rawQuery("select * from " + TABLE_NAME, null);
		res.moveToFirst();

		while (!res.isAfterLast()) {
			result.add(res.getString(res.getColumnIndex(COLUMN_NAME)));
			res.moveToNext();
		}

		res.close();
		return result;
	}

	public void deleteAllEntries() {
		SQLiteDatabase db = getWritableDatabase();
		db.delete(TABLE_NAME, null, null);
	}
}
