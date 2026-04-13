using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
     Dictionary<Poolable,Queue<Poolable>> objectsPool = new Dictionary<Poolable,Queue<Poolable>>();
     Dictionary<Poolable,GameObject> parents = new Dictionary<Poolable,GameObject>();
    
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
                 temp.gameObject.SetActive(false);
                 objectsPool[poolable].Enqueue(temp);
             }
         }
     }

     public void BackToPool(Poolable poolable)
     {
         if (objectsPool.ContainsKey(poolable))
         {
             GameObject go=poolable.gameObject; 
             go.SetActive(false);
             go.transform.SetParent(parents[poolable].transform);
             objectsPool[poolable].Enqueue(poolable);
         }
     }

     public void DeletePool(Poolable poolable)
     {
         if(objectsPool.ContainsKey(poolable))
         {
             foreach (Poolable tempObj in objectsPool[poolable])
             {
                Destroy(tempObj.gameObject);
             }
             objectsPool[poolable].Clear();
             objectsPool.Remove(poolable);
             Destroy(parents[poolable]);
             parents.Remove(poolable);
         }
             
     }
     #endregion
}
