using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace GameJam
{
    public class Snowman : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _health = 100f;

        [Space(10)]
        [SerializeField] private Transform _presentPosition;
        [SerializeField] private ParticleSystem _explosion;

        [Space(10)]
        [SerializeField] private GameObject _hands;
        [SerializeField] private GameObject _handsWithPresent;
        [SerializeField] private float _presentSize;

        [Space(10)]
        [SerializeField] private float _timeToWait = 2;

        [SerializeField] private Present _present;

        private NavMeshAgent _navMeshAgent;

        public event EventHandler<Snowman> SnowmanKilled;

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

        public void Init(SnowmanData data)
        {
            _health = data.Health;
            _navMeshAgent.speed = data.Speed;
        }

        private IEnumerator OnSnowmanHere()
        {
            yield return new WaitForSeconds(_timeToWait / 2f);
            _handsWithPresent.SetActive(true);
            _hands.SetActive(false);

            _present.transform.parent = _presentPosition;
            _present.transform.localPosition = Vector3.zero;
            _present.transform.localScale = new Vector3(_presentSize, _presentSize, _presentSize);
            _present.transform.rotation = Quaternion.identity;

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
                var explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
                explosion.Play();

                SnowmanKilled?.Invoke(this, this);
            }
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
            _present = present;
            _present.IsBusy = true;
            _navMeshAgent.SetDestination(_present.transform.position);
        }

        public bool HaveNotPresent()
        {
            return _present == null;
        }
    }
}
