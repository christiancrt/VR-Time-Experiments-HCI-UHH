using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour, FeedbackDelegate {

	public GameObject parent;

	// Use this for initialization
	void Start () {
		Object o3;
		o3 = Resources.Load("SpacialTask");
		GameObject g3 = (Instantiate(o3)as GameObject);
		SpacialTask s = g3.GetComponent<SpacialTask>();
		s.addToGameObject(parent);
		s.activate(true,this);
	}
	
	public void feedbackTriger() {
		Debug.Log ("Feedback");
	}
}
