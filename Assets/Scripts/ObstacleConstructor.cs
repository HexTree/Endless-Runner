using UnityEngine;
using System.Collections.Generic;

public class ObstacleConstructor : MonoBehaviour 
{
   public GameObject prefab;

   private Transform player = null;
   private Rigidbody2D playerBody;
   private PlayerControl playerControl;
   private float playerWidth;
   private GameObject previousSpike = null;

   // Use this for initialization
   void Start() 
   {
      ObjectPooler.instance.CreatePool(prefab, 10);
      player = GameObject.Find("Player").transform;
      playerBody = player.GetComponent<Rigidbody2D>();
      playerControl = player.GetComponent<PlayerControl>();
      playerWidth = player.GetComponent<BoxCollider2D>().size.x;
   }

   public void ConstructObject(Transform previous, Transform current) 
   {
      if (previous == null || current == null) 
      {
         return;
      }

      const int OBJECT_SPAWN_RATE = 50;

      // OBJECT_SPAWN_RATE percent chance to spawn object if conditions are valid
      if (Random.Range(0, 100) >= OBJECT_SPAWN_RATE) 
      {
         if (SpawnObject(previous, current)) 
         {

         }
      }
   }

   private bool SpawnObject(Transform previous, Transform current) 
   {
      float distanceOfJump = (playerBody.velocity.x * playerControl.GetJumpForce()) / 
                             ((Physics2D.gravity.y * -1) * playerBody.gravityScale * 0.5f);

      float previousWidth = previous.GetComponent<BoxCollider2D>().size.x;
      float currentWidth = current.GetComponent<BoxCollider2D>().size.x;

      float previousRightBoundary = previous.position.x + (previousWidth / 2);
      float currentLeftBoundary = current.position.x - (currentWidth / 2);
      float currentRightBoundary = current.position.x + (currentWidth / 2);

      float gapBetweenPlatforms = current.position.x - previousRightBoundary;
      float jumpLocationToReachPlatform = currentLeftBoundary + playerWidth - distanceOfJump;
      float jumpLocationAtLastMoment = previousRightBoundary + distanceOfJump;

      float minPosition = currentLeftBoundary;
      float maxPosition = currentRightBoundary;

      if (previousSpike != null) 
      {
         float spikeWidth = previousSpike.GetComponent<BoxCollider2D>().size.x;
         float lastMomentJump = jumpLocationToReachPlatform - (previousSpike.transform.position.x + (spikeWidth / 2));

         if (lastMomentJump > playerWidth * 1.5f) 
         {
            minPosition = currentLeftBoundary + (spikeWidth / 2);
            if (((previousRightBoundary - playerWidth) + distanceOfJump) < (minPosition - playerWidth)) 
            {
               Debug.Log("Adjusted because couldn't jump that far");
               minPosition += (playerWidth * 1.5f);
            }
         }
         else {
            Debug.Log("Not here");
         }

         // Isn't right but should be good enough for now to setup next jump
         if ((jumpLocationAtLastMoment - (spikeWidth * 1.5f) + distanceOfJump) < currentRightBoundary)
         {
            // If this is true, youre fucked. Well if I used previous spike stuff but that should be prevented by this.
            maxPosition -= (playerWidth * 1.5f);
         }
      }

      Vector3 randomPosition = new Vector3(Random.Range(minPosition, maxPosition), -3.1f);
      previousSpike = ObjectPooler.instance.ReuseObject(prefab, randomPosition, transform.rotation);

      return false;
   }
}
