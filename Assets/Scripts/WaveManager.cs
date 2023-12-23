using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameJam
{
    public class WaveManager : MonoBehaviour
    {
        public WaveData WaveData;

        [SerializeField] private Snowman _snowmanPrefab;
        [SerializeField] private Present _presentPrefab;

        [SerializeField] private List<Transform> _startPositions;
        [SerializeField] private List<Transform> _targetPositions;
        [SerializeField] private List<Transform> _presentsPositions;

        private List<Present> _presents = new();

        private List<Snowman> _snowmen = new();

        private GameManager _gameManager;

        public event EventHandler OnWaveOver;
        public event EventHandler OnPresentsOvered;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.OnGameLose += (sender, e) => StopSpawn();
            _gameManager.OnGameOver += (sender, e) => StopSpawn();

            _presents = FindObjectsOfType<Present>().ToList();
            _presents.ForEach(x => x.OnPresentDestroyed += OnPresentDestroyed);
        }

        private void OnPresentDestroyed(object sender, Present present)
        {
            _presents.Remove(present);

            if (_presents.Count == 0)
            {
                OnPresentsOvered?.Invoke(this, null);
            }
        }

        private IEnumerator StartWave()
        {
            for (int i = 0; i < WaveData.SnowmanCount; i++)
            {
                float x = Random.Range(_startPositions[0].position.x, _startPositions[1].position.x);
                float y = Random.Range(_startPositions[0].position.y, _startPositions[1].position.y);
                float z = Random.Range(_startPositions[0].position.z, _startPositions[1].position.z);

                Vector3 position = new Vector3(x, y, z);

                Snowman snowman = Instantiate(_snowmanPrefab, position, Quaternion.identity);

                Present present = _presents.Find(x => !x.IsBusy);
                Vector3 targetPosition = Vector3.zero;

                if (present is null)
                {
                    targetPosition = GenerateDestination();
                }
                else
                {
                    present.IsBusy = true;
                    targetPosition = present.transform.position;
                }

                snowman.Init(WaveData.SnowmanData, present);

                snowman.SetDestination(targetPosition);
                snowman.SnowmanKilled += SnowmanKilled;
                snowman.SnowmanDestroyed += SnowmanDestroyed;

                _snowmen.Add(snowman);

                yield return new WaitForSeconds(WaveData.SpawnTime);
            }
        }

        private void SnowmanDestroyed(object sender, Snowman snowman)
        {
            _snowmen.Remove(snowman);
        }

        private void SnowmanKilled(object sender, Snowman e)
        {
            Present present = e.GetPresent();

            _snowmen.Remove(e);
            Destroy(e.gameObject);

            var snowman = _snowmen.Find(x => x.HaveNotPresent());
            if (snowman is not null)
            {
                snowman.SetPresent(present);
            }
        }

        private Vector3 GenerateDestination()
        {
            float x = Random.Range(_targetPositions[0].position.x, _targetPositions[1].position.x);
            float y = Random.Range(_targetPositions[0].position.y, _targetPositions[1].position.y);
            float z = Random.Range(_targetPositions[0].position.z, _targetPositions[1].position.z);

            return new Vector3(x, y, z);
        }

        public void StartNextWave(WaveData waveData)
        {
            _snowmen.ForEach(x => Destroy(x.gameObject));
            _snowmen.Clear();

            WaveData = waveData;

            SpawnPresents();
            StartCoroutine(StartWave());
        }

        private void SpawnPresents()
        {
            foreach (var item in _presentsPositions)
            {
                var present = Instantiate(_presentPrefab, item.position, Quaternion.identity);
                _presents.Add(present);
            }
        }

        public void StopSpawn()
        {
            StopAllCoroutines();
        }

        public bool IsThereSnowmenOnMap()
        {
            return _snowmen.Count != 0;
        }

        public bool IsTherePresentsOnMap()
        {
            return _presents.Count != 0;
        }
    }
}
