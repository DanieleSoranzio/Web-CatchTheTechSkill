using System.Collections;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    #region Variables
    
    [SerializeField,Range(10f,100f)] public int PoolSize=10;
    [SerializeField,Range(10f,100f)] public int Max_PoolSize=50;
    public Poolable PrefabSource { get; internal set; }
    
    #endregion
    
    #region Mono
    public virtual void Start()
    {

    }
    
    public virtual void OnDisable()
    {
       
    }
    
    #endregion
    
    #region Methods
    public void Register()
    {
        ObjectPooler.Instance.RegisterPoolable(this);
    }

    public void ReturnToPool()
    {
        ObjectPooler.Instance.BackToPool(this);
    }

    private void IncreaseMaxPoolSize(int amount)
    {
        Max_PoolSize += amount;
    }
    
    #endregion
}
