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

        [SerializeField] private Vector2 _randomValues = new Vector2(2, 3);

        private GameManager _gameManager;

        public event EventHandler OnPresentsOvered;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.GameLosed += (sender, e) => StopSpawn();
            _gameManager.GameOvered += (sender, e) => StopSpawn();
        }

        private void OnPresentDestroyed(object sender, Present present)
        {
            if (present is not null)
            {
                present.OnPresentDestroyed -= OnPresentDestroyed;
            }

            var presents = FindObjectsOfType<Present>().Where(x => x.enabled);

            if (!presents.Any())
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

                var presents = FindObjectsOfType<Present>().Where(x => x.enabled).ToList();
                Present present = presents.Find(x => !x.IsBusy);

                if (present is null)
                {
                    snowman.SetDestination(GenerateDestination());
                }
                else
                {
                    snowman.SetPresent(present);
                }

                snowman.Init(WaveData.SnowmanData);
                snowman.SnowmanKilled += SnowmanKilled;

                yield return new WaitForSeconds(WaveData.SpawnTime);
            }
        }

        private void SnowmanKilled(object sender, Snowman e)
        {
            Present present = e.GetPresent();

            if (present is not null)
            {
                present.IsBusy = false;
            }

            e.SnowmanKilled -= SnowmanKilled;

            Destroy(e.gameObject);

            List<Snowman> snowmen = FindObjectsOfType<Snowman>().Where(x => x.enabled).ToList();

            var snowman = snowmen.Find(x => x.HaveNotPresent());

            if (present is not null && snowman is not null)
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
            List<Snowman> snowmen = FindObjectsOfType<Snowman>().Where(x => x.enabled).ToList();

            if (snowmen.Any())
            {
                snowmen.ForEach(x => Destroy(x.gameObject));
            }

            WaveData = waveData;

            SpawnPresents();
            StartCoroutine(StartWave());
        }

        private void SpawnPresents()
        {
            var presents = FindObjectsOfType<Present>().Where(x => x.enabled).ToList();

            if (presents.Any())
            {
                for (int i = 0; i < presents.Count; i++)
                {
                    Destroy(presents[i].gameObject);
                }
            }

            foreach (var item in _presentsPositions)
            {
                Vector3 position = item.position;

                position.x += Random.Range(_randomValues.x, _randomValues.y);
                position.z += Random.Range(_randomValues.x, _randomValues.y);

                var present = Instantiate(_presentPrefab, position, Quaternion.identity);
                present.OnPresentDestroyed += OnPresentDestroyed;
            }
        }

        public void StopSpawn()
        {
            StopAllCoroutines();
        }

        public bool IsThereSnowmenOnMap()
        {
            return FindObjectsOfType<Snowman>().Where(x => x.enabled).Any();
        }

        public bool IsTherePresentsOnMap()
        {
            return FindObjectsOfType<Present>().Where(x => x.enabled).Any();
        }

        public void ResetWave()
        {
            SpawnPresents();
            StartNextWave(WaveData);
        }

        public void Kill(Snowman snowman)
        {
            Destroy(snowman.gameObject);
        }
    }
}
