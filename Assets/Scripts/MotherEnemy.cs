using UnityEngine;

public class MotherEnemy : MonoBehaviour
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private Transform _pivotBulletStart;
    [SerializeField] private int _costScore;
    [SerializeField] float _speedMove;
    [SerializeField] float _speedBulletMove;
    [SerializeField] float _shootRechargeTime;
    [SerializeField] float _shootRechargeStartTime;

#pragma warning restore 0649
    #endregion

    public int CostScore => _costScore;
    public float SpeedMove => _speedMove;
    public float SpeedBulletMove => _speedBulletMove;
    public float ShootRechargeTime => _shootRechargeTime;
    public float ShootRechargeStartTime => _shootRechargeStartTime;
    public Vector3 PivotBulletPosition => _pivotBulletStart.position;
    public Vector3 PivotBulletDirection => _pivotBulletStart.up;

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
