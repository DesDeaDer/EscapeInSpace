using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemiesGroupController : MonoBehaviour
{
	[SerializeField] private CameraInfo _cameraInfo;
	[SerializeField] private PoolEnemies _poolEnemies;
	[SerializeField] private float _sizeCell;
	[SerializeField] private float _offsetCell;
	[SerializeField] private float _speed;
	[SerializeField] private float _speedIncrease;
	[SerializeField] private float _shootRechargeStartTime;
	[SerializeField] private float _shootRechargeTime;
	[SerializeField] private float _speedBulletMove;

	public event Action OnAllDead;
	public event Action<EnemyController> OnDead;

	private enum MoveDirectionID
	{
		None = 0,
		Left = 1,
		Rigth = 2,
		Down = 3,
	}

	private bool _isUpdateMove;
	private int _indexDirection;
	private int _count;
	private float _speedCurrent;
	private float _shootRechargeDelay;
	private EnemyController[][] _enemies;

	private MoveDirectionID[] _pathFromDirections =
	{ 
		MoveDirectionID.Rigth, 
		MoveDirectionID.Down,
		MoveDirectionID.Left,
		MoveDirectionID.Down 
	};

	private IDictionary<MoveDirectionID, Vector3> _directions = new Dictionary<MoveDirectionID, Vector3>()
	{
		{ MoveDirectionID.None, Vector3.zero },
		{ MoveDirectionID.Left, Vector3.left },
		{ MoveDirectionID.Rigth, Vector3.right },
		{ MoveDirectionID.Down, Vector3.down }
	};

	public bool IsCanShoot
	{
		get
		{
			return _shootRechargeDelay == 0;
		}
	}

	public void Reset()
	{
		_poolEnemies.GetAll();

		foreach (var line in _enemies)
		{
			foreach (var item in line)
			{
				item.RestartPosition();
			}	
		}
	}

	private void OnEnable()
	{ 
		Init();
		SubscribeAll();
	}

	private void OnDisable()
	{
		UnsubscribeAll();
	}

	private void Update()
	{
		ShootRechargeProcessing();
		if (ShootProcessing()) 
		{
			ShootRecharge();
		}

		MoveProcessing();
	}

	private void Init()
	{
		int count;
		_enemies = GetEnemiesInGrid(out count);

		_indexDirection = 0;

		_count = count;

		_speedCurrent = _speed;

		_shootRechargeDelay = _shootRechargeStartTime;

		_isUpdateMove = true;
	}

	private void ShootRechargeProcessing()
	{
		if (_shootRechargeDelay > 0) 
		{
			_shootRechargeDelay -= Time.deltaTime;
		}
		if (_shootRechargeDelay < 0) 
		{
			_shootRechargeDelay = 0;
		}
	}

	private bool ShootProcessing()
	{
		if (IsCanShoot) 
		{
			Shoot();
			return true;
		}

		return false;
	}

	private void ShootRecharge()
	{
		_shootRechargeDelay = _shootRechargeTime;
	}

	private void Shoot()
	{
		foreach (var line in _enemies)
		{
			foreach (var item in line)
			{
				item.StopShoot();
			}
		}

		ShootLeftRirthFromLine(1);
		ShootLeftRirthFromLine(3);
	}

	private void ShootLeftRirthFromLine(int indexLine)
	{
		if (_enemies.Length > indexLine) 
		{
			var line = _enemies[indexLine];
			var len = line.Length;
			var first = (EnemyController)null;
			var last = (EnemyController)null;
			if (len > 0)
			{
				first = line.FirstOrDefault(x => x.gameObject.activeInHierarchy);
			}
			if (len > 1)
			{
				last = line.LastOrDefault(x => x.gameObject.activeInHierarchy);
			}

			if (first != null)
			{
				first.Shoot(_speedBulletMove);
				if (last != null && last != first) 
				{
					last.Shoot(_speedBulletMove);
				}
			}
		}
	}

	private void MoveProcessing()
	{
		if (_isUpdateMove) 
		{
			_isUpdateMove = false;

			if (!IsCanMove() || GetCurrentMoveDirectionID() == MoveDirectionID.Down) 
			{
				SetNextDirection();
			}

			MoveEnemies();
		}
	}

	private void IncreaseSpeed()
	{
		_speedCurrent += _speedIncrease;

		foreach (var line in _enemies)
		{
			foreach (var item in line) 
			{
				item.Speed = _speedCurrent;
			}
		}
	}

	private void MoveEnemies()
	{
		var dir = GetCurrentDirection();

		foreach (var line in _enemies) 
		{
			foreach (var item in line) 
			{
				if (item.isActiveAndEnabled)
				{
					item.MoveTo(item.Position + dir * _sizeCell, _speedCurrent);
				}
			}
		}
	}

	private bool IsCanMove()
	{
		var dir = GetCurrentDirection();

		var left = _cameraInfo.Left + _offsetCell;
		var rigth = _cameraInfo.Rigth - _offsetCell;

		Vector3 pos;
		foreach (var line in _enemies) 
		{
			foreach (var item in line) 
			{
				pos = item.Position + dir * _sizeCell;
				if (pos.x < left || pos.x > rigth)
				{
					return false;
				}
			}
		}

		return true;
	}

	private void SetNextDirection()
	{
		++_indexDirection;

		if (_indexDirection >= _pathFromDirections.Length)
		{
			_indexDirection = 0;
		}
	}

	private Vector3 GetCurrentDirection()
	{
		return GetDirection(GetCurrentMoveDirectionID());
	}

	private MoveDirectionID GetCurrentMoveDirectionID()
	{
		return GetMoveDirectionID(_indexDirection);
	}

	private MoveDirectionID GetMoveDirectionID(int index)
	{
		return _pathFromDirections [index];
	}

	private Vector3 GetDirection(MoveDirectionID dir)
	{
		return _directions[dir];
	}

	private void SubscribeAll()
	{
		foreach (var line in _enemies) 
		{
			foreach (var item in line) 
			{
				if (item)
				{
					item.OnEndMove += OnEndMoveHandler;
					item.OnDead += OnDeadHandler;
				}
			}
		}
	}

	private void UnsubscribeAll()
	{
		foreach (var line in _enemies) 
		{
			foreach (var item in line) 
			{
				if (item) 
				{
					item.OnEndMove -= OnEndMoveHandler;
					item.OnDead -= OnDeadHandler;
				}
			}
		}
	}

	private void OnDeadHandler(EnemyController obj)
	{
		obj.OnEndMove -= OnEndMoveHandler;
		obj.OnDead -= OnDeadHandler;

		_poolEnemies.Set(obj);
		DecreaseCount();
		IncreaseSpeed();

		Dead(obj);

		if (_count == 0)
		{
			AllDead();
		}
	}

	private void OnEndMoveHandler()
	{
		_isUpdateMove = true;
	}

	private EnemyController[][] GetEnemiesInGrid(out int resultCount)
	{
		var all = GetComponentsInChildren<EnemyController>();

		resultCount = all.Length;

		if (all.Length == 0)
		{
			return new EnemyController[][] { };
		}

		Array.Sort(all, ComparerByPosition);

		var last = all[0].Position;
		var current = last;

		var indexesLine = new List<int>();

		for (int i = 1; i < all.Length; i++) 
		{
			current = all[i].Position;
			if (!EqualLines(last, current, _sizeCell)) 
			{
				indexesLine.Add(i);
			}
			last = current;
		}
		indexesLine.Add(all.Length);

		if (indexesLine.Count == 1)
		{
			return new EnemyController[][] { all };
		}

		var result = new EnemyController[indexesLine.Count][];

		var indexLast = 0;
		var indexLine = 0;
		foreach (var index in indexesLine)
		{
			result[indexLine] = new EnemyController[index - indexLast];

			for (int i = 0; indexLast < index; i++, indexLast++) 
			{
				result[indexLine][i] = all[indexLast];
			}

			++indexLine;
		}

		return result;
	}

	private int ComparerByPosition(EnemyController first, EnemyController second)
	{
		var firstPosition = first.Position;
		var secondPosition = second.Position;

		firstPosition.y = ((int)(firstPosition.y / _sizeCell)) * _sizeCell;
		secondPosition.y = ((int)(secondPosition.y / _sizeCell)) * _sizeCell;

		if (firstPosition.x > secondPosition.x || firstPosition.y > secondPosition.y)
		{
			return -1;
		}
		if (firstPosition.x < secondPosition.x || firstPosition.y < secondPosition.y)
		{
			return 1;
		}

		return 0;
	}

	private bool EqualLines(Vector3 first, Vector3 second, float size)
	{
		var firstY = first.y - first.y % size;
		var secondY = second.y - second.y % size;

		return firstY == secondY;
	}

	private void DecreaseCount()
	{
		--_count;
	}

	private void AllDead()
	{
		if (OnAllDead != null)
		{
			OnAllDead();
		}
	}
	private void Dead(EnemyController obj)
	{
		if (OnDead != null)
		{
			OnDead(obj);
		}
	}
}
