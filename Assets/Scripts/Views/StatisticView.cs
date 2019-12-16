using UnityEngine;
using UnityEngine.UI;

public class StatisticView : View
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private Text _healthText;
    [SerializeField] private Text _ScoreText;
    [SerializeField] private Level _level;

#pragma warning restore 0649
    #endregion

    private void OnEnable()
    {
        _level.OnChangeHealth += OnChangeHealth;
        _level.OnChangeScore += OnChangeScore;

        OnChangeScore(_level.Score);
        OnChangeHealth(_level.Health);
    }

    private void OnDisable()
    {
        _level.OnChangeHealth -= OnChangeHealth;
        _level.OnChangeScore -= OnChangeScore;

    }

    private void OnChangeScore(int value) => _ScoreText.text = "Score: " + value;
    private void OnChangeHealth(int value) => _healthText.text = "Health: " + value;
}
