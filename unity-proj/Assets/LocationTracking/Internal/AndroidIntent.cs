
using Nineva.LocationTracker;

#if UNITY_ANDROID
namespace LocationTracking.Internal
{
	using System;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Android intent.
	/// </summary>
	[PublicAPI]
	public class AndroidIntent : IDisposable
	{
		public const string MIMETypeTextPlain = "text/plain";
		public const string MIMETypeMessage = "message/rfc822";
		public const string MIMETypeImageJpeg = "image/jpeg";
		public const string MIMETypeImagePng = "image/png";
		public const string MIMETypeImageAll = "image/*";
		public const string MIMETypePdf = "application/pdf";

		#region actions

		/// <summary>
		/// Broadcast Action: The user has switched the phone into or out of Airplane Mode.
		/// One or more radios have been turned off or on. The intent will have the following extra value:
		/// state - A boolean value indicating whether Airplane Mode is on.
		/// If true, then cell radio and possibly other radios such as blue tooth or WiFi may have also been turned off
		/// </summary>
		public const string ActionAirplaneModeChanged = "android.intent.action.AIRPLANE_MODE";
		/// <summary>
		/// Activity Action: List all available applications.
		/// </summary>
		public const string ActionAllApps = "android.intent.action.ALL_APPS";
		/// <summary>
		/// An activity that provides a user interface for adjusting application preferences.
		/// Optional but recommended settings for all applications which have settings.
		/// </summary>
		public const string ActionApplicationPreferences = "android.intent.action.APPLICATION_PREFERENCES";
		/// <summary>
		/// Activity Action: Creates a shortcut.
		/// For compatibility with older versions of android the intent may also contain three extras:
		/// SHORTCUT_INTENT (value: Intent), SHORTCUT_NAME (value: String), and SHORTCUT_ICON (value: Bitmap)
		/// or SHORTCUT_ICON_RESOURCE (value: ShortcutIconResource).
		/// </summary>
		public const string ActionCreateShortCut = "android.intent.action.CREATE_SHORTCUT";
		
		public const string ActionMain = "android.intent.action.MAIN";
		public const string ActionSend = "android.intent.action.SEND";
		public const string ActionEdit = "android.intent.action.EDIT";
		public const string ActionSendTo = "android.intent.action.SENDTO";
		public const string ActionView = "android.intent.action.VIEW";
		public const string ActionInsert = "android.intent.action.INSERT";
		public const string ActionDelete = "android.intent.action.DELETE";
		public const string ActionBatteryChanged = "android.intent.action.BATTERY_CHANGED";
		public const string ActionBatteryLow = "android.intent.action.BATTERY_LOW";

		public const string ActionMediaMounted = "android.intent.action.MEDIA_MOUNTED";

		public const string ActionDial = "android.intent.action.DIAL";
		public const string ActionCall = "android.intent.action.CALL";
		/// <summary>
		/// Activity Action: Handle an incoming phone call.
		/// </summary>
		public const string ActionAnswer = "android.intent.action.ANSWER";

		public const string ExtraTitle = "android.intent.extra.TITLE";
		public const string ExtraSubject = "android.intent.extra.SUBJECT";
		public const string ExtraText = "android.intent.extra.TEXT";
		public const string ExtraEmail = "android.intent.extra.EMAIL";
		public const string ExtraCc = "android.intent.extra.CC";
		public const string ExtraBcc = "android.intent.extra.BCC";
		public const string ExtraStream = "android.intent.extra.STREAM";
		/// <summary>
		/// Activity Action: Pick a Wi-Fi network to connect to.
		/// </summary>
		public const string ActionPickWifiNetwork = "android.net.wifi.PICK_WIFI_NETWORK";

		#endregion

		#region category

		public const string CATEGORY_LAUNCHER = "android.intent.category.LAUNCHER";

		#endregion

		[Flags]
		[PublicAPI]
		public enum Flags
		{
			ActivityBroughtToFront = 4194304,
			ActivityClearTask = 32768,
			ActivityNewTask = 268435456,
			ActivitySingleTop = 536870912,
			ActivityResetTaskIfNeeded = 2097152,
			ActivityClearTop = 67108864,
			GrantReadUriPermission = 1,
			GrantWriteUriPermission = 2
		}

