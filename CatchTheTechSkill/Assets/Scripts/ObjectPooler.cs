using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
     #region Variables
    
      Dictionary<Poolable,Queue<Poolable>> objectsPool = new Dictionary<Poolable,Queue<Poolable>>();
      Dictionary<Poolable,GameObject> parents = new Dictionary<Poolable,GameObject>();
      
     #endregion
     
     #region Methods

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

     public void BackToPool(Poolable poolable)
     {
         Poolable key = poolable.PrefabSource;
         if (objectsPool.ContainsKey(key))
         {
             poolable.gameObject.transform.SetParent(parents[key].transform);
             poolable.gameObject.transform.position = new Vector3();
             poolable.gameObject.SetActive(false);
             poolable.gameObject.SetActive(false);
             objectsPool[key].Enqueue(poolable);
             CheckBounds(key);
         }
         else
         {
             Destroy(poolable.gameObject);
         }
     }

     public void DeletePool(Poolable poolable)
     {
         Poolable key = poolable.PrefabSource;
         if(objectsPool.ContainsKey(key))
         {
             foreach (Poolable tempObj in objectsPool[key])
             {
                Destroy(tempObj.gameObject);
             }
             objectsPool[key].Clear();
             objectsPool.Remove(key);
             Destroy(parents[key]);
             parents.Remove(key);
         }
             
     }
     
     #endregion
     
}
