using UnityEngine;
using System.Collections;

public class LightScript : MonoBehaviour {

	public static float MOON_STEP = 1.1f;
	public static float BOX_STEP = -1.0f;
	public static LightScript instance;
	public Camera leftEye;	
	public Camera rightEye;	

	private double _step = 0.0;
	private double _timeOfDay = 12.0;

	/*private static Color DAWN_AMBIENT_COLOR = new Color(66/255.0f,66/255.0f,66/255.0f);
	private static Color NIGHT_AMBIENT_COLOR = new Color(35/255.0f,35/255.0f,35/255.0f);
	private static Color DAY_AMBIENT_COLOR = new Color(144/255.0f,144/255.0f,117/255.0f);*/

	/*private static Color DAWN_FOG_COLOR = new Color(23/255.0f,32/255.0f,39/255.0f,1);
	private static Color NIGHT_FOG_COLOR = new Color(5/255.0f,5/255.0f,5/255.0f,1);
	private static Color DAY_FOG_COLOR = new Color(169/255.0f,201/255.0f,227/255.0f,1);*/

	private static Color DAWN_FOG_COLOR = new Color(1,1,1,1);
	private static Color NIGHT_FOG_COLOR = new Color(1,1,1,1);
	private static Color DAY_FOG_COLOR = new Color(1,1,1,1);

	private static Color DAY_COLOR = new Color(255/255.0f,247/255.0f,219/255.0f,1);
	private static Color NIGHT_COLOR = new Color(0,0,0,0);
	private static Color DAWN_COLOR = new Color(255/255.0f,100/255.0f,0,1);

	/*private static float FOG_INTENSITY_DAY = 9500;
	private static float FOG_INTENSITY_DAWN = 8500;
	private static float FOG_INTENSITY_NIGHT = 7000;*/

	private static float FOG_INTENSITY_DAY = 17500;
	private static float FOG_INTENSITY_DAWN = 16500;
	private static float FOG_INTENSITY_NIGHT = 15000;

	private static float AMBIENT_INTENSITY_DAY = 0.75f;
	private static float AMBIENT_INTENSITY_DAWN = 0.5f;
	private static float AMBIENT_INTENSITY_NIGHT = 0.05f;

	private static float WATER_INTENSITY_DAY = 1.00f;
	private static float WATER_INTENSITY_DAWN = 0.3f;
	private static float WATER_INTENSITY_NIGHT = 0.01f;

	private static float DAY_SHIFT_1 = 4.5f;
	private static float DAY_SHIFT_2 = 6.0f;
	private static float DAY_SHIFT_3 = 8.5f;

	private static float NIGHT_SHIFT_1 = 15.0f;
	private static float NIGHT_SHIFT_2 = 18.0f;
	private static float NIGHT_SHIFT_3 = 19.5f;

	private static float NIGHTBOX_DAY = 0.0f;
	private static float NIGHTBOX_DAWN = 0.5f;
	private static float NIGHTBOX_NIGHT = 1.0f;
	
	private static double _timeStep = 0.0;
	private static double _timeMultiplicator = 0.0;


	public int getVRMinutes() {
		double minutes = _timeOfDay - (int)_timeOfDay;
		minutes *= 60.0;
		return (int)minutes;
	}
	
	public int getRWMinutes() {
		double minutes = realWorldDayTime() - (int)realWorldDayTime();
		minutes *= 60.0;
		return (int)minutes;
	}
	
	public int getVRSeconds() {
		double minutes = _timeOfDay - (int)_timeOfDay;
		minutes *= 60.0;
		double seconds = minutes - (int)minutes;
		seconds *= 60.0;
		return (int) seconds;
	}
	
	public int getRWSeconds() {
		double minutes = realWorldDayTime() - (int)realWorldDayTime();
		minutes *= 60.0;
		double seconds = minutes - (int)minutes;
		seconds *= 60.0;
		return (int) seconds;
	}
	
	public int getVRHours() {
		return (int)_timeOfDay;
	}
	
