
using System.Text.RegularExpressions;

#if UNITY_ANDROID
namespace LocationTracking.Internal
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	class SceneHelper : MonoBehaviour
	{
		static SceneHelper _instance;
		static readonly object InitLock = new object();
		readonly object _queueLock = new object();
		readonly List<Action> _queuedActions = new List<Action>();
		readonly List<Action> _executingActions = new List<Action>();

		public static SceneHelper Instance
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
					var instances = FindObjectsOfType<SceneHelper>();

					if (instances.Length > 1)
					{
						Debug.LogError(typeof(SceneHelper) + " Something went really wrong " +
						               " - there should never be more than 1 " + typeof(SceneHelper) +
						               " Reopening the scene might fix it.");
					}
					else if (instances.Length == 0)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<SceneHelper>();
						singleton.name = "GoodiesSceneHelper";

						DontDestroyOnLoad(singleton);

						Debug.Log("[Singleton] An _instance of " + typeof(SceneHelper) +
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

		SceneHelper()
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

		public void OnLocationReceived(string latLong)
		{
			var location = Regex.Split(latLong, ",");
			
		}
	}
}
#endif