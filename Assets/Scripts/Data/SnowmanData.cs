using UnityEngine;

namespace GameJam
{
    [CreateAssetMenu(fileName = "SnowmanData", menuName = "Custom/SnowmanData")]
    public class SnowmanData : ScriptableObject
    {
        [Range(20, 100)] public float Health = 20f;
        [Range(10, 100)] public float Speed = 20f;
    }
}
