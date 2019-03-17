using LocationTracking.Scripts;

#if UNITY_ANDROID
namespace LocationTracking.Internal
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	class LocationTrackingSceneHelper : MonoBehaviour
	{
		static LocationTrackingSceneHelper _instance;
		static readonly object InitLock = new object();
		readonly object _queueLock = new object();
		readonly List<Action> _queuedActions = new List<Action>();
		readonly List<Action> _executingActions = new List<Action>();

		public static LocationTrackingSceneHelper Instance
		{
			get
			{
				if (_instance == null)
				{
					Init();
				}

				return _instance;
			}
		}

		public static bool IsInImmersiveMode { set; private get; }

		public Texture2D LastTekenScreenshot { get; private set; }

		internal static void Init()
		{
			lock (InitLock)
			{
				if (ReferenceEquals(_instance, null))
				{
					var instances = FindObjectsOfType<LocationTrackingSceneHelper>();

					if (instances.Length > 1)
					{
						Debug.LogError(typeof(LocationTrackingSceneHelper) + " Something went really wrong " +
						               " - there should never be more than 1 " + typeof(LocationTrackingSceneHelper) +
						               " Reopening the scene might fix it.");
					}
					else if (instances.Length == 0)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<LocationTrackingSceneHelper>();
						singleton.name = "GoodiesSceneHelper";

						DontDestroyOnLoad(singleton);

						Debug.Log("[Singleton] An _instance of " + typeof(LocationTrackingSceneHelper) +
						          " is needed in the scene, so '" + singleton.name +
						          "' was created with DontDestroyOnLoad.");
					}
					else
					{
						Debug.Log("[Singleton] Using _instance already created: " + _instance.gameObject.name);
					}
				}
			}
		}

		LocationTrackingSceneHelper()
		{
		}

		internal static void Queue(Action action)
		{
			if (action == null)
			{
				Debug.LogWarning("Trying to queue null action");
				return;
			}

			lock (_instance._queueLock)
			{
				_instance._queuedActions.Add(action);
			}
		}

		void Update()
		{
			MoveQueuedActionsToExecuting();

			while (_executingActions.Count > 0)
			{
				Action action = _executingActions[0];
				_executingActions.RemoveAt(0);
				action();
			}
		}

		void MoveQueuedActionsToExecuting()
		{
			lock (_queueLock)
			{
				while (_queuedActions.Count > 0)
				{
					Action action = _queuedActions[0];
					_executingActions.Add(action);
					_queuedActions.RemoveAt(0);
				}
			}
		}

		public void OnLocationReceived(string locationJson)
		{
			LocationTracker.OnLocationReceived(new Location(locationJson));
		}

		public void OnCheckLocationSettingsCancelled(string message)
		{
			LocationTracker.OnError(LocationTracker.ErrorCode.UserCancelled);
		}

		public void OnCheckLocationSettingsFailed(string message)
		{
			LocationTracker.OnError(LocationTracker.ErrorCode.LocationDisabled);
		}

		public void OnPermissionDenied(string message)
		{
			LocationTracker.OnError(LocationTracker.ErrorCode.LocationPermissionNotGranted);
		}
	}
}
#endif