	public int getRWHours() {
		return (int)realWorldDayTime();
	}
	
	public string vrTimeString() {
		double minutes = _timeOfDay - (int)_timeOfDay;
		//minutes /= 10.0f;
		minutes *= 60.0;
		string preH = "";
		string preM = "";
		if((int)_timeOfDay < 10)
			preH = "0";
		if(minutes < 10.0)
			preM = "0";
		return preH+(int)_timeOfDay+":"+preM+(int)minutes;
	}
	
	public string vrTimeStringExtended() {
		double minutes = _timeOfDay - (int)_timeOfDay;
		minutes *= 60.0;
		double seconds = minutes - (int)minutes;
		seconds *= 60.0;
		string preH = "";
		string preM = "";
		string preS = "";
		if((int)_timeOfDay < 10)
			preH = "0";
		if(minutes < 10.0)
			preM = "0";
		if(seconds < 10.0)
			preS = "0";
		return preH+(int)_timeOfDay+":"+preM+(int)minutes+":"+preS+(int)seconds;
	}
	
	public string rwTimeStringExtended() {
		double minutes = realWorldDayTime() - (int)realWorldDayTime();
		minutes *= 60.0;
		double seconds = minutes - (int)minutes;
		seconds *= 60.0;
		string preH = "";
		string preM = "";
		string preS = "";
		if((int)realWorldDayTime() < 10)
			preH = "0";
		if(minutes < 10.0)
			preM = "0";
		if(seconds < 10.0)
			preS = "0";
		return preH+(int)realWorldDayTime()+":"+preM+(int)minutes+":"+preS+(int)seconds;
	}
	
	public double vrTimeSpeed() {
		return _timeMultiplicator;
	}

	public double realWorldDayTime() {
		int seconds = System.DateTime.Now.Second + System.DateTime.Now.Minute*60 + System.DateTime.Now.Hour * 3600;
		return ((double)seconds / 86400.0) * 24.0;
	}
	
	public double getNumericRWTime() {
		return realWorldDayTime();
	}
	
	public double getNumericVRTime() {
		return _timeOfDay;
	}

	public void jumpToDayTime(double dayTime) {
		setDayTime(dayTime);
		sunStart();
	}













	// Use this for initialization
	void Start () {
		instance = this;
		init();
	}

	void getDeltaTime() {

	}
	
	void init() {
		_timeMultiplicator = 0.0f;
		_step = realWorldSpeedFactor()/3600.0f;
		_step*=4096.0f;
		//_step = 0.0f;
		setDayTime (12);
	}

	void setNightBoxOpacity(double opacity) {
		foreach(Renderer child in GameObject.Find("NightBox").GetComponentsInChildren<Renderer>()) {
			//Debug.Log (child.gameObject.name);
			child.material.color = new Color(child.material.color.r,child.material.color.g,child.material.color.b,(float)opacity);
		}
	}

