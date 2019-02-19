using JetBrains.Annotations;

namespace LocationTracking.Scripts
{
	[PublicAPI]
	public struct TrackingOptions
	{
		public LocationRequest request;
		public bool showNotification;
		public Notification notification;
	}
}