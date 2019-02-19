using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LocationTracking.Internal;
using MiniJSON;

namespace LocationTracking.Scripts
{
	[PublicAPI]
	public class Location
	{
		const string Lat = "lat";
		const string Lng = "lng";
		const string Accuracy = "accuracy";
		const string Altitude = "altitude";
		const string Bearing = "bearing";
		const string BearingAccuracy = "bearingAccuracy";
		const string Speed = "speed";
		const string SpeedAccuracy = "speedAccuracy";
		const string VerticalAccuracy = "verticalAccuracy";
		const string Time = "time";
		const string ElapsedTime = "elapsedTime";

		const string HasAccuracy = "hasAccuracy";
		const string HasAltitude = "hasAltitude";
		const string HasBearing = "hasBearing";
		const string HasBearingAccuracy = "hasBearingAccuracy";
		const string HasSpeed = "hasSpeed";
		const string HasSpeedAccuracy = "hasSpeedAccuracy";
		const string HasVerticalAccuracy = "hasVerticalAccuracy";
		const string IsFromMockProvider = "isFromMockProvider";

		public double latitude;
		public double longitude;
		public float accuracy;
		public float altitude;
		public float bearing;
		public float speed;
		public long time;

		/// <summary>
		/// API level 17
		/// </summary>
		public long elapsedRealTime;

		/// <summary>
		/// API level 18
		/// </summary>
		public bool isFromMockProvider;

		/// <summary>
		/// API level 26
		/// </summary>
		public float speedAccuracy;

		/// <summary>
		/// API level 26
		/// </summary>
		public float bearingAccuracy;

		/// <summary>
		/// API level 26
		/// </summary>
		public float verticalAccuracy;

		public bool hasAccuracy;
		public bool hasAltitude;
		public bool hasBearing;
		public bool hasSpeed;

		/// <summary>
		/// API level 26
		/// </summary>
		public bool hasBearingAccuracy;

		/// <summary>
		/// API level 26
		/// </summary>
		public bool hasSpeedAccuracy;

		/// <summary>
		/// API level 26
		/// </summary>
		public bool hasVerticalAccuracy;

		public Location(string serializedLocation)
		{
			var dic = Json.Deserialize(serializedLocation) as Dictionary<string, object>;
			latitude = dic.GetDouble(Lat);
			longitude = dic.GetDouble(Lng);

			hasAccuracy = dic.GetBool(HasAccuracy);
			if (hasAccuracy) accuracy = dic.GetFloat(Accuracy);

			hasAltitude = dic.GetBool(HasAltitude);
			if (hasAltitude) altitude = dic.GetFloat(Altitude);

			hasBearing = dic.GetBool(HasBearing);
			if (hasBearing) bearing = dic.GetFloat(Bearing);

			hasSpeed = dic.GetBool(HasSpeed);
			if (hasSpeed) speed = dic.GetFloat(Speed);

			time = dic.GetLong(Time);
			elapsedRealTime = dic.GetLong(ElapsedTime);

			hasSpeedAccuracy = dic.GetBool(HasSpeedAccuracy);
			if (hasSpeedAccuracy) speedAccuracy = dic.GetFloat(SpeedAccuracy);

			hasBearingAccuracy = dic.GetBool(HasBearingAccuracy);
			if (hasBearingAccuracy) bearingAccuracy = dic.GetFloat(BearingAccuracy);

			hasVerticalAccuracy = dic.GetBool(HasVerticalAccuracy);
			if (hasVerticalAccuracy) verticalAccuracy = dic.GetFloat(VerticalAccuracy);

			isFromMockProvider = dic.GetBool(IsFromMockProvider);
		}

		/// <summary>
		/// Returns the approximate distance in meters between this location and the given location.
		/// </summary>
		/// <param name="destination">Location to get distance to</param>
		[PublicAPI]
		public float DistanceTo(Location destination)
		{
			var result = new float[1];
			DistanceBetween(latitude, longitude, destination.latitude, destination.longitude, result);
			return result[0];
		}

		/// <summary>
		/// Computes the approximate distance in meters between two locations, and optionally the initial and final bearings of the shortest path between them.
		/// Distance and bearing are defined using the WGS84 ellipsoid.
		/// 
		/// The computed distance is stored in results[0]. 
		/// If results has length 2 or greater, the initial bearing is stored in results[1]. 
		/// If results has length 3 or greater, the final bearing is stored in results[2].
		/// </summary>
		public static void DistanceBetween(double startLatitude, double startLongitude, double endLatitude, double endLongitude, float[] results)
		{
			if (results == null || results.Length < 1)
			{
				throw new ArgumentException("results is null or has length < 1");
			}

			LocationUtils.ComputeDistanceAndBearing(startLatitude, startLongitude, endLatitude, endLongitude, results);
		}

		public override string ToString()
		{
			return string.Format("[Location: Latitude = {0}, Longitude = {1}, HasAccuracy = {2}, Accuracy = {3}, Time = {4}, HasSpeed = {5}, Speed = {6}, " +
			                     "HasBearing = {7}, Bearing = {8}, HasAltitude = {9}, Altitude = {10}, IsFromMockProvider = {11}, ElapsedRealTime = {12}, " +
			                     "HasBearingAccuracy = {13}, BearingAccuracy = {14}, HasVerticalAccuracy = {15}, VerticalAccuracy = {16}, " +
			                     "HasSpeedAccuracy = {17}, SpeedAccuracy = {18}]", latitude, longitude, hasAccuracy, accuracy, time, hasSpeed, speed,
				hasBearing, bearing, hasAltitude, altitude, isFromMockProvider, elapsedRealTime, hasBearingAccuracy, bearingAccuracy,
				hasVerticalAccuracy, verticalAccuracy, hasSpeedAccuracy, speedAccuracy);
		}
	}
}