using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsReseter : MonoBehaviour
{
	[SerializeField] private PoolBullets _pool;

	public void Reset()
	{
		var bulletsShip = GetComponentsInChildren<BulletController>();
		foreach (var item in bulletsShip) 
		{
			_pool.Set(item);
		}
	}
}
