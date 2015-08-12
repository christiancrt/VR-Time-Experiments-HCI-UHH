using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	double delta = 0.0f;
	long lastTimeStamp;
	public static double _lastTimeOfDay = 6.5;
	public static int lastSeat = -1;
	public static float globalSpeed = 0.0f;
	public static double startOfDay = 12.0;
	public static int proband = 1;
	public static int waitDuration = 600;
	public static int verbalDuration = 600;
	public static int spacialDuration = 600;
	public static int stage = 0;
	public static int stageNR = 0;
	private static bool waitPhaseDone = false;
	private static bool verbalPhaseDone = false;
	private static bool spacialPhaseDone = false;

	long getTimeStamp() {
		return System.DateTime.Now.Second*1000 + System.DateTime.Now.Minute*60000 + System.DateTime.Now.Hour * 3600000 + System.DateTime.Now.Millisecond;
	}

	long getMaxTimeStamp() {
		return 23 * 3600000 + 59*60000 + 59*1000 + 999;
	}

	void LateUpdate() {
		long newTimeStamp = getTimeStamp();
		while(lastTimeStamp > newTimeStamp)
			newTimeStamp += getMaxTimeStamp();
		delta = (newTimeStamp - lastTimeStamp)/1000.0;
		//Debug.Log(delta);
		lastTimeStamp = newTimeStamp;
	}

	public double DeltaTime {
		get {
			return delta;
		}
		set {
			delta = value;
		}
	}

	// Use this for initialization
	void Start () {
		instance = this;
		lastTimeStamp = getTimeStamp();
	}
}
