using UnityEngine;

namespace GameJam
{
    [CreateAssetMenu(fileName = "SnowmanData", menuName = "Custom/WaveData")]
    public class WaveData : ScriptableObject
    {
        public float WaveTime = 10;
        public float SnowmanCount = 5;
        public float SpawnTime = 1.25f;
        public SnowmanData SnowmanData;
    }
}
