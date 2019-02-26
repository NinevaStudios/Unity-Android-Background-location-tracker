using JetBrains.Annotations;

namespace LocationTracking.Scripts
{
	[PublicAPI]
	public class LocationRequest
	{
		[PublicAPI]
		public enum TrackingPriority
		{
			/// <summary>
			/// Provides the most accurate location possible, which is computed using as many inputs as necessary (it enables GPS, Wi-Fi, and cell, and uses a variety of Sensors),
			/// and may cause significant battery drain.
			/// </summary>
			HighAccuracy = 100,

			/// <summary>
			/// Provides accurate location while optimizing for power. Very rarely uses GPS.
			/// Typically uses a combination of Wi-Fi and cell information to compute device location.
			/// </summary>
			BalancedPowerAccuracy = 102,

			/// <summary>
			/// Largely relies on cell towers and avoids GPS and Wi-Fi inputs, providing coarse (city-level) accuracy with minimal battery drain.
			/// </summary>
			LowPower = 104,

			/// <summary>
			/// Receives locations passively from other apps for which location has already been computed.
			/// </summary>
			NoPower = 105
		}

		// TODO remove docs from private fields

		/// <summary>
		/// Interval at which location is computed for your app.
		/// </summary>
		public long Interval { get; private set; }

		/// <summary>
		/// The interval at which location computed for other apps is delivered to your app.
		/// </summary>
		public long FastestInterval { get; private set; }

		/// <summary>
		/// The precision of the location data. In general, the higher the accuracy, the higher the battery drain.
		/// </summary>
		public TrackingPriority Priority { get; private set; }

		/// <summary>
		/// Latency of location delivery. Typically a value that is several times larger than the <see cref="Interval"/>.
		/// This setting delays location delivery, and multiple location updates may be delivered in batches.
		/// </summary>
		public long MaxWaitTime { get; private set; }

		// TODO document
		public LocationRequest(long interval, long fastestInterval, TrackingPriority trackingPriority, long maxWaitTime)
		{
			Interval = interval;
			FastestInterval = fastestInterval;
			Priority = trackingPriority;
			MaxWaitTime = maxWaitTime;
		}

		public LocationRequest()
		{
			// TODO come up with reasonable defaults
			Interval = 0;
			FastestInterval = 0;
			Priority = TrackingPriority.BalancedPowerAccuracy;
			MaxWaitTime = 0;
		}
		
		
	}
}