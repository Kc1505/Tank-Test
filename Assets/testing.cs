using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour {

	public GameObject block1;
	public GameObject block2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("i"))
		{
			transform.RotateAround(block2.transform.position, new Vector3(0, 0, 1), 90);
		}
	}
}
