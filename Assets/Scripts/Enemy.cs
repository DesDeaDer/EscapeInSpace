using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	[SerializeField] private Transform _pivotBulletStart;
	[SerializeField] private int _costScore;
	[SerializeField] private int _costShootScore;

	public int CostScore
	{
		get
		{
			return _costScore;
		}
	}

	public int CostShootScore
	{
		get
		{
			return _costShootScore;
		}
	}

	public Vector3 PivotBulletPosition
	{
		get
		{
			return _pivotBulletStart.position;
		}
	}

	public Vector3 PivotBulletDirection
	{
		get
		{
			return _pivotBulletStart.up;
		}
	}

	public Vector3 Position 
	{
		get 
		{
			return transform.position;
		}
		set
		{
			transform.position = value;
		}
	}
}

