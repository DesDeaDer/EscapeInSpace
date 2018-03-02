using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	[SerializeField] private PoolBullets _poolBullets;
	[SerializeField] private CameraInfo _cameraInfo;
	[SerializeField] private Bullet _bullet;
	[SerializeField] private float _offsetBorder;

	private Vector3 _positionTarget;
	private float _speed;

	public void Shoot(Vector3 pos, Vector3 dir, float speed)
	{
		_bullet.Position = pos;

		//lenth of [leftdown, rigthup]
		var length = Mathf.Sqrt ((_cameraInfo.Rigth - _cameraInfo.Left) * (_cameraInfo.Rigth - _cameraInfo.Left) + (_cameraInfo.Up - _cameraInfo.Down) * (_cameraInfo.Up - _cameraInfo.Down));

		var posEnd = pos + dir * length;

		var start = Vector2.Min(pos, posEnd);
		var end = Vector2.Max(pos, posEnd);

		var leftUp = new Vector2 (_cameraInfo.Left - _offsetBorder, _cameraInfo.Up + _offsetBorder);
		var leftDown = new Vector2 (_cameraInfo.Left - _offsetBorder, _cameraInfo.Down - _offsetBorder);
		var RigthDown = new Vector2 (_cameraInfo.Rigth + _offsetBorder, _cameraInfo.Down - _offsetBorder);
		var RigthUp = new Vector2 (_cameraInfo.Rigth + _offsetBorder, _cameraInfo.Up + _offsetBorder);

		Vector2 pointIntersect;
		if
		(
				intersection(start, end, leftUp, RigthUp, out pointIntersect) ||
				intersection(start, end, leftDown, RigthDown, out pointIntersect) ||
				intersection(start, end, leftUp, leftUp, out pointIntersect) ||
				intersection(start, end, RigthUp, RigthUp, out pointIntersect)
		)
		{
			_positionTarget = pointIntersect;
			_speed = speed;
		}

	}

	private bool intersection(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out Vector2 result)
    {
        var dir1 = end1 - start1;
        var dir2 = end2 - start2;

        var a1 = -dir1.y;
        var b1 = +dir1.x;
        var d1 = -(a1 * start1.x + b1 * start1.y);

        var a2 = -dir2.y;
        var b2 = +dir2.x;
        var d2 = -(a2 * start2.x + b2 * start2.y);

        var seg1_line2_start = a2 * start1.x + b2 * start1.y + d2;
        var seg1_line2_end = a2 * end1.x + b2 * end1.y + d2;

        var seg2_line1_start = a1 * start2.x + b1 * start2.y + d1;
        var seg2_line1_end = a1 * end2.x + b1 * end2.y + d1;

		if (seg1_line2_start * seg1_line2_end >= 0 || seg2_line1_start * seg2_line1_end >= 0)
		{
			result = Vector2.zero;
			return false;
		}

        var u = seg1_line2_start / (seg1_line2_start - seg1_line2_end);
		result =  start1 + u * dir1;

        return true;
    }

	private void Update()
	{
		MoveProcessing();
	}

	private void MoveProcessing()
	{
		var position = _bullet.Position;

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

		_bullet.Position = position;

		if (position == _positionTarget) 
		{
			_poolBullets.Set(this);
		}
	}

	private void OnCollisionEnter2D(Collision2D coll) 
	{
		_poolBullets.Set(this);
	}
}
