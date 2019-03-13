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
		
		public string title;
		public string content;
		public Visibility visibility;
		public Importance importance;
		public bool hasStopServiceAction;
		public string stopServiceActionName;

		public static Notification DefaultNotification()
		{
			return new Notification
			{
				title = "Location Tracker",
				content = "Your location is being tracked now.",
				visibility = Visibility.Private,
				importance = Importance.Default,
				hasStopServiceAction = true,
				stopServiceActionName = "Stop Tracking"
			};
		}
	}
}