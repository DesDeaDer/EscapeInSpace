using UnityEngine;

public class Ship : MonoBehaviour
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private Transform _pivotBulletStart;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedBullet;
    [SerializeField] private float _shootRechargeTime;

#pragma warning restore 0649
    #endregion

    public float Speed => _speed;
    public float SpeedBullet => _speedBullet;
    public float ShootRechargeTime => _shootRechargeTime;
    public Vector3 PivotBulletPosition => _pivotBulletStart.position;
    public Vector3 PivotBulletDirection => _pivotBulletStart.up;

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