	void sunStart() {
		//RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Custom;
		if(_timeOfDay > NIGHT_SHIFT_2 && _timeOfDay <= NIGHT_SHIFT_3) {//Abenddämmerung=>Nacht
			//RenderSettings.ambientSkyColor = DAWN_AMBIENT_COLOR;
			RenderSettings.ambientIntensity = AMBIENT_INTENSITY_DAWN;
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = WATER_INTENSITY_DAWN;
			RenderSettings.reflectionIntensity = AMBIENT_INTENSITY_DAWN;
			RenderSettings.fogColor = DAWN_FOG_COLOR;
			GetComponent<Light>().color = DAWN_COLOR;
			setFogDensity(FOG_INTENSITY_DAWN);
			setNightBoxOpacity(NIGHTBOX_DAWN);
			leftEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = DAWN_COLOR;
			rightEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = DAWN_COLOR;
			//UnityStandardAssets.Water.WaterBase.instance.reflectionColor = DAWN_COLOR;
		}
		else if(_timeOfDay > NIGHT_SHIFT_1 && _timeOfDay <= NIGHT_SHIFT_2) {//Tag=>Abenddämmerung
			//RenderSettings.ambientSkyColor = DAWN_AMBIENT_COLOR;
			RenderSettings.ambientIntensity = AMBIENT_INTENSITY_DAWN;
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = WATER_INTENSITY_DAWN;
			RenderSettings.reflectionIntensity = AMBIENT_INTENSITY_DAWN;
			RenderSettings.fogColor = DAWN_FOG_COLOR;
			GetComponent<Light>().color = DAWN_COLOR;
			setFogDensity(FOG_INTENSITY_DAWN);
			setNightBoxOpacity(NIGHTBOX_DAWN);
			//UnityStandardAssets.Water.WaterBase.instance.reflectionColor = DAWN_COLOR;
		}
		else if(_timeOfDay > DAY_SHIFT_3 && _timeOfDay <= NIGHT_SHIFT_1) {//Tag
			//RenderSettings.ambientGroundColor = Color.red;
			//RenderSettings.ambientSkyColor = DAY_AMBIENT_COLOR;
			RenderSettings.ambientIntensity = AMBIENT_INTENSITY_DAY;
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = WATER_INTENSITY_DAY;
			RenderSettings.reflectionIntensity = AMBIENT_INTENSITY_DAY;
			RenderSettings.fogColor = DAY_FOG_COLOR;
			GetComponent<Light>().color = DAY_COLOR;
			setFogDensity(FOG_INTENSITY_DAY);
			setNightBoxOpacity(NIGHTBOX_DAY);

			leftEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = DAY_COLOR;
			rightEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = DAY_COLOR;
			//UnityStandardAssets.Water.WaterBase.instance.reflectionColor = DAY_COLOR;
		}
		else if(_timeOfDay > DAY_SHIFT_2 && _timeOfDay <= DAY_SHIFT_3) {//Morgendämmerung=>Tag
			//RenderSettings.ambientSkyColor = DAY_AMBIENT_COLOR;
			RenderSettings.ambientIntensity = AMBIENT_INTENSITY_DAY;
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = WATER_INTENSITY_DAY;
			RenderSettings.reflectionIntensity = AMBIENT_INTENSITY_DAY;
			RenderSettings.fogColor = DAY_FOG_COLOR;
			GetComponent<Light>().color = DAY_COLOR;
			setFogDensity(FOG_INTENSITY_DAY);
			setNightBoxOpacity(NIGHTBOX_DAY);
		}
		else if(_timeOfDay > DAY_SHIFT_1 && _timeOfDay <= DAY_SHIFT_2) {//Nacht=>Morgendämmerung
			//RenderSettings.ambientSkyColor = DAWN_AMBIENT_COLOR;
			RenderSettings.ambientIntensity = AMBIENT_INTENSITY_DAWN;
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = WATER_INTENSITY_DAWN;
			RenderSettings.reflectionIntensity = AMBIENT_INTENSITY_DAWN;
			RenderSettings.fogColor = DAWN_FOG_COLOR;
			GetComponent<Light>().color = DAWN_COLOR;
			setFogDensity(FOG_INTENSITY_DAWN);
			setNightBoxOpacity(NIGHTBOX_DAWN);
			leftEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = DAWN_COLOR;
			rightEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = DAWN_COLOR;
			//UnityStandardAssets.Water.WaterBase.instance.reflectionColor = DAWN_COLOR;
		}
		else {//Nacht
			//RenderSettings.ambientSkyColor = NIGHT_AMBIENT_COLOR;
			RenderSettings.ambientIntensity = AMBIENT_INTENSITY_NIGHT;
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = WATER_INTENSITY_NIGHT;
			RenderSettings.reflectionIntensity = AMBIENT_INTENSITY_NIGHT;
			RenderSettings.fogColor = NIGHT_FOG_COLOR;
			GetComponent<Light>().color = NIGHT_COLOR;
			setFogDensity(FOG_INTENSITY_NIGHT);
			setNightBoxOpacity(NIGHTBOX_NIGHT);

			leftEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = DAWN_COLOR;
			rightEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = DAWN_COLOR;
			//UnityStandardAssets.Water.WaterBase.instance.reflectionColor = DAWN_COLOR;
		}
	}

