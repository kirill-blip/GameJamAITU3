using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJam
{
    public class NextWavePanel : MonoBehaviour
    {
        [SerializeField] private Button _nextWaveButton;
        [SerializeField] private Button _menuButton;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            _nextWaveButton.onClick.AddListener(() => _gameManager.StartNextLevel());
            _menuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }
    }
}
