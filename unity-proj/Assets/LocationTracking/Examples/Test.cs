using LocationTracking.Scripts;
using UnityEngine;

public class Test : MonoBehaviour {

	public void OnClick()
	{
		var request = new LocationRequest {
			interval = 60 * 1000L, 
			maxWaitTime = 600 * 1000L, 
			priority = LocationRequest.Priority.BalancedPowerAccuracy, 
			fastestInterval = 30 * 1000L};

		LocationTracker.RegisterLocationTrackingService(request);
	}
}
