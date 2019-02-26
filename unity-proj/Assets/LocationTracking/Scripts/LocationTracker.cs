using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LocationTracking.Internal;
using Nineva.LocationTracker;
using UnityEngine;

namespace LocationTracking.Scripts
{
	public static class LocationTracker
	{
		const string ExtraRequestInterval = "com.ninevastudios.locationtracker.RequestInterval";
		const string ExtraRequestFastestInterval = "com.ninevastudios.locationtracker.RequestFastestInterval";
		const string ExtraRequestPriority = "com.ninevastudios.locationtracker.RequestPriority";
		const string ExtraRequestMaxWaitTime = "com.ninevastudios.locationtracker.RequestMaxWaitTime";
		
		const string ServiceClassName = "com.ninevastudios.locationtracker.NinevaLocationService";

		static Action<Location> _onLocationReceived;
		static Action<string> _onServiceStopped;
		
		public static void StartLocationTracking([NotNull] TrackingOptions options, Action<Location> onLocationReceived, Action<string> onError = null)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}

			_onLocationReceived = onLocationReceived;
			
			var intent = GetIntent(options);

			Utils.StartActivity(intent.AJO);
		}

		static AndroidIntent GetIntent(TrackingOptions options)
		{
			var intent = new AndroidIntent(Utils.ClassForName("com.ninevastudios.locationtracker.LocationHelperActivity"));
			intent.SetFlags(AndroidIntent.Flags.ActivityNewTask | AndroidIntent.Flags.ActivityClearTask);
			intent.PutExtra(ExtraRequestInterval, options.Request.Interval);
			intent.PutExtra(ExtraRequestFastestInterval, options.Request.FastestInterval);
			intent.PutExtra(ExtraRequestPriority, (int) options.Request.Priority);
			intent.PutExtra(ExtraRequestMaxWaitTime, options.Request.MaxWaitTime);
			return intent;
		}

		/// <summary>
		/// Count of saved locations in the local database
		/// </summary>
		public static int SavedLocationsCount
		{
			get
			{
				var databaseHelper = new AndroidJavaClass(ServiceClassName);
				return JavaExtensions.CallStaticInt(databaseHelper, "getNumberOfRows", Utils.Activity);
			}
		}

		public static List<Location> PersistedLocations
		{
			get
			{
				var locationsList = new List<Location>();
				
				var databaseHelper = new AndroidJavaClass(ServiceClassName);
				var listAjo = JavaExtensions.CallStaticAJO(databaseHelper, "getAllLocations", Utils.Activity);
				var list = listAjo.FromJavaList<string>();
				foreach (var entry in list)
				{
					locationsList.Add(new Location(entry));
				}
				
				return locationsList;
			}
		}

		/// <summary>
		/// Delete all persisted locations from the local database
		/// </summary>
		public static void CleanDatabase()
		{
			var databaseHelper = new AndroidJavaClass(ServiceClassName);
			databaseHelper.CallStatic("deleteAllEntries", Utils.Activity);
		}

		public static void StopLocationTracking(Action<string> onServiceStopped = null)
		{
			_onServiceStopped = onServiceStopped;
			
			JavaExtensions.CallBool(Utils.Activity, "stopService", new AndroidIntent(new AndroidJavaClass(ServiceClassName)).AJO);
		}
		
		public static void OnLocationReceived(Location location)
		{
			if(_onLocationReceived != null)
			_onLocationReceived(location);
		}

		public static void OnServiceStopped(string message)
		{
			if(_onServiceStopped != null)
			_onServiceStopped(message);
		}
	}
}