using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MotherEnemiesSpawnController : MonoBehaviour
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private PoolMotherEnemies _poolMotherEneies;
    [SerializeField] private CameraInfo _cameraInfo;
    [SerializeField] private float _offsetBorder;
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _spawnChance;

#pragma warning restore 0649
    #endregion

    private List<MotherEnemyController> _motherEnemies = new List<MotherEnemyController>();
    private float _spawnDelay;

    public event Action OnAllDead;
    public event Action<MotherEnemyController> OnDead;

    public int Count => _motherEnemies.Count;

    protected bool IsCanSpawn => _spawnDelay == 0;

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
        var left = new Vector3(_cameraInfo.Left - _offsetBorder, _cameraInfo.Up - _offsetBorder);
        var rigth = new Vector3(_cameraInfo.Rigth + _offsetBorder, _cameraInfo.Up - _offsetBorder);
        var (from, to) = RandomizeDirection(left, rigth);
        var instance = _poolMotherEneies.Get();

        instance.OnEnd += OnEndHandler;
        instance.OnDead += OnEndHandler;
        instance.OnDead += Dead;

        instance.MoveTo(from, to);

        _motherEnemies.Add(instance);
    }

    private static (Vector3 from, Vector3 to) RandomizeDirection(Vector3 left, Vector3 rigth) => Random.Range(0.0f, 1.0f) <= 0.5f ? (left, rigth) : (rigth, left);

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

    private void SpawnTimeRecharge() => _spawnDelay = _spawnTime;
    private bool GetIsCanSpawnProcessing() => Random.Range(0.0f, 1.0f) <= _spawnChance;
    private void AllDead() => OnAllDead?.Invoke();
    private void Dead(MotherEnemyController obj) => OnDead?.Invoke(obj);
}
