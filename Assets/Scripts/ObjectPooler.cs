﻿using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

   Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

   private static ObjectPooler mInstance;

   public static ObjectPooler instance {
      get {
         if (mInstance == null) {
            mInstance = FindObjectOfType<ObjectPooler>();
         }
         return mInstance;
      }
   }

   public void CreatePool(GameObject prefab, int poolSize) {
      int poolKey = prefab.GetInstanceID();

      if (!poolDictionary.ContainsKey(poolKey)) {
         poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

         GameObject poolHolder = new GameObject(prefab.name + " Pool");
         poolHolder.transform.parent = transform;

         for (int i = 0; i < poolSize; i++) {
            ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
            poolDictionary[poolKey].Enqueue(newObject);
            newObject.SetParent(poolHolder.transform);
         }
      }
   }

   public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation) {
      GameObject reuseObject = null;
      int poolKey = prefab.GetInstanceID();

      if (poolDictionary.ContainsKey(poolKey)) {
         ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
         // TODO Need out if nothing is available.  Create a new slot.
         while (objectToReuse.gameObject.activeSelf == true)
         {
            poolDictionary[poolKey].Enqueue(objectToReuse);
            objectToReuse = poolDictionary[poolKey].Dequeue();
         }

         poolDictionary[poolKey].Enqueue(objectToReuse);
         objectToReuse.Reuse(position, rotation);
         reuseObject = objectToReuse.gameObject;
      }

      return reuseObject;
   }

   public class ObjectInstance {

      public GameObject gameObject;

      private Transform transform;
      private bool hasPoolObjectComponent;
      private PoolObject poolObjectScript;

      public ObjectInstance(GameObject objectInstance) {
         gameObject = objectInstance;
         transform = gameObject.transform;
         gameObject.SetActive(false);

         if (gameObject.GetComponent<PoolObject>()) {
            hasPoolObjectComponent = true;
            poolObjectScript = gameObject.GetComponent<PoolObject>();
         }
      }

      public void Reuse(Vector3 position, Quaternion rotation) {
         transform.position = position;
         transform.rotation = rotation;
         if (hasPoolObjectComponent) {
            poolObjectScript.OnObjectReuse();
         }
         gameObject.SetActive(true);
      }

      public void SetParent(Transform parent) {
         transform.parent = parent;
      }
   }
}