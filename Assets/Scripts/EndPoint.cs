using UnityEngine;

namespace GameJam
{
    public class EndPoint : MonoBehaviour
    {
        private WaveManager _waveManager;

        private void Start()
        {
            _waveManager = FindObjectOfType<WaveManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Snowman snowman))
            {
                _waveManager.Kill(snowman);
            }
        }
    }
}
