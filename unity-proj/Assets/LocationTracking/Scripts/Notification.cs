using JetBrains.Annotations;

namespace LocationTracking.Scripts
{
	[PublicAPI]
	public class Notification
	{
		/// <summary>
		/// Notification visibility
		/// </summary>
		[PublicAPI]
		public enum Visibility
		{
			/// <summary>
			/// Show this notification on all lockscreens, but conceal sensitive or private information on secure lockscreens.
			/// </summary>
			Private = 0,

			/// <summary>
			/// Show this notification in its entirety on all lockscreens.
			/// </summary>
			Public = 1,

			/// <summary>
			/// Do not reveal any part of this notification on a secure lockscreen.
			/// </summary>
			Secret = -1
		}

		/// <summary>
		/// Notification importance
		/// </summary>
		[PublicAPI]
		public enum Importance
		{
			/// <summary>
			/// Shows everywhere, but is not intrusive.
			/// </summary>
			Low = 2,

			/// <summary>
			/// Shows everywhere, makes noise, but does not visually intrude.
			/// </summary>
			Default = 3,

			/// <summary>
			/// Shows everywhere, makes noise and peeks. May use full screen intents.
			/// </summary>
			High = 4
		}

		/// <summary>
		/// Notification title
		/// </summary>
		public string title;

		/// <summary>
		/// Notification content text
		/// </summary>
		public string content;

		/// <summary>
		/// Notification lock screen visibility
		/// </summary>
		public Visibility visibility;

		/// <summary>
		/// Notification importance
		/// </summary>
		public Importance importance;

		/// <summary>
		/// Whether notification has a button to stop the service
		/// </summary>
		public bool hasStopServiceAction;

		/// <summary>
		/// Stop service button text
		/// </summary>
		public string stopServiceActionName;

		/// <summary>
		/// Create a notification with default settings
		/// </summary>
		/// <returns></returns>
		public Notification()
		{
			title = "Location Tracker";
			content = "Your location is being tracked now.";
			visibility = Visibility.Private;
			importance = Importance.Default;
			hasStopServiceAction = true;
			stopServiceActionName = "Stop Tracking";
		}
	}
}