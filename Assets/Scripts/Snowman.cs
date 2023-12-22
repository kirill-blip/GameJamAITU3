using UnityEngine;
using UnityEngine.AI;

namespace GameJam
{
    public class Snowman : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _health = 100f;

        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Init(SnowmanData data)
        {
            _health = data.Health;
            _navMeshAgent.speed = data.Speed;
        }

        public void Damage(float damage)
        {
            if (damage < 0) throw new System.Exception("Damage can't be negative");

            _health -= damage;

            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void SetDestination(Vector3 target)
        {
            _navMeshAgent.SetDestination(target);
        }
    }
}
