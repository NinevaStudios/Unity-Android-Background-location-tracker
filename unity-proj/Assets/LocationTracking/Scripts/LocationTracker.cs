using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LocationTracking.Internal;
using Nineva.LocationTracker;
using UnityEngine;

namespace LocationTracking.Scripts
{
	// TODO document each class and menthod!
	[PublicAPI]
	public static class LocationTracker
	{
		const string ExtraRequestInterval = "com.ninevastudios.locationtracker.RequestInterval";
		const string ExtraRequestFastestInterval = "com.ninevastudios.locationtracker.RequestFastestInterval";
		const string ExtraRequestPriority = "com.ninevastudios.locationtracker.RequestPriority";
		const string ExtraRequestMaxWaitTime = "com.ninevastudios.locationtracker.RequestMaxWaitTime";

		const string ExtraNotificationTitle = "com.ninevastudios.locationtracker.NotificationTitle";
		const string ExtraNotificationContent = "com.ninevastudios.locationtracker.NotificationContent";
		const string ExtraNotificationVisibility = "com.ninevastudios.locationtracker.NotificationVisibility";
		const string ExtraNotificationImportance = "com.ninevastudios.locationtracker.NotificationImportance";
		const string ExtraNotificationHasStopServiceAction = "com.ninevastudios.locationtracker.NotificationHasStopServiceAction";
		const string ExtraNotificationStopServiceActionName = "com.ninevastudios.locationtracker.NotificationStopServiceActionName";

		const string ServiceClassName = "com.ninevastudios.locationtracker.NinevaLocationService";
		const string LocationHelperActivityClass = "com.ninevastudios.locationtracker.LocationHelperActivity";

		static Action<Location> _onLocationReceived;
		static Action<string> _onServiceStopped;
		static Action<ErrorCode> _onError;
		static BackgroundTrackingOptions _options;

		public enum ErrorCode
		{
			LocationPermissionNotGranted = 1,
			LocationDisabled = 2
		}

		// TODO implement last known location property from android API
		
		// TODO implement all the properties android goodies has like is location enabled etc. maybe we will rework this to be modular
		
		public static void StartLocationTracking([NotNull] TrackingOptions options, [NotNull] Action<Location> onLocationReceived, Action<ErrorCode> onError = null)
		{
			// TODO implement
		}

		// TODO make it work correctly with error codes (ErrorCode)
		// TODO document, explain how foreground and background works
		public static void StartBackgroundLocationTracking([NotNull] BackgroundTrackingOptions options, [NotNull] Action<Location> onLocationReceived, Action<ErrorCode> onError = null)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}

			if (onLocationReceived == null)
			{
				throw new ArgumentNullException("onLocationReceived");
			}

			_options = options;
			_onLocationReceived = onLocationReceived;
			_onError = onError;

			var intent = GetIntent(options);

			Utils.StartActivity(intent.AJO);
		}

		static AndroidIntent GetIntent(BackgroundTrackingOptions options)
		{
			var intent = new AndroidIntent(Utils.ClassForName(LocationHelperActivityClass));
			intent.SetFlags(AndroidIntent.Flags.ActivityNewTask | AndroidIntent.Flags.ActivityClearTask);

			var request = options.Request;
			intent.PutExtra(ExtraRequestInterval, request.Interval);
			intent.PutExtra(ExtraRequestFastestInterval, request.FastestInterval);
			intent.PutExtra(ExtraRequestPriority, (int) request.Priority);
			intent.PutExtra(ExtraRequestMaxWaitTime, request.MaxWaitTime);

			var notification = options.Notification;
			intent.PutExtra(ExtraNotificationTitle, notification.title);
			intent.PutExtra(ExtraNotificationContent, notification.content);
			intent.PutExtra(ExtraNotificationImportance, (int) notification.importance);
			intent.PutExtra(ExtraNotificationVisibility, (int) notification.visibility);
			intent.PutExtra(ExtraNotificationHasStopServiceAction, notification.hasStopServiceAction);
			intent.PutExtra(ExtraNotificationStopServiceActionName, notification.stopServiceActionName);

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

		/// <summary>
		/// Returns the locations cached in the local database
		/// </summary>
		public static List<Location> PersistedLocations
		{
			get
			{
				var databaseHelper = new AndroidJavaClass(ServiceClassName);
				var str = JavaExtensions.CallStaticStr(databaseHelper, "getAllLocations", Utils.Activity);

				var array = str.Split(';');

				return array.Select(item => new Location(item)).ToList();
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

		// TODO we need to not have this pubic methods here like this because user will confuse them with the API
		public static void OnLocationReceived(Location location)
		{
			if (_onLocationReceived != null)
			{
				_onLocationReceived(location);
			}
		}

		public static void OnServiceStopped(string message)
		{
			if (_onServiceStopped != null)
			{
				_onServiceStopped(message);
			}
		}

		public static void OnPermissionGranted()
		{
			if (_options != null && _onLocationReceived != null)
			{
				StartBackgroundLocationTracking(_options, _onLocationReceived, _onError);
			}
		}

		public static void OnError(string message)
		{
			if (_onError != null)
			{
				// TODO fix with error code
				_onError((ErrorCode) int.Parse(message));
			}
		}
	}
}