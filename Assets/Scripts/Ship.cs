using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ship : MonoBehaviour 
{
	[SerializeField] private Transform _pivotBulletStart;
	[SerializeField] private float _speed;
	[SerializeField] private float _speedBullet;
	[SerializeField] private float _shootRechargeTime;

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

	public float Speed
	{
		get 
		{
			return _speed;
		}
	}

	public float SpeedBullet
	{
		get 
		{
			return _speedBullet;
		}
	}

	public float ShootRechargeTime
	{
		get 
		{
			return _shootRechargeTime;
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
	