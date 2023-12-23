using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GameJam
{
    public class BackgroundSnowman : MonoBehaviour
    {
        [SerializeField] private float _waitTime = 2f;

        [SerializeField] private List<Transform> _positions;
        [SerializeField] private Transform _defaultPosition;

        private NavMeshAgent _navMeshAgent;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private IEnumerator MoveCouritine()
        {
            ResetPosition();

            Vector3 position = _positions[0].position;

            _navMeshAgent.SetDestination(position);

            yield return new WaitWhile(() => _navMeshAgent.isStopped);

            yield return new WaitForSeconds(_waitTime);

            position = _positions[1].position;

            _navMeshAgent.SetDestination(position);

            yield return new WaitWhile(() => Vector3.Distance(transform.position, position) <= 0.5f);
        }

        public void Move()
        {
            StartCoroutine(MoveCouritine());
        }

        public void ResetPosition()
        {
            transform.position = _defaultPosition.position;
        }
    }
}
