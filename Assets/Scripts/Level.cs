using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    #region Data
#pragma warning disable 0649

    [SerializeField] private TimerView _timerView;
    [SerializeField] private WinView _winView;
    [SerializeField] private LoseView _loseView;
    [SerializeField] private EnemiesGroupController _enemiesGroupController;
    [SerializeField] private MotherEnemiesSpawnController _motherEnemiesSpawnController;
    [SerializeField] private ShipController _shipController;
    [SerializeField] private BulletsReseter _reseterBulletsShip;
    [SerializeField] private BulletsReseter _reseterBulletsEnemy;
    [SerializeField] private int _health;

#pragma warning restore 0649
    #endregion

    private int _healthCurrent;
    private int _score;

    public event Action<int> OnChangeHealth;
    public event Action<int> OnChangeScore;

    public int Health
    {
        get => _healthCurrent;
        private set
        {
            _healthCurrent = value;

            OnChangeHealth?.Invoke(_healthCurrent);
        }
    }

    public int Score
    {
        get => _score;
        private set
        {
            _score = value;

            OnChangeScore?.Invoke(_score);
        }
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Reload()
    {
        SceneManager.LoadScene(1);
    }

    private void OnEnable()
    {
        Health = _health;
    }

    private void Start()
    {
        DeactivateController(_enemiesGroupController);
        DeactivateController(_motherEnemiesSpawnController);
        DeactivateController(_shipController);

        ActivateController(_timerView);

        _timerView.OnEnd += OnEndTimerViewHandler;
        _timerView.StartTimer();
        _timerView.Show();
    }

    private void OnEndTimerViewHandler()
    {
        _timerView.OnEnd -= OnEndTimerViewHandler;
        _timerView.StopTimer();
        _timerView.Hide();
        StartGame();
    }

    private void StartGame()
    {
        ActivateController(_enemiesGroupController);
        ActivateController(_motherEnemiesSpawnController);
        ActivateController(_shipController);

        _enemiesGroupController.OnDead += OnDeadEnemyHandler;
        _enemiesGroupController.OnAllDead += OnAllDeadEnemiesHandler;

        _motherEnemiesSpawnController.OnDead += OnDeadMotherEnemyHandler;

        _shipController.OnDead += OnDeadShipHandler;
    }

    private void OnDeadShipHandler()
    {
        _shipController.OnDead -= OnDeadShipHandler;

        DeactivateController(_enemiesGroupController);
        DeactivateController(_motherEnemiesSpawnController);
        DeactivateController(_shipController);

        --Health;

        if (_healthCurrent > 0)
        {
            Restart();
        }
        else
        {
            Reset();
            _loseView.Show();
        }
    }

    private void OnDeadEnemyHandler(EnemyController obj)
    {
        Score += obj.ScoreCost;
    }

    private void OnDeadMotherEnemyHandler(MotherEnemyController obj)
    {
        Score += obj.ScoreCost;
    }

    private void OnAllDeadEnemiesHandler()
    {
        DeactivateController(_enemiesGroupController);
        DeactivateController(_motherEnemiesSpawnController);

        if (_motherEnemiesSpawnController.Count > 0)
        {
            _motherEnemiesSpawnController.OnAllDead += OnAllDeadMotherEnemiesHandler;
        }
        else
        {
            OnAllDeadMotherEnemiesHandler();
        }
    }

    private void OnAllDeadMotherEnemiesHandler()
    {
        DeactivateController(_enemiesGroupController);
        DeactivateController(_motherEnemiesSpawnController);
        DeactivateController(_shipController);

        _winView.Show();
    }

    private void Reset()
    {
        _enemiesGroupController.OnDead -= OnDeadEnemyHandler;
        _enemiesGroupController.OnAllDead -= OnAllDeadEnemiesHandler;
        _motherEnemiesSpawnController.OnDead -= OnDeadMotherEnemyHandler;
        _motherEnemiesSpawnController.OnAllDead -= OnAllDeadEnemiesHandler;
        _shipController.OnDead -= OnDeadShipHandler;

        _reseterBulletsShip.Reset();
        _reseterBulletsEnemy.Reset();
        _enemiesGroupController.Reset();
        _motherEnemiesSpawnController.Reset();
        _shipController.Reset();
    }

    private void Restart()
    {
        Reset();
        Start();
    }

    private void ActivateController(MonoBehaviour obj)
    {
        if (!obj.enabled)
        {
            obj.enabled = true;
        }
    }

    private void DeactivateController(MonoBehaviour obj)
    {
        if (obj.enabled)
        {
            obj.enabled = false;
        }
    }

}
