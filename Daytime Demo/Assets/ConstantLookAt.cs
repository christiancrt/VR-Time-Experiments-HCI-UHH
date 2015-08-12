using UnityEngine;
using System.Collections;

public class ConstantLookAt : MonoBehaviour {

	public GameObject _target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(_target.transform.position);
	}
}
