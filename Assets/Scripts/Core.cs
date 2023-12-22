using UnityEngine;

namespace GameJam
{
    public class Core : MonoBehaviour
    {
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _timeToDestroy = 10f;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody= GetComponent<Rigidbody>();
            _rigidbody.AddRelativeForce(Vector3.up * _speed, ForceMode.Impulse);

            Destroy(gameObject, _timeToDestroy);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var damageables = collision.transform.GetComponents<IDamageable>();

            foreach (var item in damageables)
            {
                item.Damage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
