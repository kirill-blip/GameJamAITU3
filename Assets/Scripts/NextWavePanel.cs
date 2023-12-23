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

        public event EventHandler OnNextWaveButtonClicked;

        private void Start()
        {
            _nextWaveButton.onClick.AddListener(() => OnNextWaveButtonClicked?.Invoke(this, null));
            _menuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }
    }
}
