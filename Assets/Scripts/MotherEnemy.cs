using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherEnemy : MonoBehaviour
{
	[SerializeField] private Transform _pivotBulletStart;
	[SerializeField] private int _costScore;
	[SerializeField] float _speedMove;
	[SerializeField] float _speedBulletMove;
	[SerializeField] float _shootRechargeTime;
	[SerializeField] float _shootRechargeStartTime;

	public int CostScore
	{
		get
		{
			return _costScore;
		}
	}

	public float SpeedMove
	{
		get
		{
			return _speedMove;
		}
	}

	public float SpeedBulletMove
	{
		get
		{
			return _speedBulletMove;
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

	public float ShootRechargeTime
	{
		get
		{
			return _shootRechargeTime;
		}
	}

	public float ShootRechargeStartTime
	{
		get
		{
			return _shootRechargeStartTime;
		}
	}

}