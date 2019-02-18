#if UNITY_ANDROID
namespace LocationTracking.Internal
{
	using System;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	public static class Utils
	{
		static AndroidJavaObject _activity;

		public static AndroidJavaObject Activity
		{
			get
			{
				if (_activity == null)
				{
					var unityPlayer = new AndroidJavaClass(C.ComUnity3DPlayerUnityPlayer);
					_activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
				}
				return _activity;
			}
		}
		
		public static AndroidIntent NotificationsActivityIntent
		{
			get
			{
				var activity = new AndroidJavaClass(C.GoodiesNotificationsIntermediateActivity);
				var notificationIntent = activity.GetStatic<AndroidJavaObject>("lastIntent");
				return notificationIntent.IsJavaNull() ? null : AndroidIntent.Wrap(notificationIntent);
			}
		}

		public static AndroidIntent ActivityIntent
		{
			get { return AndroidIntent.Wrap(Activity.CallAJO("getIntent")); }
		}

		public static AndroidJavaObject ExternalCacheDirectory
		{
			get { return Activity.CallAJOSafe("getExternalCacheDir"); }
		}

		public static AndroidJavaObject CacheDirectory
		{
			get { return Activity.CallAJOSafe("getCacheDir"); }
		}

		public static AndroidJavaObject CodeCacheDirectory
		{
			get { return Activity.CallAJOSafe("getCodeCacheDir"); }
		}
		
		public static AndroidJavaObject DataDir
		{
			get { return Activity.CallAJOSafe("getDataDir"); }
		}
		
		public static AndroidJavaObject ObbDir
		{
			get { return Activity.CallAJOSafe("getObbDir"); }
		}

		public static AndroidJavaObject ActivityDecorView
		{
			get { return Window.Call<AndroidJavaObject>("getDecorView"); }
		}

		public static AndroidJavaObject Window
		{
			get { return Activity.Call<AndroidJavaObject>("getWindow"); }
		}

		public static AndroidJavaObject PackageManager
		{
			get { return Activity.CallAJO("getPackageManager"); }
		}

		public static AndroidJavaObject ContentResolver
		{
			get { return Activity.CallAJO("getContentResolver"); }
		}

		public static bool HasSystemFeature(string feature)
		{
			using (var pm = PackageManager)
			{
				return pm.CallBool("hasSystemFeature", feature);
			}
		}

		public static long CurrentTimeMillis
		{
			get
			{
				using (var system = new AndroidJavaClass(C.JavaLangSystem))
				{
					return system.CallStaticLong("currentTimeMillis");
				}
			}
		}

		#region reflection

		public static AndroidJavaObject ClassForName(string className)
		{
			using (var clazz = new AndroidJavaClass(C.JavaLangClass))
			{
				return clazz.CallStaticAJO("forName", className);
			}
		}

		public static AndroidJavaObject Cast(this AndroidJavaObject source, string destClass)
		{
			using (var destClassAJC = ClassForName(destClass))
			{
				return destClassAJC.Call<AndroidJavaObject>("cast", source);
			}
		}

		public static bool IsJavaNull(this AndroidJavaObject ajo)
		{
			return ajo == null || ajo.GetRawObject().ToInt32() == 0;
		}

		#endregion

		public static bool IsNotAndroidCheck()
		{
			bool isAndroid = Application.platform == RuntimePlatform.Android;

			if (isAndroid)
			{
				SceneHelper.Init();
			}

			return !isAndroid;
		}

		public static void RunOnUiThread(Action action)
		{
			Activity.Call("runOnUiThread", new AndroidJavaRunnable(action));
		}

		public static void StartActivity(AndroidJavaObject intent, Action fallback = null)
		{
			try
			{
				Activity.Call("startActivity", intent);
			}
			catch (AndroidJavaException exception)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("Could not start the activity with " + intent.JavaToString() + ": " + exception.Message);
				}
				if (fallback != null)
				{
					fallback();
				}
			}
			finally
			{
				intent.Dispose();
			}
		}

		public static void StartActivityWithChooser(AndroidJavaObject intent, string chooserTitle)
		{
			try
			{
				var jChooser = intent.CallStaticAJO("createChooser", intent, chooserTitle);
				Activity.Call("startActivity", jChooser);
			}
			catch (AndroidJavaException exception)
			{
				Debug.LogWarning("Could not start the activity with " + intent.JavaToString() + ": " + exception.Message);
			}
			finally
			{
				intent.Dispose();
			}
		}

		public static void SendBroadcast(AndroidJavaObject intent)
		{
			Activity.Call("sendBroadcast", intent);
		}

		public static AndroidJavaObject GetLaunchIntent(string packageName)
		{
			using (var pm = PackageManager)
			{
				return pm.CallAJO("getLaunchIntentForPackage", packageName);
			}
		}
		
		public static AndroidJavaObject NotificationIntermediateActivityIntent
		{
			get
			{
				return new AndroidIntent(ClassForName(C.GoodiesNotificationsIntermediateActivity)).AJO;
			}
		}

		public static AndroidJavaObject NewJavaFile(string path)
		{
			return new AndroidJavaObject(C.JavaIoFile, path);
		}

		#region images

		static Texture2D Texture2DFromBitmap(AndroidJavaObject bitmapAjo)
		{
			var compressFormatPng = new AndroidJavaClass(C.AndroidGraphicsBitmapCompressFormat).GetStatic<AndroidJavaObject>("PNG");
			var outputStream = new AndroidJavaObject(C.JavaIoBytearrayOutputStream);
			bitmapAjo.CallBool("compress", compressFormatPng, 100, outputStream);
			var buffer = outputStream.Call<byte[]>("toByteArray");

			var tex = new Texture2D(2, 2);
			tex.LoadImage(buffer);
			return tex;
		}

		public static AndroidJavaObject DecodeBitmap(string path)
		{
			return C.AndroidGraphicsBitmapFactory.AJCCallStaticOnceAJO("decodeFile", path);
		}

		static AndroidJavaObject RotateBitmap(AndroidJavaObject bitmap, float angle)
		{
			using (var matrix = new AndroidJavaObject(C.AndroidGraphicsMatrix))
			{
				matrix.CallBool("postRotate", angle);
				return C.AndroidGraphicsBitmap.AJCCallStaticOnceAJO("createBitmap", bitmap, 0, 0, bitmap.CallInt("getWidth"), bitmap.CallInt("getHeight"), matrix, true);
			}
		}

		#endregion

		public static string GetPermissionErrorMessage(string permission)
		{
			return string.Format("{0} runtime permission missing in AndroidManifest.xml or user did not grant the permission.", permission);
		}

		public static int RandomId()
		{
			return UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		}
	}
}
#endif