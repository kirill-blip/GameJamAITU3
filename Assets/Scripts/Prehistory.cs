using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class Prehistory : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        public event EventHandler OnStartGameButtonClicked;

        private void Start()
        {
            _startButton.onClick.AddListener(() =>
            {
                OnStartGameButtonClicked?.Invoke(this, null);
                gameObject.SetActive(false);
            });
        }
    }
}
