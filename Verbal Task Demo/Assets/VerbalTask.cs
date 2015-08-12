using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum VerbalStimulus {
	A,
	B,
	C,
	D,
	NULL
}

public class VerbalTask : MonoBehaviour {

	GameObject beachChair = GameObject.Find("beach_chair_1");
	VerbalStimulus displayedStimulus = VerbalStimulus.NULL;
	VerbalStimulus hiddenStimulus = VerbalStimulus.NULL;
	VerbalStimulus toBeMatchedStimulus = VerbalStimulus.NULL;
	GameObject displayedObject = null;
	int _animationStage = 0;
	double _counter = 0.0f;
	bool active = false;
	bool choiceMade = false;
	bool _isTutorial = false;
	FeedbackDelegate _delegate;

	int hits = 0;
	int fails = 0;
	int misses = 0;
	int tN = 0;

	public int truePositives() {
		return hits;
	}

	public int falsePositives() {
		return fails;
	}

	public int falseNegatives() {
		return misses;
	}

	public int trueNegatives() {
		return tN;
	}

	void Start () {

	}

	public void addToBeachChair(int chair) {
		beachChair = GameObject.Find("beach_chair_"+chair);
	}

	public void addToGameObject(GameObject o) {
		beachChair = o;
	}

	public void activate() {
		activate(false,null);
	}

	public void activate(bool isTutorial, FeedbackDelegate fbDel) {
		active = true;
		nextStimulus();
		display(displayedStimulus);
		_isTutorial = isTutorial;
		_delegate = fbDel;
	}

	void nextStimulus(){
		toBeMatchedStimulus = hiddenStimulus;
		hiddenStimulus = displayedStimulus;

		//System.Random r = new System.Random();
		//do {
			//switch(r.Next(1,5)) {
		switch(fakeRandomGenerator()) {
			case 1:
				displayedStimulus = VerbalStimulus.A;
				break;
			case 2:
				displayedStimulus = VerbalStimulus.B;
				break;
			case 3:
				displayedStimulus = VerbalStimulus.C;
				break;
			case 4:
				displayedStimulus = VerbalStimulus.D;
				break;
			default:
				break;
			}
		//} while(displayedStimulus==hiddenStimulus);
	}

	void display(VerbalStimulus stimulus) {
		char x = 'X';
		switch(stimulus) {
		case VerbalStimulus.A:
			x = 'A';
			break;
		case VerbalStimulus.B:
			x = 'B';
			break;
		case VerbalStimulus.C:
			x = 'C';
			break;
		case VerbalStimulus.D:
			x = 'D';
			break;
		case VerbalStimulus.NULL:
			x = 'S';
			break;
		}

		if(displayedObject!=null)
			GameObject.Destroy(displayedObject);

		displayedObject = Instantiate(Resources.Load("VerbalWall"+x))as GameObject;
		displayedObject.transform.parent = beachChair.transform;
		displayedObject.transform.position = new Vector3(0,0,0);
		displayedObject.transform.eulerAngles = new Vector3(0,0,0);
		displayedObject.transform.localPosition = new Vector3(-0.4f,-2.0f,76.0f);
		displayedObject.transform.localEulerAngles = new Vector3(2.45947f,180.7448f,0.07715777f);
	}

	void choiceDetector() {
		if(toBeMatchedStimulus==VerbalStimulus.NULL)
			return;
		if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Joystick1Button2)) {
			if(choiceMade)
				return;
			choiceMade = true;
			GameObject.Find("ButtonPressSound").GetComponent<AudioSource>().Play();
			if(toBeMatchedStimulus==displayedStimulus) {
				hits += 1;
			}
			else {
				fails += 1;
			}
		}
	}

	public void endTheGame() {
		if(_delegate != null)
			_delegate.feedbackTriger();
		GameObject.Destroy(gameObject);
		GameObject.Destroy(displayedObject);
	}

	public void stop() {
		active = false;
	}
	
	void Update () {
		if(!active)
			return;
		if(_isTutorial && Input.GetKeyDown(KeyCode.Space)) {
			_delegate.feedbackTriger();
			GameObject.Destroy(displayedObject);
			active = false;
			GameObject.Destroy(gameObject);
			return;
		}
		switch(_animationStage) {
		case 0:
			choiceMade = false;
			_counter = 0.5;
			_animationStage += 1;
			break;
		case 1:
			_counter -= GameManager.instance.DeltaTime;
			if(_counter <= 0.0f)
				_animationStage += 1;
			choiceDetector();

			break;
		case 2:
			System.Random rnd = new System.Random();
			int c = rnd.Next(1100,1500);
			_counter = c/1000.0;
			choiceDetector();
			display(VerbalStimulus.NULL);
			_animationStage += 1;
			break;
		case 3:
			_counter -= GameManager.instance.DeltaTime;
			if(_counter <= 0.0f)
				_animationStage += 1;
			choiceDetector();
			break;
		case 4:
			if(toBeMatchedStimulus==displayedStimulus && !choiceMade) {
				misses += 1;
			}
			else if(toBeMatchedStimulus!=displayedStimulus && !choiceMade) {
				tN += 1;
			}
			nextStimulus();
			display(displayedStimulus);
			_animationStage = 0;
			break;
		}

	}

	static long fakeSeed = 1234567890572018520L;
	long fakeRemaining = -1;
	static long fakeSeedTutorial = 7462908749222317898L;
	long fakeRemainingTutorial = -1;

	private int fakeRandomGenerator() {
		int result = 0;
		if(_isTutorial) {
			if(fakeRemaining<=0) {
				fakeRemaining = fakeSeed;
			}
			result = (int)(fakeRemaining % 4)+1;
			fakeRemaining = fakeRemaining/4;
		}
		else {
			if(fakeRemainingTutorial<=0) {
				fakeRemainingTutorial = fakeSeedTutorial;
			}
			result = (int)(fakeRemainingTutorial % 4)+1;
			fakeRemainingTutorial = fakeRemainingTutorial/4;
		}
		return result;
	}
}
