using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class FireworksManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> _positions;
        [SerializeField] private List<GameObject> _rockets;
        [SerializeField] private int _fireworkCount = 5;
        [SerializeField] private Vector2 _timeBetweenFireworks = new Vector2(.5f, 2f);

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.LastWavePlayed += OnLastWavePlayed;
        }

        private void OnLastWavePlayed(object sender, System.EventArgs e)
        {
            StartCoroutine(GenerateFireworks());
        }

        private IEnumerator GenerateFireworks()
        {
            for (int i = 0; i < _fireworkCount; i++)
            {
                var rocket = _rockets[Random.Range(0, _rockets.Count)];
                Vector3 position = Vector3.zero;

                position.x = Random.Range(_positions[0].position.x, _positions[1].position.x);
                position.y = Random.Range(_positions[0].position.y, _positions[1].position.y);
                position.z = Random.Range(_positions[0].position.z, _positions[1].position.z);

                var fireworks = Instantiate(rocket);
                fireworks.transform.position = position;
                fireworks.transform.eulerAngles = _positions[0].eulerAngles;

                yield return new WaitForSeconds(Random.Range(_timeBetweenFireworks.x, _timeBetweenFireworks.y));
            }
        }
    }
}
