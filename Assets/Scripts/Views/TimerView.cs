using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : View
{
    public const int COUNT_DIGIT = 3;
    public const float TIME_ON_DIGIT = 1.0f;

    #region Data
#pragma warning disable 0649

    [SerializeField] private Animator _animator;
    [SerializeField] private Text _text;

#pragma warning restore 0649
    #endregion

    private void SetText(string value) => _text.text = value;

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
            SetText(((int)(_timeDalay / TIME_ON_DIGIT) + 1).ToString());
        }
        if (_timeDalay < 0)
        {
            _timeDalay = 0;
            End();
        }
    }

    private void End()
    {
        OnEnd?.Invoke();
    }
}
