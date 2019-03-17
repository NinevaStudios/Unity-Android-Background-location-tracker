using JetBrains.Annotations;

namespace LocationTracking.Scripts
{
	/// <summary>
	/// Required parameters for location tracking
	/// </summary>
	[PublicAPI]
	public class BackgroundTrackingOptions : TrackingOptions
	{


		/// <summary>
		/// A notification to be shown to the user while service is working
		/// </summary>
		public Notification Notification { get; private set; }
		

		public BackgroundTrackingOptions([NotNull] LocationRequest request, [CanBeNull] Notification notification) : base(request)
		{
			if (notification == null)
			{
				// TODO construct default notification
				Notification = new Notification();
			}

			Notification = notification;
		}
	}
}