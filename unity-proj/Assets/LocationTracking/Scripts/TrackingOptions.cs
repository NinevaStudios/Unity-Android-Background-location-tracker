namespace LocationTracking.Scripts
{
	public class TrackingOptions
	{
		public TrackingOptions(LocationRequest request)
		{
			Request = request;
		}

		/// <summary>
		/// Location tracking request. <see cref="LocationRequest"/>
		/// </summary>
		public LocationRequest Request { get; private set; }
	}
}