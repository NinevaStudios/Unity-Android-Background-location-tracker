using JetBrains.Annotations;
using LocationTracking.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
	[SerializeField] Text locationText, numberText;

	int _ticks;

	// TODO separate examples for background and foreground tracking
	[UsedImplicitly]
	public void OnStartTracking()
	{
		var interval = 10 * 1000L;
		var fastestInterval = 60 * 1000L;
		var maxWaitTime = 5 * 1000L;
		var priority = LocationRequest.TrackingPriority.BalancedPowerAccuracy;
		var request = new LocationRequest(interval, fastestInterval, priority, maxWaitTime);

		var options = new BackgroundTrackingOptions(request, new Notification());

		LocationTracker.StartLocationTrackingBackground(options, location =>
		{
			locationText.horizontalOverflow = HorizontalWrapMode.Wrap;
			locationText.text = ++_ticks + ". " + location.ToString() + "\n";
			numberText.text = LocationTracker.SavedLocationsCount.ToString();
		}, errorMessage => { Debug.Log(errorMessage); });
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