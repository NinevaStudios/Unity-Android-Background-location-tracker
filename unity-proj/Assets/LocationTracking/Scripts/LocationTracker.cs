using System;
using LocationTracking.Internal;
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
		
		public static void StartLocationTracking(TrackingOptions options, Action<Location> onLocationReceived, Action<string> onError = null)
		{
			_onLocationReceived = onLocationReceived;
			
			var intent = new AndroidIntent(Utils.ClassForName("com.ninevastudios.locationtracker.LocationHelperActivity"));
			intent.SetFlags(AndroidIntent.Flags.ActivityNewTask | AndroidIntent.Flags.ActivityClearTask);
			intent.PutExtra(ExtraRequestInterval, options.request.interval);
			intent.PutExtra(ExtraRequestFastestInterval, options.request.fastestInterval);
			intent.PutExtra(ExtraRequestPriority, (int) options.request.priority);
			intent.PutExtra(ExtraRequestMaxWaitTime, options.request.maxWaitTime);
		
			Utils.StartActivity(intent.AJO);
		}
		
		// get list from db
		// clean db
		// stop tracking

		public static void StopLocationTracking(Action<string> onServiceStopped = null)
		{
			_onServiceStopped = onServiceStopped;
			
			Utils.Activity.CallBool("stopService", new AndroidIntent(new AndroidJavaClass(ServiceClassName)).AJO);
		}
		
		public static void OnLocationReceived(Location location)
		{
			if(_onLocationReceived != null)
			_onLocationReceived(location);
		}

		public static void OnServiceStopped(string message)
		{
			_onServiceStopped(message);
		}
	}
}