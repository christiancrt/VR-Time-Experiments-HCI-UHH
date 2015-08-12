using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour, FeedbackDelegate {

	public GameObject parent;

	// Use this for initialization
	void Start () {
		Object o = Resources.Load("VerbalTask");
		GameObject g = (Instantiate(o)as GameObject);
		VerbalTask v = g.GetComponent<VerbalTask>();
		v.addToGameObject (parent);
		v.activate(true,this);
	}
	
	// Update is called once per frame
	public void feedbackTriger() {
		Debug.Log ("Feedback");
	}
}
