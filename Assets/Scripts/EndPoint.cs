using System;
using UnityEngine;

namespace GameJam
{
    public class EndPoint : MonoBehaviour
    {
        public event EventHandler SnowmanCollided;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Snowman _))
            {
                SnowmanCollided?.Invoke(this, null);
            }
        }
    }
}
