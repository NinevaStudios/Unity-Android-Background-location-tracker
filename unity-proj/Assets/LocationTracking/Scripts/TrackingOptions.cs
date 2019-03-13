using JetBrains.Annotations;

namespace LocationTracking.Scripts
{
	[PublicAPI]
	public class TrackingOptions
	{
		public LocationRequest Request { get; private set; }

		public Notification Notification { get; private set; }

		public bool TrackInBackground { get; private set; }

		public TrackingOptions([NotNull] LocationRequest request)
		{
			Request = request;
			TrackInBackground = true;
			Notification = Notification.DefaultNotification();
		}

		public TrackingOptions SetNotification([NotNull] Notification notification)
		{
			Notification = notification;
			return this;
		}
	}
}