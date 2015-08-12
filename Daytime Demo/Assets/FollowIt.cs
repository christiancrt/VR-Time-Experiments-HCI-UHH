using UnityEngine;
using System.Collections;

public class FollowIt : MonoBehaviour {

	public GameObject _parent;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(_parent.transform.position.x,_parent.transform.position.y,_parent.transform.position.z);
	}
}
