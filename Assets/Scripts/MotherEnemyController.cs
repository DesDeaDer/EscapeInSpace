using System;
using UnityEngine;

public class MotherEnemyController : MonoBehaviour
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private MotherEnemy _motherEnemy;
    [SerializeField] private PoolBullets _poolBullets;

#pragma warning restore 0649
    #endregion

    public event Action<MotherEnemyController> OnEnd;
    public event Action<MotherEnemyController> OnDead;

    private bool _isMove;
    private float _shootRechargeDelay;
    private Vector3 _positionTarget;

    public int ScoreCost => _motherEnemy.CostScore;

    public void MoveTo(Vector3 from, Vector3 to)
    {
        _isMove = true;
        _motherEnemy.Position = from;
        _positionTarget = to;
    }

    protected bool IsCanShoot => _shootRechargeDelay == 0;

    private void OnEnable()
    {
        _shootRechargeDelay = _motherEnemy.ShootRechargeStartTime;
    }

    private void Update()
    {
        ShootRechargeProcessing();
        if (ShootProcessing())
        {
            ShootRecharge();
        }

        if (_isMove)
        {
            if (MoveProcessing())
            {
                End();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Dead();
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

    private void ShootRecharge() => _shootRechargeDelay = _motherEnemy.ShootRechargeTime;

    private void Shoot() => _poolBullets.Get().Shoot(_motherEnemy.PivotBulletPosition, _motherEnemy.PivotBulletDirection, _motherEnemy.SpeedBulletMove);

    private void End()
    {
        _isMove = false;

        OnEnd?.Invoke(this);
    }

    private void Dead()
    {
        OnDead?.Invoke(this);
    }

    private bool MoveProcessing()
    {
        var position = _motherEnemy.Position;

        var distanceVector = _positionTarget - position;
        var speed = distanceVector.normalized * (_motherEnemy.SpeedMove * Time.deltaTime);

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

        _motherEnemy.Position = position;

        if (position == _positionTarget)
        {
            return true;
        }

        return false;
    }
}