	double timeToDegrees(double time) {
		while(time >= 24.0)
			time -= 24.0;
		return (360.0/24.0)*time;
	}

	double degreesToTime(double degrees) {
		while(degrees >= 360.0)
			degrees -= 360.0;
		return (24.0/360.0)*degrees;
	}

	double between(double a, double b, double c) {
		double d = c - b;
		double max = c - a;
		return 1.0 - (d/max);
	}

	public void setDayTime(double newDayTime) {
		//double oldDayTime = _timeOfDay;
		double oldSunAngle = timeToDegrees(_timeOfDay);
		double newSunAngle = timeToDegrees(newDayTime);
		_timeOfDay = newDayTime;
		while(newSunAngle < oldSunAngle)
			newSunAngle += 360.0;
		double oldStep = _step;
		_step = newSunAngle - oldSunAngle;
		_timeStep = degreesToTime(_step);
		lightAnimation(1.0);
		_step = oldStep;
		sunStart();
	}





	void calculateDelta() {

	}

	// Update is called once per frame
	void Update () {
		speedControl ();
		//delta = getNumericRWTime
		_timeStep = degreesToTime(_step);
		//_timeOfDay += _timeStep * Time.deltaTime;
		_timeOfDay += _timeStep * (float)GameManager.instance.DeltaTime;
		while(_timeOfDay >= 24.0f)
			_timeOfDay -= 24.0f;
		//Debug.Log (vrTimeString());

		lightAnimation((float)GameManager.instance.DeltaTime);
	}

	double realWorldSpeedFactor() {
		return timeToDegrees(1.0f);
	}

	public void setSpeed(float speed) {
		_step = realWorldSpeedFactor()/3600.0f;
		_step *= speed;
		_timeMultiplicator = speed;
	}

