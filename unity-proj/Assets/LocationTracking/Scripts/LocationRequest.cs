using JetBrains.Annotations;

namespace LocationTracking.Scripts
{
	/// <summary>
	/// Entity for creating a location tracking request
	/// </summary>
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

		/// <summary>
		/// Interval at which location is computed for your app (in milliseconds).
		/// </summary>
		public long Interval { get; private set; }

		/// <summary>
		/// The interval at which location computed for other apps is delivered to your app (in milliseconds).
		/// </summary>
		public long FastestInterval { get; private set; }

		/// <summary>
		/// The precision of the location data. In general, the higher the accuracy, the higher the battery drain.
		/// </summary>
		public TrackingPriority Priority { get; private set; }

		/// <summary>
		/// Latency of location delivery (in milliseconds). Typically a value that is several times larger than the <see cref="Interval"/>.
		/// This setting delays location delivery, and multiple location updates may be delivered in batches.
		/// </summary>
		public long MaxWaitTime { get; private set; }

		/// <summary>
		/// Create a location tracking request with given parameters
		/// </summary>
		/// <param name="interval"><see cref="Interval"/></param>
		/// <param name="fastestInterval"><see cref="FastestInterval"/></param>
		/// <param name="trackingPriority"><see cref="Priority"/></param>
		/// <param name="maxWaitTime"><see cref="MaxWaitTime"/></param>
		public LocationRequest(long interval, long fastestInterval, TrackingPriority trackingPriority, long maxWaitTime)
		{
			Interval = interval;
			FastestInterval = fastestInterval;
			Priority = trackingPriority;
			MaxWaitTime = maxWaitTime;
		}

		/// <summary>
		/// Create a location tracking request with default parameters
		/// </summary>
		public LocationRequest()
		{
			Interval = 30 * 1000L;
			FastestInterval = 30 * 1000L;
			Priority = TrackingPriority.BalancedPowerAccuracy;
			MaxWaitTime = 300 * 1000L;
		}
	}
}