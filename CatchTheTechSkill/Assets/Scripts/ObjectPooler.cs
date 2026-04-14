using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
     #region Variables
    
      Dictionary<Poolable,Queue<Poolable>> objectsPool = new Dictionary<Poolable,Queue<Poolable>>();
      Dictionary<Poolable,GameObject> parents = new Dictionary<Poolable,GameObject>();
      
     #endregion
     
     #region Methods

     [Tooltip("Will return a request object, will create a new pool if there's none.")]
     public Poolable GetPoolable(Poolable poolable)
     {
         
         if (!objectsPool.ContainsKey(poolable))
             CreatePool(poolable);
    
         if (objectsPool[poolable].Count == 0)
             RegeneratePool(poolable);

         Poolable temp = objectsPool[poolable].Dequeue();
         temp.transform.SetParent(null);
         
         return temp;
     }
    
     [Tooltip("Will register the object to a new pool.")]
     public void RegisterPoolable(Poolable poolable)
     {
         if (!objectsPool.ContainsKey(poolable))
         {
             CreatePool(poolable);
         }
     }
    
     private void CreatePool(Poolable poolable)
     {
         objectsPool.Add(poolable,new Queue<Poolable>());
         GameObject goParent = new GameObject();
         goParent.name = poolable.gameObject.name;
         goParent.transform.SetParent(transform);
         parents.Add(poolable,goParent);
         RegeneratePool(poolable);
     }

     private void RegeneratePool(Poolable poolable)
     {

         if (objectsPool.ContainsKey(poolable))
         {
             int PoolSize = poolable.PoolSize;   
             
             for (int i = 0; i < PoolSize ; i++)
             {
                 Poolable temp= Instantiate(poolable,Vector3.zero,Quaternion.identity,parents[poolable].transform);
                 temp.PrefabSource = poolable;
                 temp.gameObject.SetActive(false);
                 objectsPool[poolable].Enqueue(temp);
             }
         }
         CheckBounds(poolable);
     }

     private void CheckBounds(Poolable poolable)
     {
         if (objectsPool.ContainsKey(poolable))
         {
             if (objectsPool[poolable].Count > poolable.Max_PoolSize &&  poolable.Max_PoolSize > 0)
             {
                 int diff=objectsPool[poolable].Count - poolable.Max_PoolSize;
                 for (int i = 0; i < diff; i++)
                 {
                     Poolable temp = objectsPool[poolable].Dequeue();
                     Destroy(temp.gameObject);
                 }
             }
         }
     }
     [Tooltip("Will return the object to his pool. If the bool doesn't exist it will get destroyed.")]
     public void BackToPool(Poolable poolable)
     {
         Poolable key = poolable.PrefabSource;
         if (key != null)
         {
             if(objectsPool.ContainsKey(key))
             {
                 poolable.gameObject.transform.SetParent(parents[key].transform);
                 poolable.gameObject.transform.position = new Vector3();
                 poolable.gameObject.SetActive(false);
                 objectsPool[key].Enqueue(poolable);
                 CheckBounds(key);
             }
             else
             {
                 Destroy(poolable.gameObject);
             }
         }
         else
         {
             Destroy(poolable.gameObject);
         }
        
     }
     [Tooltip("Will delete the pool of the object given type.")]
     public void DeletePool(Poolable poolable)
     {
         Poolable key = poolable.PrefabSource;
         if (key != null)
         {
             if (objectsPool.ContainsKey(key))
             {
                 objectsPool[key].Clear();
                 objectsPool.Remove(key);
                 Destroy(parents[key].gameObject);
                 parents.Remove(key);
             }
         }
     }
     #endregion
}