	void speedControl() {
		//if(!Input.anyKey)
			//return;
		//double step = _step;
		//_step = realWorldSpeedFactor()/3600.0f;

		if(Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Keypad0)) {
			setSpeed(0.0f);
		}
		else if(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1)) {
			setSpeed(1.0f);
		}
		else if(Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)) {
			setSpeed(2.0f);
		}
		else if(Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3)) {
			setSpeed(4.0f);
		}
		else if(Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4)) {
			setSpeed(16.0f);
		}
		else if(Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Keypad5)) {
			setSpeed(256.0f);
		}
		else if(Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Keypad6)) {
			setSpeed(512.0f);
		}
		else if(Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Keypad7)) {
			setSpeed(1024.0f);
		}
		else if(Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Keypad8)) {
			setSpeed(2048.0f);
		}
		else if(Input.GetKey(KeyCode.Alpha9) || Input.GetKey(KeyCode.Keypad9)) {
			setSpeed(4096.5f);
		}
	}

	void setFogDensity(double distanceValue) {
		double fogDist = distanceValue;
		fogDist *= 0.0001;
		fogDist = 1.0 - fogDist; 
		RenderSettings.fogDensity = (float)(fogDist*0.01);
	}

	void lightAnimation(double delta) {
		//transform.RotateAround(new Vector3(0,0,0),Vector3.forward,(float)(_step * delta));
		//transform.LookAt(new Vector3(0,0,0));
		Vector3 v = transform.parent.position;
		transform.RotateAround(v,Vector3.forward,(float)(_step * delta));
		transform.LookAt(v);

		GameObject.Find ("Moon").transform.RotateAround(new Vector3(0,0,0),Vector3.forward,(float)(_step * MOON_STEP * delta));
		GameObject.Find ("Moon").transform.LookAt(new Vector3(0,0,0));

		GameObject.Find ("NightBox").transform.Rotate(Vector3.forward,(float)(_step * BOX_STEP * delta));
			//.RotateAround(new Vector3(0,0,0),Vector3.forward,_step * BOX_STEP * Time.deltaTime);

		if(_timeOfDay > NIGHT_SHIFT_2 && _timeOfDay <= NIGHT_SHIFT_3) {//Abenddämmerung=>Nacht
			double p = between(NIGHT_SHIFT_2,_timeOfDay,NIGHT_SHIFT_3);
			//RenderSettings.ambientSkyColor = Color.Lerp(DAWN_AMBIENT_COLOR,NIGHT_AMBIENT_COLOR,p);
			RenderSettings.ambientIntensity = (float)(AMBIENT_INTENSITY_NIGHT + ((AMBIENT_INTENSITY_DAWN - AMBIENT_INTENSITY_NIGHT)*(1.0-p)));
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = (float)(WATER_INTENSITY_NIGHT + ((WATER_INTENSITY_DAWN - WATER_INTENSITY_NIGHT)*(1.0-p)));
			RenderSettings.reflectionIntensity = (float)(AMBIENT_INTENSITY_NIGHT + ((AMBIENT_INTENSITY_DAWN - AMBIENT_INTENSITY_NIGHT)*(1.0-p)));
			RenderSettings.fogColor = Color.Lerp(DAWN_FOG_COLOR,NIGHT_FOG_COLOR,(float)p);
			GetComponent<Light>().color = Color.Lerp(DAWN_COLOR,NIGHT_COLOR,(float)p);
			setFogDensity(FOG_INTENSITY_NIGHT + ((FOG_INTENSITY_DAWN - FOG_INTENSITY_NIGHT)*(1.0-p)));
			setNightBoxOpacity(NIGHTBOX_NIGHT + ((NIGHTBOX_DAWN - NIGHTBOX_NIGHT)*(1.0-p)));

			//Debug.Log ("Abenddämmerung=>Nacht");
		}
		else if(_timeOfDay > NIGHT_SHIFT_1 && _timeOfDay <= NIGHT_SHIFT_2) {//Tag=>Abenddämmerung
			double p = between(NIGHT_SHIFT_1,_timeOfDay,NIGHT_SHIFT_2);
			//RenderSettings.ambientSkyColor = Color.Lerp(DAY_AMBIENT_COLOR,DAWN_AMBIENT_COLOR,p);
			RenderSettings.ambientIntensity = (float)(AMBIENT_INTENSITY_DAWN + ((AMBIENT_INTENSITY_DAY - AMBIENT_INTENSITY_DAWN)*(1.0-p)));
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = (float)(WATER_INTENSITY_DAWN + ((WATER_INTENSITY_DAY - WATER_INTENSITY_DAWN)*(1.0-p)));
			RenderSettings.reflectionIntensity = (float)(AMBIENT_INTENSITY_DAWN + ((AMBIENT_INTENSITY_DAY - AMBIENT_INTENSITY_DAWN)*(1.0-p)));
			RenderSettings.fogColor = Color.Lerp(DAY_FOG_COLOR,DAWN_FOG_COLOR,(float)p);
			GetComponent<Light>().color = Color.Lerp(DAY_COLOR,DAWN_COLOR,(float)p);

			leftEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = Color.Lerp(DAY_COLOR,DAWN_COLOR,(float)p);
			rightEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = Color.Lerp(DAY_COLOR,DAWN_COLOR,(float)p);
			//UnityStandardAssets.Water.WaterBase.instance.reflectionColor = Color.Lerp(DAY_COLOR,DAWN_COLOR,(float)p);

			setFogDensity(FOG_INTENSITY_DAWN + ((FOG_INTENSITY_DAY - FOG_INTENSITY_DAWN)*(1.0-p)));
			setNightBoxOpacity(NIGHTBOX_DAWN + ((NIGHTBOX_DAY - NIGHTBOX_DAWN)*(1.0-p)));
			//Debug.Log ("Tag=>Abenddämmerung");
		}
		else if(_timeOfDay > DAY_SHIFT_3 && _timeOfDay <= NIGHT_SHIFT_1) {
			//Debug.Log ("Tag");
		}
		else if(_timeOfDay > DAY_SHIFT_2 && _timeOfDay <= DAY_SHIFT_3) {//Morgendämmerung=>Tag
			double p = between(DAY_SHIFT_2,_timeOfDay,DAY_SHIFT_3);
			//RenderSettings.ambientSkyColor = Color.Lerp(DAWN_AMBIENT_COLOR,DAY_AMBIENT_COLOR,p);
			RenderSettings.ambientIntensity = (float)(AMBIENT_INTENSITY_DAWN + ((AMBIENT_INTENSITY_DAY - AMBIENT_INTENSITY_DAWN)*p));
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = (float)(WATER_INTENSITY_DAWN + ((WATER_INTENSITY_DAY - WATER_INTENSITY_DAWN)*p));
			RenderSettings.reflectionIntensity = (float)(AMBIENT_INTENSITY_DAWN + ((AMBIENT_INTENSITY_DAY - AMBIENT_INTENSITY_DAWN)*p));
			RenderSettings.fogColor = Color.Lerp(DAWN_FOG_COLOR,DAY_FOG_COLOR,(float)p);
			GetComponent<Light>().color = Color.Lerp(DAWN_COLOR,DAY_COLOR,(float)p);

			leftEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = Color.Lerp(DAWN_COLOR,DAY_COLOR,(float)p);
			rightEye.GetComponent<UnityStandardAssets.ImageEffects.SunShafts>().sunColor = Color.Lerp(DAWN_COLOR,DAY_COLOR,(float)p);
			//UnityStandardAssets.Water.WaterBase.instance.reflectionColor = Color.Lerp(DAWN_COLOR,DAY_COLOR,(float)p);

			setFogDensity(FOG_INTENSITY_DAWN + ((FOG_INTENSITY_DAY - FOG_INTENSITY_DAWN)*p));
			setNightBoxOpacity(NIGHTBOX_DAWN + ((NIGHTBOX_DAY - NIGHTBOX_DAWN)*p));
			//Debug.Log ("Morgendämmerung=>Tag");
		}
		else if(_timeOfDay > DAY_SHIFT_1 && _timeOfDay <= DAY_SHIFT_2) {//Nacht=>Morgendämmerung
			double p = between(DAY_SHIFT_1,_timeOfDay,DAY_SHIFT_2);
			//RenderSettings.ambientSkyColor = Color.Lerp(NIGHT_AMBIENT_COLOR,DAWN_AMBIENT_COLOR,p);
			RenderSettings.ambientIntensity = (float)(AMBIENT_INTENSITY_NIGHT + ((AMBIENT_INTENSITY_DAWN - AMBIENT_INTENSITY_NIGHT)*p));
			//UnityStandardAssets.Water.WaterBase.instance.opacityFactor = (float)(WATER_INTENSITY_NIGHT + ((WATER_INTENSITY_DAWN - WATER_INTENSITY_NIGHT)*p));
			RenderSettings.reflectionIntensity = (float)(AMBIENT_INTENSITY_NIGHT + ((AMBIENT_INTENSITY_DAWN - AMBIENT_INTENSITY_NIGHT)*p));
			RenderSettings.fogColor = Color.Lerp(NIGHT_FOG_COLOR,DAWN_FOG_COLOR,(float)p);
			GetComponent<Light>().color = Color.Lerp(NIGHT_COLOR,DAWN_COLOR,(float)p);
			setFogDensity(FOG_INTENSITY_NIGHT + ((FOG_INTENSITY_DAWN - FOG_INTENSITY_NIGHT)*p));
			setNightBoxOpacity(NIGHTBOX_NIGHT + ((NIGHTBOX_DAWN - NIGHTBOX_NIGHT)*p));
			//Debug.Log ("Nacht=>Morgendämmerung");
		}
		else {
			//Debug.Log ("Nacht");
		}
	}
}
