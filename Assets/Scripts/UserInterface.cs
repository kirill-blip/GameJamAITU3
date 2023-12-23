using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJam
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private NextWavePanel _nextWavePanel;
        [SerializeField] private RestartWavePanel _restartWavePanel;
        [SerializeField] private GameObject _endPanel;

        [SerializeField] private Button _menuButton;
        [SerializeField] private Slider _slider;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            _gameManager.TimeChanged += OnTimeChanged;
            _gameManager.GameOvered += OnGameOver;
            _gameManager.GameLosed += OnGameLose;
            _gameManager.LastWavePlayed += OnLastWavePlayed;
            _gameManager.NextLevelStarted += GameManager_NextLevelStarted;
            _gameManager.WaveRestarted += GameManager_WaveRestarted;

            _menuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }

        private void InitSlider()
        {
            _slider.value = 0;
            _slider.gameObject.SetActive(true);
        }

        private void GameManager_WaveRestarted(object sender, EventArgs e)
        {
            InitSlider();
            _restartWavePanel.gameObject.SetActive(false);
        }

        private void GameManager_NextLevelStarted(object sender, EventArgs e)
        {
            InitSlider();
            _nextWavePanel.gameObject.SetActive(false);
        }

        private void OnLastWavePlayed(object sender, EventArgs e)
        {
            _slider.value = 0;
            _endPanel.SetActive(true);
        }

        private void OnGameLose(object sender, EventArgs e)
        {
            _slider.gameObject.SetActive(false);
            _restartWavePanel.gameObject.SetActive(true);
        }

        private void OnGameOver(object sender, EventArgs e)
        {
            _nextWavePanel.gameObject.SetActive(true);
        }

        private void OnTimeChanged(object sender, EventArgs e)
        {
            _slider.value = _gameManager.GetCurrentWaveTime() / _gameManager.CurrentWaveData.WaveTime;
        }
    }
}
