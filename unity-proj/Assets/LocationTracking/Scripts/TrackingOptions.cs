using JetBrains.Annotations;

namespace LocationTracking.Scripts
{
	[PublicAPI]
	public class TrackingOptions
	{
		LocationRequest _request;
		Notification _notification;

		public LocationRequest Request
		{
			get { return _request; }
		}

		public Notification Notification
		{
			get { return _notification; }
		}

		public TrackingOptions([NotNull] LocationRequest request)
		{
			_request = request;
		}

		public TrackingOptions SetNotification([NotNull] Notification notification)
		{
			_notification = notification;
			return this;
		}
	}
}