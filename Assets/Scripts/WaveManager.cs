using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameJam
{
    public class WaveManager : MonoBehaviour
    {
        public WaveData WaveData;

        [SerializeField] private Snowman _snowmanPrefab;

        [SerializeField] private List<Transform> _startPositions;
        [SerializeField] private List<Transform> _targetPositions;

        private GameManager _gameManager;

        public event EventHandler OnWaveOver;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.OnGameLose += (sender, e) => StopSpawn();
            _gameManager.OnGameOver += (sender, e) => StopSpawn();
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
                snowman.Init(WaveData.SnowmanData);
                snowman.SetDestination(GenerateDestination());

                yield return new WaitForSeconds(WaveData.SpawnTime);
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
            WaveData = waveData;
            StartCoroutine(StartWave());
        }

        public void StopSpawn()
        {
            StopAllCoroutines();
        }
    }
}
