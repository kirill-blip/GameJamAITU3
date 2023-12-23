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

        [SerializeField] private Button _menuButton;
        [SerializeField] private Slider _slider;

        [SerializeField] private Prehistory _history;

        private GameManager _gameManager;

        public NextWavePanel NextWavePanel => _nextWavePanel;
        public RestartWavePanel RestartWavePanel => _restartWavePanel;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            _history.gameObject.SetActive(true);

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

            string text = "Силы зла оказались сильными, но несмотря на трудности, мастерам игрушек удалось восстановить порядок на фабрике Санты. Сплоченные вместе, они противостояли коварным снеговикам и смогли использовать свою мастерскую, чтобы создать волшебную пушку, стреляющую сверкающими елками. Эти магические снаряды стали оружием против снеговиков, и каждая из них была пропитана благословением добра и радости.";
            _history.gameObject.SetActive(true);
            _history.Init(text, true);
            _history.OnTextPrinted += OnTextPrinted;
            _history.StartWriting();
        }

        private void OnTextPrinted(object sender, EventArgs e)
        {
            _menuButton.gameObject.SetActive(true);
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
