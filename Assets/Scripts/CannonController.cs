using UnityEngine;

namespace GameJam
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private float _speedRotation;
        [SerializeField] private float _maxRotationAngle = 90;
        [SerializeField] private AudioClip _sound;

        [SerializeField] private GameObject _core;
        [SerializeField] private Transform _corePosition;
        [SerializeField] private float _timeBetweenShooting = 1;

        private AudioSource _audioSource;

        private Rigidbody _rigidbody;
        private GameManager _gameManager;
        private float _currentRotation = 0f;
        private float _currentTime;
        private bool _gameOver = false;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
            _gameManager = FindObjectOfType<GameManager>();

            _gameManager.OnGameOver += (sender, e) => StopMovement();
            _gameManager.OnGameLose += (sender, e) => StopMovement();
            _gameManager.OnLastWavePlayed += (sender, e) => StopMovement();
        }

        private void Update()
        {
            if (_gameOver)
            {
                return;
            }
            _currentTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = !Cursor.visible;
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            }

            if (Input.GetMouseButton(0) && _currentTime >= _timeBetweenShooting)
            {
                Shoot();
                _currentTime = 0;
            }

            Rotate();
        }

        private void Rotate()
        {
            float rotationInput = Input.GetAxis("Horizontal");

            float rotationAmount = rotationInput * _speedRotation * Time.deltaTime;

            _currentRotation = Mathf.Clamp(_currentRotation + rotationAmount, -_maxRotationAngle, _maxRotationAngle);

            Quaternion rotation = Quaternion.Euler(-90f, _currentRotation, 0f);

            _rigidbody.MoveRotation(rotation);
        }

        private void Shoot()
        {
            _audioSource.PlayOneShot(_sound);
            Instantiate(_core, _corePosition.position, _corePosition.rotation);
        }

        public void StartMovement()
        {
            _gameOver = false;
        }

        public void StopMovement()
        {
            _gameOver = true;
        }
    }
}
