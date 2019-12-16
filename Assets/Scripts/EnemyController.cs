using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private Enemy _enemy;
    [SerializeField] private PoolBullets _poolBullets;

#pragma warning restore 0649
    #endregion

    private bool _isMove;
    private bool _isShooting;
    private float _speed;
    private Vector3 _positionTarget;
    private Vector3 _positionStart;

    public event Action OnEndMove;
    public event Action<EnemyController> OnDead;

    public int ScoreCost => _isShooting ? _enemy.CostShootScore : _enemy.CostScore;

    public Vector3 Position => _enemy.Position;

    public void SetSpeed(float value) => _speed = value;

    public void MoveTo(Vector3 to, float speed)
    {
        _positionTarget = to;
        _speed = speed;
        _isMove = true;
    }

    public void Shoot(float speed)
    {
        _poolBullets.Get().Shoot(_enemy.PivotBulletPosition, _enemy.PivotBulletDirection, speed);
        _isShooting = true;
    }

    public void StopShoot() => _isShooting = false;

    public void RestartPosition()
    {
        _enemy.Position = _positionStart;
        _positionTarget = _positionStart;
        _isMove = false;
    }

    private void Start()
    {
        _positionStart = _enemy.Position;
    }

    private void Update()
    {
        if (_isMove)
        {
            if (MoveProcessing())
            {
                EndMove();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Dead();
    }

    private void EndMove()
    {
        _isMove = false;
        OnEndMove?.Invoke();
    }

    private void Dead()
    {
        OnDead?.Invoke(this);
    }

    private bool MoveProcessing()
    {
        var position = _enemy.Position;

        var distanceVector = _positionTarget - position;
        var speed = distanceVector.normalized * (_speed * Time.deltaTime);

        var distanceSqr = distanceVector.sqrMagnitude;
        var speedSqr = speed.sqrMagnitude;

        if (speedSqr > distanceSqr)
        {
            position = _positionTarget;
        }
        else
        {
            position += speed;
        }

        _enemy.Position = position;

        if (position == _positionTarget)
        {
            return true;
        }

        return false;
    }

}
