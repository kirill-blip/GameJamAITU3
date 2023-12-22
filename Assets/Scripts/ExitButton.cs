using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameJam
{
    public class ExitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _door;
        [SerializeField] private float _rotateTo = -45;
        [SerializeField] private float _rotationSpeed = 1.5f;

        public void OnPointerEnter(PointerEventData eventData)
        {
            StartCoroutine(RotateDoor(_rotateTo));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StartCoroutine(RotateDoor(-_rotateTo));
        }

        private IEnumerator RotateDoor(float targetRotation)
        {
            float currentRotation = 0f;

            while (Mathf.Abs(currentRotation) < Mathf.Abs(targetRotation))
            {
                float rotationThisFrame = _rotationSpeed * Time.deltaTime * Mathf.Sign(targetRotation);

                // Обновляем текущий угол вращения
                currentRotation += rotationThisFrame;

                // Создаем кватернион для вращения вокруг оси Y
                Quaternion rotation = Quaternion.Euler(0f, rotationThisFrame, 0f);

                // Применяем вращение
                _door.transform.rotation *= rotation;

                yield return null;
            }
        }
    }
}
