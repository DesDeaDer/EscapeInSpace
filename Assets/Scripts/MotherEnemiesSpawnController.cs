using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class MotherEnemiesSpawnController : MonoBehaviour
{

	[SerializeField] private PoolMotherEnemies _poolMotherEneies;
	[SerializeField] private CameraInfo _cameraInfo;
	[SerializeField] private float _offsetBorder;
	[SerializeField] private float _spawnTime;
	[SerializeField] private float _spawnChance;

	private List<MotherEnemyController> _motherEnemies = new List<MotherEnemyController>();
	private float _spawnDelay;

	public event Action OnAllDead;
	public event Action<MotherEnemyController> OnDead;

	public int Count
	{
		get
		{
			return _motherEnemies.Count;
		}
	}

	public void Reset()
	{
		foreach (var item in _motherEnemies)
		{
			item.OnEnd -= OnEndHandler;
			item.OnDead -= OnEndHandler;
			item.OnDead -= Dead;
			_poolMotherEneies.Set(item);
		}

		_motherEnemies.Clear();
	}

	protected bool IsCanSpawn
	{
		get
		{
			return _spawnDelay == 0;
		}
	}

	private void OnEnable()
	{
		_spawnDelay = _spawnTime;
	}

	private void Update()
	{
		SpawnTimeProcessing();
		if (IsCanSpawn)
		{
			if (GetIsCanSpawnProcessing())
			{
				Spawn();
			}
			SpawnTimeRecharge();
		}
	}

	private void Spawn()
	{
		var left = new Vector3( _cameraInfo.Left - _offsetBorder, _cameraInfo.Up - _offsetBorder);
		var rigth = new Vector3(_cameraInfo.Rigth + _offsetBorder, _cameraInfo.Up - _offsetBorder);

		Vector3 from;
		Vector3 to;
		if (Random.Range (0.0f, 1.0f) <= 0.5f)
		{
			from = left;
			to = rigth;
		}
		else
		{
			from = rigth;
			to = left;
		}

		var instance = _poolMotherEneies.Get();

		instance.OnEnd += OnEndHandler;
		instance.OnDead += OnEndHandler;
		instance.OnDead += Dead;

		instance.MoveTo(from, to);

		_motherEnemies.Add(instance);
	}

	private void OnEndHandler(MotherEnemyController obj)
	{
		obj.OnEnd -= OnEndHandler;
		obj.OnDead -= OnEndHandler;
		obj.OnDead -= Dead;

		_motherEnemies.Remove(obj);

		_poolMotherEneies.Set(obj);

		if (_motherEnemies.Count == 0)
		{
			AllDead();
		}
	}

	private void SpawnTimeProcessing()
	{
		if (_spawnDelay > 0)
		{
			_spawnDelay -= Time.deltaTime;
		}
		if (_spawnDelay < 0)
		{
			_spawnDelay = 0;
		}
	}

	private void SpawnTimeRecharge()
	{
		_spawnDelay = _spawnTime;
	}

	private bool GetIsCanSpawnProcessing()
	{
		return Random.Range(0.0f, 1.0f) <= _spawnChance;
	}

	private void AllDead()
	{
		if (OnAllDead != null)
		{
			OnAllDead();
		}
	}

	private void Dead(MotherEnemyController obj)
	{
		if (OnDead != null)
		{
			OnDead(obj);
		}
	}
}
