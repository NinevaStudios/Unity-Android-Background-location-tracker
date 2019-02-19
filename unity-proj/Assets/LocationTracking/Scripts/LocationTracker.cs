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

		public static Action<Location> onSuccess;
		public static void RegisterLocationTrackingService(LocationRequest request)
		{
			var intent = new AndroidIntent(Utils.ClassForName("com.ninevastudios.locationtracker.LocationHelperActivity"));
			intent.SetFlags(AndroidIntent.Flags.ActivityNewTask | AndroidIntent.Flags.ActivityClearTask);
			intent.PutExtra(ExtraRequestInterval, request.interval);
			intent.PutExtra(ExtraRequestFastestInterval, request.fastestInterval);
			intent.PutExtra(ExtraRequestPriority, (int) request.priority);
			intent.PutExtra(ExtraRequestMaxWaitTime, request.maxWaitTime);
		
			Utils.StartActivity(intent.AJO);
		}
		
		public static void OnLocationReceived(Location location)
		{
			onSuccess?.Invoke(location);
		}
	}
}