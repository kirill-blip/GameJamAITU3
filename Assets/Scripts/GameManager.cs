using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameJam
{
    public class GameManager : MonoBehaviour
    {
        public WaveData CurrentWaveData;

        [SerializeField] private List<WaveData> _waveData;
        [SerializeField] private List<BackgroundSnowman> _backgroundSnowmen;
        [SerializeField] private Prehistory _prehistory;

        private float _waveTime = 30;
        private float _currentWaveTime = 0;

        private bool _isGameOver = false;
        private bool _isGameLose = false;
        private bool _isLastLevel = false;
        private bool _isGameStarted = false;

        private UserInterface _userInterface;
        private WaveManager _waveManager;
        private EndPoint _endPoint;
        private CannonController _cannonController;

        public event EventHandler OnTimeChanged;
        public event EventHandler OnGameOver;
        public event EventHandler OnGameLose;
        public event EventHandler OnLastWavePlayed;

        private void Start()
        {
            _userInterface = FindObjectOfType<UserInterface>();
            _waveManager = FindObjectOfType<WaveManager>();
            _endPoint = FindObjectOfType<EndPoint>();
            _cannonController = FindObjectOfType<CannonController>();

            _cannonController.StopMovement();

            _waveManager.OnPresentsOvered += StopGame;

            _prehistory.OnTextPrinted += OnTextPrinted;
            _endPoint.SnowmanCollided += StopGame;
            _userInterface.NextWavePanel.OnNextWaveButtonClicked += OnNextLevelButtonClicked;
        }

        private void Update()
        {
            if (_isGameLose || !_isGameStarted)
            {
                return;
            }

            if (!_isGameOver && _waveTime >= _currentWaveTime)
            {
                _currentWaveTime += Time.deltaTime;
                OnTimeChanged?.Invoke(this, null);
            }
            else if (!_isGameOver && !_waveManager.IsThereSnowmenOnMap())
            {
                _isGameOver = true;

                if (!_waveManager.IsTherePresentsOnMap())
                {
                    OnGameLose?.Invoke(this, null);
                }
                else if (_isLastLevel)
                {
                    OnLastWavePlayed?.Invoke(this, null);
                    _prehistory.OnTextPrinted -= OnTextPrinted;
                }
                else
                {
                    OnGameOver?.Invoke(this, null);
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void OnTextPrinted(object sender, EventArgs e)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _isGameStarted = true;

            _cannonController.StartMovement();

            CurrentWaveData = _waveData[0];

            _waveTime = CurrentWaveData.WaveTime;

            foreach (var item in _backgroundSnowmen)
            {
                item.Move();
            }

            _waveManager.StartNextWave(CurrentWaveData);
        }

        private void OnNextLevelButtonClicked(object sender, EventArgs e)
        {
            _isGameOver = false;

            foreach (var item in _backgroundSnowmen)
            {
                item.Move();
            }

            _waveData.Remove(CurrentWaveData);

            if (_waveData.Count == 1)
            {
                _isLastLevel = true;
            }

            CurrentWaveData = _waveData[0];

            _currentWaveTime = 0;
            _waveTime = CurrentWaveData.WaveTime;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _cannonController.StartMovement();
            _waveManager.StartNextWave(CurrentWaveData);
        }

        private void StopGame(object sender, EventArgs e)
        {
            OnGameLose?.Invoke(this, null);
            _isGameLose = true;

            var snowmans = FindObjectsOfType<Snowman>().ToList();
            snowmans.ForEach(x => Destroy(x.gameObject));

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public float GetCurrentWaveTime()
        {
            return _currentWaveTime;
        }
    }
}
