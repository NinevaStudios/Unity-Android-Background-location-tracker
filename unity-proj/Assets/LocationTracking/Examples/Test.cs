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
		var priority = LocationRequest.Priority.BalancedPowerAccuracy;
		var request = new LocationRequest(interval, fastestInterval, priority, maxWaitTime);

		var options = new TrackingOptions(request);

		LocationTracker.StartLocationTracking(options, location =>
		{
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
}