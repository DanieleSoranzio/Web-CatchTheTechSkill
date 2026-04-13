using UnityEngine;

public class Poolable : MonoBehaviour
{
    [SerializeField] public int PoolSize=10;
    #region Mono
    public virtual void Start()
    {
 
    }
    
    public virtual void OnDisable()
    {
        ObjectPooler.Instance.BackToPool(this);
    }
    
    #endregion
}
