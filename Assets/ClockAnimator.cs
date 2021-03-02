using UnityEngine;
using System.Collections;
using System;

// GameDemo
public class ClockAnimator : MonoBehaviour {
	
	public Transform hours,minutes,seconds;
	public bool analog;
	
	private const float hoursToDegrees = 360f / 12f;
	private const float minutesToDegrees = 360f / 60f;
	private const float secondsToDegrees = 360f / 60f;
	
	// Use this for initialization
	void Start () {
		
		// game id is 9999 for test
		TssSdk.TssSdkInit(9999u);
		
		// user info
		TssSdk.TssSdkSetUserInfo(TssSdk.EENTRYID.ENTRY_ID_MM, 472250383u, 12345678u);
		TssSdk.TssSdkSetUserInfo(TssSdk.EENTRYID.ENTRY_ID_MM, "uin472250383", 12345678u);
		TssSdk.TssSdkSetUserInfo(TssSdk.EENTRYID.ENTRY_ID_MM, 472250383u, "appid12345678");
		TssSdk.TssSdkSetUserInfo(TssSdk.EENTRYID.ENTRY_ID_MM, "uin472250383", "appid12345678");

		
	}
	
	// Update is called once per frame
	void Update () {
		
		//hello ("Hello Unity Android Plugin");
		if (analog)
		{
			TimeSpan timespan = DateTime.Now.TimeOfDay;
			hours.localRotation = Quaternion.Euler(0f, 0f, (float)timespan.TotalHours * -hoursToDegrees);
			minutes.localRotation = Quaternion.Euler(0f, 0f, (float)timespan.TotalMinutes * -minutesToDegrees);
			seconds.localRotation = Quaternion.Euler(0f, 0f, (float)timespan.TotalSeconds * -secondsToDegrees);
		}
		else {
			DateTime time = DateTime.Now;
			hours.localRotation = Quaternion.Euler(0.0f,0.0f,time.Hour * -hoursToDegrees);
			minutes.localRotation = Quaternion.Euler(0f, 0f, time.Minute * -minutesToDegrees);
			seconds.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);
		}	
	}
	
	private void onApplicationPause(bool pause)
	{
		// calls tss_sdk_setgamestatus
		TssSdk.TssSdkSetGameStatus((!pause)?TssSdk.EGAMESTATUS.GAME_STATUS_BACKEND:TssSdk.EGAMESTATUS.GAME_STATUS_BACKEND);
		//TssSdk.TssSdkSetGameStatus(777u);
	}
}
