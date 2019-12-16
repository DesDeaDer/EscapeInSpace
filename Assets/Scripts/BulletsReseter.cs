using UnityEngine;

public class BulletsReseter : MonoBehaviour
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private PoolBullets _pool;

#pragma warning restore 0649
    #endregion

    public void Reset()
    {
        var bulletsShip = GetComponentsInChildren<BulletController>();
        foreach (var item in bulletsShip)
        {
            _pool.Set(item);
        }
    }
}
