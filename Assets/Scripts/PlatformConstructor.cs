using UnityEngine;
using System.Collections;

public class PlatformConstructor : MonoBehaviour 
{
	public GameObject platform;
	private Transform constructionPoint;
	private float distanceBetweenMin = 1;
	private float distanceBetweenMax = 3;
	private float platformWidth;

	// Use this for initialization
	void Start () 
	{
		constructionPoint = GameObject.Find ("PlatformConstructionPoint").transform;
		platformWidth = platform.GetComponent<BoxCollider2D> ().size.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.position.x < constructionPoint.position.x)
		{
			transform.position = new Vector3(transform.position.x + platformWidth + Random.Range (distanceBetweenMin, distanceBetweenMax), 
				transform.position.y, transform.position.z);
				Instantiate(platform, transform.position, transform.rotation);
		}		
	}
}
