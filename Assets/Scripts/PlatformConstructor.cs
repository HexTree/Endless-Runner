using UnityEngine;
using System.Collections;

public class PlatformConstructor : MonoBehaviour 
{
   public GameObject platform = null;

   private const float MIN_DISTANCE_BETWEEN_PLATFORMS = 1;
   private const float MAX_DISTANCE_BETWEEN_PLATFORMS = 3;
   private float platformWidth;

   private ObstacleConstructor obstacleConstructor;
   private Transform constructionPoint = null;
   private Transform previousPlatform = null;

   // Use this for initialization
   void Start () 
	{
      // Create a pool for platforms of size 10
      ObjectPooler.instance.CreatePool(platform, 10);

      obstacleConstructor = GetComponent<ObstacleConstructor>();
      constructionPoint = GameObject.Find ("PlatformConstructionPoint").transform;
		platformWidth = platform.GetComponent<BoxCollider2D>().size.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
      // Platform Constructor gets moved as a new platform is created.
      // Construction Point gets moved as the camera moves.
		if (transform.position.x < constructionPoint.position.x)
		{
         GameObject currentPlatform;

         // Update the position of the Platform Constructor so it will spawn another platform when ready.
         transform.position = new Vector3(transform.position.x + platformWidth + 
            Random.Range (MIN_DISTANCE_BETWEEN_PLATFORMS, MAX_DISTANCE_BETWEEN_PLATFORMS), 
				transform.position.y, transform.position.z);

         // Create a new platform
         currentPlatform = ObjectPooler.instance.ReuseObject(platform, transform.position, transform.rotation);

         // Create any obstacles on the platforms
         obstacleConstructor.ConstructObject(previousPlatform, currentPlatform.transform);
      }		
	}
}
