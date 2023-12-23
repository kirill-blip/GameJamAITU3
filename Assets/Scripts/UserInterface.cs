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

        public NextWavePanel NextWavePanel => _nextWavePanel;
        public RestartWavePanel RestartWavePanel => _restartWavePanel;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            _gameManager.OnTimeChanged += OnTimeChanged;
            _gameManager.OnGameOver += OnGameOver;
            _gameManager.OnGameLose += OnGameLose;
            _gameManager.OnLastWavePlayed += OnLastWavePlayed;

            _nextWavePanel.OnNextWaveButtonClicked += OnNextWaveButtonClicked;

            _menuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }

        private void OnNextWaveButtonClicked(object sender, EventArgs e)
        {
            _slider.value = 0;
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
