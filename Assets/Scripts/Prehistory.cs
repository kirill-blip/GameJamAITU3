using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class Prehistory : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private Button _menuButton;

        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private float _timeForOneLetter = 0.1f;
        [SerializeField] private string _history;

        private bool _isAfterWaves = false;

        public event EventHandler OnTextPrinted;

        public void Init(string text, bool isAfterWaves)
        {
            _history = text;
            _isAfterWaves = isAfterWaves;
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(() =>
            {
                OnTextPrinted?.Invoke(this, null);
                gameObject.SetActive(false);
            });

            _skipButton.onClick.AddListener(Skip);

            StartWriting();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StopWriting();
            }
        }

        private void Skip()
        {
            StopWriting();
        }

        public void StopWriting()
        {
            StopAllCoroutines();

            _text.text = "";
            _text.text = _text.text.Insert(_text.text.Length, _history);
            _skipButton.gameObject.SetActive(false);

            if (_isAfterWaves)
            {
                _menuButton.gameObject.SetActive(true);
            }
            else
            {
                _closeButton.gameObject.SetActive(true);
            }
        }

        public void StartWriting()
        {
            StartCoroutine(StartWritingCorouting(_history));
        }

        private IEnumerator StartWritingCorouting(string text)
        {
            ResetValues();

            for (int i = 0; i < text.Length; i++)
            {
                _text.text += text[i];
                yield return new WaitForSeconds(_timeForOneLetter);
            }
        }

        private void ResetValues()
        {
            _menuButton.gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
            _skipButton.gameObject.SetActive(true);
            _text.text = "";
        }
    }
}
