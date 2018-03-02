using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TimerView : View
{
	public const int COUNT_DIGIT = 3;
	public const float TIME_ON_DIGIT = 1.0f;

	[SerializeField] private Animator _animator;
	[SerializeField] private Text _text;

	private string Text
	{
		set
		{
			_text.text = value;
		}
	}

	public event Action OnEnd;

	private float _timeDalay;

	public void StartTimer()
	{
		_animator.enabled = true;
		_timeDalay = COUNT_DIGIT * TIME_ON_DIGIT;
	}

	public void StopTimer()
	{
		_animator.enabled = false;
	}

	private void LateUpdate()
	{
		if (_timeDalay > 0)
		{
			_timeDalay -= Time.deltaTime;
			Text = ((int)(_timeDalay / TIME_ON_DIGIT) + 1).ToString();
		}
		if (_timeDalay < 0) 
		{
			_timeDalay = 0;
			End();
		}
	}

	private void End()
	{
		if (OnEnd != null)
		{
			OnEnd();
		}
	}
}
