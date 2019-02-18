
using DeadMosquito.AndroidGoodies.Internal;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick()
	{
		var intent = new AndroidIntent(AGUtils.ClassForName("com.ninevastudios.locationtracker.LocationHelperActivity"));
		intent.SetFlags(AndroidIntent.Flags.ActivityNewTask | AndroidIntent.Flags.ActivityClearTask);
		
		AGUtils.StartActivity(intent.AJO);
	}
}
