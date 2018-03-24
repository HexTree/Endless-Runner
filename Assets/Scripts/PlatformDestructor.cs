using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestructor : MonoBehaviour {
	private Transform destructionPoint;

	// Use this for initialization
	void Start () {
		destructionPoint = GameObject.Find ("PlatformDestructionPoint").transform;
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.x < destructionPoint.position.x)
			Destroy (gameObject);
	}
}
