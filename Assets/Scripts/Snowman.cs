using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace GameJam
{
    public class Snowman : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private Transform _presentPosition;
        [SerializeField] private float _timeToWait = 2;

        [SerializeField] private Present _present;

        private NavMeshAgent _navMeshAgent;

        public event EventHandler<Snowman> SnowmanKilled;
        public event EventHandler<Snowman> SnowmanDestroyed;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Present present) && present == _present)
            {
                StartCoroutine(OnSnowmanHere());
            }
        }

        public void Init(SnowmanData data, Present present)
        {
            _health = data.Health;
            _navMeshAgent.speed = data.Speed;

            if (present is not null)
            {
                _present = present;
            }
        }

        private IEnumerator OnSnowmanHere()
        {
            _present.transform.parent = _presentPosition;
            _present.transform.localPosition = Vector3.zero;

            yield return new WaitForSeconds(_timeToWait);

            _navMeshAgent.SetDestination(GenerateTargerPoint());
        }

        private Vector3 GenerateTargerPoint()
        {
            Vector3 result;

            result = new Vector3(transform.position.x + 50, 0, transform.position.z);

            return result;
        }

        public void Damage(float damage)
        {
            if (damage < 0) throw new Exception("Damage can't be negative");

            _health -= damage;

            if (_health <= 0)
            {
                SnowmanKilled?.Invoke(this, this);
            }
        }

        public void Kill()
        {
            SnowmanDestroyed?.Invoke(this, this);
            Destroy(gameObject);
        }

        public void SetDestination(Vector3 target)
        {
            _navMeshAgent.SetDestination(target);
        }

        public Present GetPresent()
        {
            return _present;
        }

        public void SetPresent(Present present)
        {
            if (present is not null)
            {
                _present = present;
                SetDestination(_present.transform.position);
            }
        }

        public bool HaveNotPresent()
        {
            return _present == null;
        }
    }
}
