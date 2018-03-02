using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraInfo : MonoBehaviour
{

	public float Left { get; private set;}
	public float Rigth { get; private set;}
	public float Up { get; private set;}
	public float Down { get; private set;}

	private Camera _camera;
	protected Camera Camera
	{
		get 
		{
			if (!_camera) 
			{
				_camera = GetComponent<Camera>();
			}
			return _camera;
		}
	}

	private void Start()
	{
		var leftDown = Camera.ViewportToWorldPoint(Vector3.zero);
		var rightUp = Camera.ViewportToWorldPoint(Vector3.one);

		Left = leftDown.x;
		Rigth = rightUp.x;
		Up = rightUp.y;
		Down = leftDown.y;
	}
}
