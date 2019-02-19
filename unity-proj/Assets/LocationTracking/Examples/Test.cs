using LocationTracking.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
	[SerializeField]
	Text text;

	int _ticks;

	public void OnClick()
	{
		var request = new LocationRequest
		{
			interval = 10 * 1000L,
			maxWaitTime = 60 * 1000L,
			priority = LocationRequest.Priority.BalancedPowerAccuracy,
			fastestInterval = 5 * 1000L
		};

		var options = new TrackingOptions {request = request};

		LocationTracker.StartLocationTracking(options, location => { text.text += ++_ticks + ". " + location.ToString() + "\n"; });
	}
}