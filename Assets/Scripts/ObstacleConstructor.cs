using UnityEngine;
using System.Collections;

public class ObstacleConstructor : MonoBehaviour 
{
   public GameObject prefab;

   private Transform player = null;

   // Use this for initialization
   void Start() 
   {
      player = GameObject.Find("Player").transform;
   }

   public void ConstructObject(Transform previous, Transform current) 
   {
      const int OBJECT_SPAWN_RATE = 50;

      // OBJECT_SPAWN_RATE percent chance to spawn object if conditions are valid
      if (Random.Range(0, 100) >= OBJECT_SPAWN_RATE) 
      {
         if (SpawnObjectConditionsValid()) 
         {

         }
      }
   }

   private bool SpawnObjectConditionsValid() 
   {
      Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
      PlayerControl pc = player.GetComponent<PlayerControl>();
      float distanceOfJump = (rb.velocity.x * pc.GetJumpForce()) / ((Physics2D.gravity.y * -1) * rb.gravityScale * 0.5f);
      RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

      if (hit.collider != null) 
      {
         float startOfPlatform = hit.transform.position.x;// - platformWidth;
         if (player.position.x > distanceOfJump) 
         {

         }
      }
      return false;
   }
}
