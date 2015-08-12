using UnityEngine;
using System.Collections;

public enum StimulusPosition {
	center,
	left,
	right
}

public class SpacialTask : MonoBehaviour {

	GameObject beachChair = GameObject.Find("beach_chair_1");
	int _animationStage = 0;
	double _counter = 0.0f;
	bool active = false;
	int _stimulus = 1;
	GameObject _displayedStimulusC = null;
	GameObject _displayedStimulusL = null;
	GameObject _displayedStimulusR = null;
	bool _currentIsCorrect;
	bool choiceMade = false;
	const int MAXIMUM = 24;
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

	// Use this for initialization
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
		_isTutorial = isTutorial;
		_delegate = fbDel;
	}
	
	void nextStimulus(){
		//_stimulus += 1;
		//if(_stimulus > MAXIMUM)
			//_stimulus = 1;
		_stimulus = fakeRandomGenerator2 ();
	}

	void displayChoice(int stimulus) {
		System.Random rand = new System.Random();
		//int s = rand.Next(1,50);
		int s;
		if(fakeRandomGenerator()!=1) {
			Debug.Log("Mirrored");
			_currentIsCorrect = false;
			s = -1;
		}
		else {
			Debug.Log("Original");
			s = 1;
			_currentIsCorrect = true;
		}
		createStimulusAt(StimulusPosition.center, stimulus, s, true);

	}

	void createStimulusAt(StimulusPosition position, int stimulus, int scale, bool totalRandom) {
		switch(position) {
		case StimulusPosition.center:
			if(_displayedStimulusR)
				GameObject.Destroy(_displayedStimulusR);
			if(_displayedStimulusL)
				GameObject.Destroy(_displayedStimulusL);
			if(_displayedStimulusC)
				GameObject.Destroy(_displayedStimulusC);
			_displayedStimulusC = createStimulusAt(new Vector3(0.0f,1.624638f,1.864399f),stimulus,scale,totalRandom);
			break;
		case StimulusPosition.right:
			if(_displayedStimulusC)
				GameObject.Destroy(_displayedStimulusC);
			if(_displayedStimulusR)
				GameObject.Destroy(_displayedStimulusR);
			_displayedStimulusR = createStimulusAt(new Vector3(-1.5f,2.624638f,1.864399f),stimulus,scale,totalRandom);
			break;
		case StimulusPosition.left:
			if(_displayedStimulusC)
				GameObject.Destroy(_displayedStimulusC);
			if(_displayedStimulusL)
				GameObject.Destroy(_displayedStimulusL);
			_displayedStimulusL = createStimulusAt(new Vector3(1.5f,2.624638f,1.864399f),stimulus,scale,totalRandom);
			break;
		}
	}

	int lastZ;
	int lastY;
	int lastX;

	GameObject createStimulusAt(Vector3 position, int stimulus, int scale, bool totalRandom) {
		GameObject ds;
		ds = Instantiate(Resources.Load("spacial/"+stimulus))as GameObject;
		ds.transform.parent = beachChair.transform;
		ds.transform.position = new Vector3(0,0,0);
		ds.transform.eulerAngles = new Vector3(0,0,0);
		ds.transform.localPosition = position;

		System.Random rand = new System.Random();
		//int x = rand.Next(0,360);
		//int y = rand.Next(0,360);
		int z = rand.Next(0,360);
		int y = rand.Next(0,360);
		int x = rand.Next(0,360);
		float r = 0;
		if(totalRandom) {
			if(z>180) {
				z = lastZ;
				r = 45.0f;
			}
			else {
				z =  lastZ;
				r = 315.0f;
			}
			x = lastX;
			y = lastY;
			}
		else {
			lastZ = z;
			lastX = x;
			lastY = y;
		}

		ds.transform.localScale = new Vector3(0.125f,0.125f,0.125f);

		if (scale < 0) {
			ds.transform.Rotate (new Vector3 (1, 0, 0), 90.0f);
			ds.transform.Rotate (new Vector3 (0, 1, 0), 90.0f);
			ds.transform.Rotate (new Vector3 (0, 0, 1), 90.0f);
		}
		ds.transform.localScale = new Vector3(ds.transform.localScale.x*scale,ds.transform.localScale.y*scale,ds.transform.localScale.z*scale);


		ds.transform.Rotate(new Vector3(0,1,0),r);
		return ds;
	}

	void hit() {
		GameObject.Find("ButtonPressSound").GetComponent<AudioSource>().Play();
		if(_currentIsCorrect) {
			hits += 1;
		}
		else {
			fails += 1;
		}
	}

	public void endTheGame() {
		if(_delegate != null)
			_delegate.feedbackTriger();
		GameObject.Destroy(gameObject);
		if(_displayedStimulusC!=null)
			GameObject.Destroy(_displayedStimulusC);
		if(_displayedStimulusL!=null)
			GameObject.Destroy(_displayedStimulusL);
		if(_displayedStimulusR!=null)
			GameObject.Destroy(_displayedStimulusR);
	}
	
	public void stop() {
		active = false;
	}

	
	// Update is called once per frame
	void Update () {
		if(!active)
			return;

		if(_isTutorial && Input.GetKeyDown(KeyCode.Space)) {
			_delegate.feedbackTriger();

			if(_displayedStimulusC!=null)
				GameObject.Destroy(_displayedStimulusC);
			if(_displayedStimulusL!=null)
				GameObject.Destroy(_displayedStimulusL);
			if(_displayedStimulusR!=null)
				GameObject.Destroy(_displayedStimulusR);
			active = false;
			GameObject.Destroy(gameObject);
			return;
		}

		switch(_animationStage) {
		case 0://Nächsten Stimulus anzeigen
			_counter = 1.5;
			choiceMade = false;
			nextStimulus();
			createStimulusAt(StimulusPosition.center,_stimulus,1,false);
			_animationStage += 1;
			break;
		case 1://Stimulus wird angezeigt
			_counter -= GameManager.instance.DeltaTime;
			if(_counter <= 0.0f)
				_animationStage += 1;
			break;
		case 2://Stimulus wird ausgeblendet
			System.Random rnd = new System.Random();
			int c = rnd.Next(1100,1500);
			_counter = c/1000.0;
			GameObject.Destroy(_displayedStimulusC);
			_animationStage += 1;
			break;
		case 3://kurze Wartezeit, dann Anzeige des Test-Stimulus
			_counter -= GameManager.instance.DeltaTime;
			if(_counter <= 0.0f) {
				displayChoice(_stimulus);
				_counter = 3.5;
				_animationStage += 1;
			}
			break;
		case 4://Zeit, in der reagiert werden kann
			if((Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Joystick1Button2))&&!choiceMade) {
				choiceMade = true;
				hit ();
			}

			_counter -= GameManager.instance.DeltaTime;
			if(_counter <= 0.0f) {
				if(_currentIsCorrect && !choiceMade) {
					misses += 1;
				}
				else if(!_currentIsCorrect && !choiceMade) {
					tN += 1;
				}
				_animationStage = 0;
			}
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
			result = (int)(fakeRemaining % 2)+1;
			fakeRemaining = fakeRemaining/2;
		}
		else {
			if(fakeRemainingTutorial<=0) {
				fakeRemainingTutorial = fakeSeedTutorial;
			}
			result = (int)(fakeRemainingTutorial % 2)+1;
			fakeRemainingTutorial = fakeRemainingTutorial/2;
		}
		return result;
	}
	
	long fakeRemaining2 = -1;
	long fakeRemainingTutorial2 = -1;
	
	private int fakeRandomGenerator2() {
		int result = 0;
		if(_isTutorial) {
			if(fakeRemaining<=0) {
				fakeRemaining = fakeSeed;
			}
			result = (int)(fakeRemaining % 30)+1;
			fakeRemaining = fakeRemaining/30;
		}
		else {
			if(fakeRemainingTutorial<=0) {
				fakeRemainingTutorial = fakeSeedTutorial;
			}
			result = (int)(fakeRemainingTutorial % 30)+1;
			fakeRemainingTutorial = fakeRemainingTutorial/30;
		}
		return result;
	}
}
