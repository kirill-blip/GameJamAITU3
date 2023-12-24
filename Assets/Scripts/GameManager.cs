using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        private WaveManager _waveManager;
        private CannonController _cannonController;
        private WaveCounter _waveCounter;

        public event EventHandler TimeChanged;
        public event EventHandler GameOvered;
        public event EventHandler GameLosed;
        public event EventHandler LastWavePlayed;
        public event EventHandler NextLevelStarted;
        public event EventHandler WaveRestarted;

        private void Start()
        {
            _waveManager = FindObjectOfType<WaveManager>();
            _cannonController = FindObjectOfType<CannonController>();
            _waveCounter = FindObjectOfType<WaveCounter>();

            _cannonController.StopMovement();

            _waveManager.OnPresentsOvered += StopGame;

            _prehistory.OnStartGameButtonClicked += OnGameStarted;
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
                TimeChanged?.Invoke(this, null);
            }
            else if (!_isGameOver && !_waveManager.IsThereSnowmenOnMap())
            {
                _isGameOver = true;

                if (CurrentWaveData.name.Contains('1'))
                {
                    _waveCounter.DisplayFirstNumber();
                }

                if (CurrentWaveData.name.Contains('2'))
                {
                    _waveCounter.DisplaySecondNumber();
                }

                if (CurrentWaveData.name.Contains('3'))
                {
                    _waveCounter.DisplayThirdNumber();
                }

                if (!_waveManager.IsTherePresentsOnMap())
                {
                    GameLosed?.Invoke(this, null);
                }
                else if (_isLastLevel)
                {
                    LastWavePlayed?.Invoke(this, null);
                    _prehistory.OnStartGameButtonClicked -= OnGameStarted;
                }
                else
                {
                    GameOvered?.Invoke(this, null);
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void OnGameStarted(object sender, EventArgs e)
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

        private void StopGame(object sender, EventArgs e)
        {
            GameLosed?.Invoke(this, null);
            _isGameLose = true;

            var snowmans = FindObjectsOfType<Snowman>().ToList();
            snowmans.ForEach(x => Destroy(x.gameObject));

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void StartNextLevel()
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
            NextLevelStarted?.Invoke(this, null);
        }

        public float GetCurrentWaveTime()
        {
            return _currentWaveTime;
        }

        public void RestartWave()
        {
            foreach (var item in _backgroundSnowmen)
            {
                item.Move();
            }

            _currentWaveTime = 0;
            
            _isGameLose = false;
            _isGameOver = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _cannonController.StartMovement();
            _waveManager.ResetWave();

            WaveRestarted?.Invoke(this, null);
        }
    }
}
