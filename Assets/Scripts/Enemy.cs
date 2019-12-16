using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private Transform _pivotBulletStart;
    [SerializeField] private int _costScore;
    [SerializeField] private int _costShootScore;

#pragma warning restore 0649
    #endregion

    public int CostScore => _costScore;
    public int CostShootScore => _costShootScore;
    public Vector3 PivotBulletPosition => _pivotBulletStart.position;
    public Vector3 PivotBulletDirection => _pivotBulletStart.up;

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
