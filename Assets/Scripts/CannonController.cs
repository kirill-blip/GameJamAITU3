using UnityEngine;

namespace GameJam
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private float _speedRotation;
        [SerializeField] private float _maxRotationAngle = 90;

        [SerializeField] private GameObject _core;
        [SerializeField] private Transform _corePosition;

        private Rigidbody _rigidbody;
        private float _currentRotation = 0f;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = !Cursor.visible;
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
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
            Instantiate(_core, _corePosition.position, _corePosition.rotation);
        }
    }
}
