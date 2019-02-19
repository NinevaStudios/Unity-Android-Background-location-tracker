using System;
using LocationTracking.Internal;

namespace LocationTracking.Scripts
{
	public static class LocationTracker
	{
		const string ExtraRequestInterval = "com.ninevastudios.locationtracker.RequestInterval";
		const string ExtraRequestFastestInterval = "com.ninevastudios.locationtracker.RequestFastestInterval";
		const string ExtraRequestPriority = "com.ninevastudios.locationtracker.RequestPriority";
		const string ExtraRequestMaxWaitTime = "com.ninevastudios.locationtracker.RequestMaxWaitTime";

		static Action<Location> _onLocationReceived;
		
		public static void StartLocationTracking(LocationRequest request, Action<Location> onLocationReceived, Action<string> onError = null)
		{
			_onLocationReceived = onLocationReceived;
			
			var intent = new AndroidIntent(Utils.ClassForName("com.ninevastudios.locationtracker.LocationHelperActivity"));
			intent.SetFlags(AndroidIntent.Flags.ActivityNewTask | AndroidIntent.Flags.ActivityClearTask);
			intent.PutExtra(ExtraRequestInterval, request.interval);
			intent.PutExtra(ExtraRequestFastestInterval, request.fastestInterval);
			intent.PutExtra(ExtraRequestPriority, (int) request.priority);
			intent.PutExtra(ExtraRequestMaxWaitTime, request.maxWaitTime);
		
			Utils.StartActivity(intent.AJO);
		}
		
		// get list from db
		// clean db
		// stop tracking
		
		public static void OnLocationReceived(Location location)
		{
			if(_onLocationReceived != null)
			_onLocationReceived(location);
		}
	}
}