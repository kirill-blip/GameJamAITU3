using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJam
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private Button _nextWaveButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Image _image;

        private GameManager _gameManager;

        public event EventHandler OnNextLevelButtonClicked;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.OnTimeChanged += OnTimeChanged;
            _gameManager.OnGameOver += OnGameOver;
            _gameManager.OnGameLose += OnGameLose;
            _gameManager.OnLastWavePlayed += OnLastWavePlayed; ;

            _nextWaveButton.onClick.AddListener(() => 
            {
                _image.fillAmount = 0;
                _nextWaveButton.gameObject.SetActive(false);
                OnNextLevelButtonClicked?.Invoke(this, null); 
            });

            _restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
            _menuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }

        private void OnLastWavePlayed(object sender, EventArgs e)
        {
            _image.fillAmount = 0;
            _menuButton.gameObject.SetActive(true);
        }

        private void OnGameLose(object sender, EventArgs e)
        {
            _image.gameObject.SetActive(false);
            _restartButton.gameObject.SetActive(true);
        }

        private void OnGameOver(object sender, EventArgs e)
        {
            _nextWaveButton.gameObject.SetActive(true);
        }

        private void OnTimeChanged(object sender, EventArgs e)
        {
            _image.fillAmount = _gameManager.GetCurrentWaveTime() / _gameManager.CurrentWaveData.WaveTime;
        }
    }
}
