using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJam
{
    public class RestartWavePanel : MonoBehaviour
    {
        [SerializeField] private Button _restartWaveButton;
        [SerializeField] private Button _menuButton;

        private void Start()
        {
            _restartWaveButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
            _menuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }
    }
}
