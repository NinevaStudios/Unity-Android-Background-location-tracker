using JetBrains.Annotations;

namespace LocationTracking.Scripts
{
	/// <summary>
	/// Required parameters for location tracking
	/// </summary>
	[PublicAPI]
	public class TrackingOptions
	{
		/// <summary>
		/// Location tracking request. <see cref="LocationRequest"/>
		/// </summary>
		public LocationRequest Request { get; private set; }

		/// <summary>
		/// A notification to be shown to the user while service is working
		/// </summary>
		public Notification Notification { get; private set; }

		// TODO docs
		public bool TrackInBackground { get; private set; }

		public TrackingOptions([NotNull] LocationRequest request)
		{
			Request = request;
			TrackInBackground = true;
			Notification = new Notification();
		}

		public TrackingOptions SetNotification([NotNull] Notification notification)
		{
			Notification = notification;
			return this;
		}
	}
}