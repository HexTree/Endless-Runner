using UnityEngine;

public class PlatformDestructor : MonoBehaviour 
{
   private PoolObject poolObject = null;
	private Transform destructionPoint;

	// Use this for initialization
	void Start () 
   {
      poolObject = GetComponent<PoolObject>();
		destructionPoint = GameObject.Find ("PlatformDestructionPoint").transform;
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.x < destructionPoint.position.x) {
         if (poolObject != null) {
            poolObject.Destroy();
         }
         else 
         {
            Debug.LogWarning("This should be a PoolObject");
            Destroy(gameObject);
         }
      }
			
	}
}
