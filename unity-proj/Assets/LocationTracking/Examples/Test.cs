using JetBrains.Annotations;
using LocationTracking.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
	[SerializeField] Text locationText, numberText;

	int _ticks;

	[UsedImplicitly]
	public void OnStartTracking()
	{
		var interval = 10 * 1000L;
		var fastestInterval = 60 * 1000L;
		var maxWaitTime = 5 * 1000L;
		var priority = LocationRequest.TrackingPriority.BalancedPowerAccuracy;
		var request = new LocationRequest(interval, fastestInterval, priority, maxWaitTime);

		var options = new TrackingOptions(request);
		options.SetNotification(Notification.DefaultNotification());

		LocationTracker.StartLocationTracking(options, location =>
		{
			locationText.horizontalOverflow = HorizontalWrapMode.Wrap;
			locationText.text = ++_ticks + ". " + location.ToString() + "\n";
			numberText.text = LocationTracker.SavedLocationsCount.ToString();
		});
	}

	[UsedImplicitly]
	public void OnStopTracking()
	{
		LocationTracker.StopLocationTracking(message => { locationText.text = message; });
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
}