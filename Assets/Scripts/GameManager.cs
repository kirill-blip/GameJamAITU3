using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameJam
{
    public class GameManager : MonoBehaviour
    {
        public WaveData CurrentWaveData;

        [SerializeField] private List<WaveData> WaveData;
        private int _currentIndex = 0;

        private float _waveTime = 30;
        private float _currentWaveTime = 0;

        private bool _isGameOver = false;
        private bool _isGameLose = false;
        private bool _isLastLevel = false;

        private UserInterface _userInterface;
        private WaveManager _waveManager;
        private EndPoint _endPoint;

        public event EventHandler OnTimeChanged;
        public event EventHandler OnGameOver;
        public event EventHandler OnGameLose;
        public event EventHandler OnLastWavePlayed;

        private void Start()
        {
            _userInterface = FindObjectOfType<UserInterface>();
            _waveManager = FindObjectOfType<WaveManager>();
            _endPoint = FindObjectOfType<EndPoint>();

            _endPoint.SnowmanCollided += SnowmanCollided;
            _userInterface.OnNextLevelButtonClicked += OnNextLevelButtonClicked;

            CurrentWaveData = WaveData[_currentIndex];

            _waveTime = CurrentWaveData.WaveTime;

            _waveManager.StartNextWave(CurrentWaveData);
        }

        private void SnowmanCollided(object sender, EventArgs e)
        {
            OnGameLose?.Invoke(this, null);
            _isGameLose = true;

            var snowmans = FindObjectsOfType<Snowman>().ToList();
            snowmans.ForEach(x => Destroy(x.gameObject));
        }

        private void OnNextLevelButtonClicked(object sender, EventArgs e)
        {
            _isGameOver = false;

            _currentIndex++;

            if (_currentIndex < WaveData.Count)
            {
                CurrentWaveData = WaveData[_currentIndex];
            }
            else
            {
                _isLastLevel = true;
            }

            _currentWaveTime = 0;
            _waveTime = CurrentWaveData.WaveTime;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _waveManager.StartNextWave(CurrentWaveData);
        }

        private void Update()
        {
            if (_isGameLose)
            {
                return;
            }

            if (!_isGameOver && _waveTime >= _currentWaveTime)
            {
                _currentWaveTime += Time.deltaTime;
                OnTimeChanged?.Invoke(this, null);
            }
            else if (!_isGameOver)
            {
                _isGameOver = true;

                if (_isLastLevel)
                {
                    OnLastWavePlayed?.Invoke(this, null);
                }
                else
                {
                    OnGameOver?.Invoke(this, null);
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public float GetCurrentWaveTime()
        {
            return _currentWaveTime;
        }
    }
}
