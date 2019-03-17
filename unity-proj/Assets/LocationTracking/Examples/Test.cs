using JetBrains.Annotations;
using LocationTracking.Scripts;
using UnityEngine;
using UnityEngine.UI;

// TODO rename this class to LocationTrackingExample.cs
public class Test : MonoBehaviour
{
	[SerializeField] Text locationText, numberText;

	int _ticks;

	// TODO separate examples for background and foreground tracking
	[UsedImplicitly]
	public void OnStartTracking()
	{
		StartBackgroundLocationTracking();
	}

	[UsedImplicitly]
	public void OnStopTracking()
	{
		LocationTracker.StopLocationTracking();
		_ticks = 0;
	}

	[UsedImplicitly]
	public void OnClearDatabase()
	{
		LocationTracker.CleanDatabase();
	}

	[UsedImplicitly]
	public void OnPrintAllLocations()
	{
		locationText.text = "";
		locationText.horizontalOverflow = HorizontalWrapMode.Overflow;
		foreach (var location in LocationTracker.PersistedLocations)
		{
			locationText.text += location + "\n";
		}
	}

	public void OnRequestBackgroundLocationUpdates()
	{
		if (LocationPermissionHelper.IsLocationPermissionGranted)
		{
			TryRequestLocationUpdates();
		}
		else
		{
			LocationPermissionHelper.RequestLocationPermission(result =>
			{
				if (result.Status == LocationPermissionHelper.PermissionStatus.Granted)
				{
					// Permission was granted, we can continue
					TryRequestLocationUpdates();
				}
				else
				{
					// User denied permission, now we need to find out if he clicked "Do not show again" checkbox
					if (result.ShouldShowRequestPermissionRationale)
					{
						Debug.Log("User just denied permission, we can show explanation here and request permissions again or send user to settings to do so");
					}
					else
					{
						Debug.Log("User checked Do not show again checkbox or permission can't be granted. We should continue with this permission denied");
					}
				}
			});
		}
	}

	void TryRequestLocationUpdates()
	{
		if (LocationTracker.IsLocationEnabled)
		{
			StartBackgroundLocationTracking();
		}
		else
		{
			LocationTracker.TryEnableLocationService(StartBackgroundLocationTracking, error =>
			{
				Debug.Log("Failed to enable location");
			});
		}
	}

	void StartBackgroundLocationTracking()
	{
		var request = CreateLocationRequest();

		var options = new BackgroundTrackingOptions(request, new Notification());

		LocationTracker.StartBackgroundLocationTracking(options, location =>
		{
			locationText.horizontalOverflow = HorizontalWrapMode.Wrap;
			locationText.text = ++_ticks + ". " + location.ToString() + "\n";
			numberText.text = LocationTracker.SavedLocationsCount.ToString();
		}, errorMessage => { Debug.Log(errorMessage); });
	}

	static LocationRequest CreateLocationRequest()
	{
		var interval = 10 * 1000L;
		var fastestInterval = 60 * 1000L;
		var maxWaitTime = 5 * 1000L;
		var priority = LocationRequest.TrackingPriority.BalancedPowerAccuracy;
		var request = new LocationRequest(interval, fastestInterval, priority, maxWaitTime);
		return request;
	}
}