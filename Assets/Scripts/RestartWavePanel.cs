using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJam
{
    public class RestartWavePanel : MonoBehaviour
    {
        [SerializeField] private Button _restartWaveButton;
        [SerializeField] private Button _menuButton;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            _restartWaveButton.onClick.AddListener(() =>
            {
                _gameManager.RestartWave();
            });

            _menuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }
    }
}