		internal const string PutExtraMethodName = "putExtra";
		internal const string SetActionMethodName = "setAction";
		internal const string SetTypeMethodName = "setType";
		internal const string SetDataMethodName = "setData";
		internal const string SetDataAndTypeMethodName = "setDataAndType";
		internal const string SetClassNameMethodName = "setClassName";
		internal const string SetPackageMethodName = "setPackage";
		internal const string SetFlagsMethodName = "setFlags";
		internal const string AddCategoryMethodName = "addCategory";

		AndroidJavaObject _intent;

		public AndroidJavaObject AJO
		{
			get { return _intent; }
		}

		public AndroidIntent()
		{
			PlatformCheck();

			_intent = new AndroidJavaObject(C.AndroidContentIntent);
		}

		public AndroidIntent(string action)
		{
			PlatformCheck();

			_intent = new AndroidJavaObject(C.AndroidContentIntent, action);
		}

		public AndroidIntent(AndroidJavaObject clazz)
		{
			PlatformCheck();

			_intent = new AndroidJavaObject(C.AndroidContentIntent, Utils.Activity, clazz);
		}

		public AndroidIntent(string action, AndroidJavaObject uri) : this(action)
		{
			PlatformCheck();

			_intent = new AndroidJavaObject(C.AndroidContentIntent, action, uri);
		}

		public static AndroidIntent Wrap([NotNull] AndroidJavaObject ajo)
		{
			return new AndroidIntent {_intent = ajo};
		}

		#region put_extra

		public AndroidIntent PutExtra(string name, AndroidJavaObject value)
		{
			_intent.CallAJO(PutExtraMethodName, name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, string value)
		{
			_intent.CallAJO(PutExtraMethodName, name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, int value)
		{
			_intent.CallAJO(PutExtraMethodName, name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, long value)
		{
			_intent.CallAJO(PutExtraMethodName, name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, bool value)
		{
			_intent.CallAJO(PutExtraMethodName, name, value);
			return this;
		}

		public AndroidIntent PutExtra(string name, string[] values)
		{
			_intent.CallAJO(PutExtraMethodName, name, values);
			return this;
		}

		public bool GetBoolExtra(string name, bool defaultValue)
		{
			return _intent.CallBool("getBooleanExtra", name, defaultValue);
		}

		public string GetStringExtra(string name)
		{
			return _intent.CallStr("getStringExtra", name);
		}

		#endregion

		public AndroidIntent SetAction(string action)
		{
			_intent.CallAJO(SetActionMethodName, action);
			return this;
		}

		public AndroidIntent SetType(string type)
		{
			_intent.CallAJO(SetTypeMethodName, type);
			return this;
		}

		public AndroidIntent SetData(AndroidJavaObject uri)
		{
			_intent.CallAJO(SetDataMethodName, uri);
			return this;
		}

		public AndroidIntent SetDataAndType(AndroidJavaObject uri, string type)
		{
			_intent.CallAJO(SetDataAndTypeMethodName, uri, type);
			return this;
		}

		public AndroidIntent AddCategory(string category)
		{
			_intent.CallAJO(AddCategoryMethodName, category);
			return this;
		}

		public AndroidIntent SetClassName(string packageName, string className)
		{
			_intent.CallAJO(SetClassNameMethodName, packageName, className);
			return this;
		}

		public AndroidIntent SetPackage(string packageName)
		{
			_intent.CallAJO(SetPackageMethodName, packageName);
			return this;
		}

		public AndroidIntent SetFlags(Flags flags)
		{
			_intent.CallAJO(SetFlagsMethodName, (int) flags);
			return this;
		}

		public bool ResolveActivity()
		{
			using (var pm = Utils.PackageManager)
			{
				try
				{
					_intent.CallAJO("resolveActivity", pm).GetClassSimpleName();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		public void Dispose()
		{
			AJO.Dispose();
		}
		
		static void PlatformCheck()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				throw new InvalidOperationException("AndroidJavaObject can be created only on Android");
			}
		}
	}
}
#endif