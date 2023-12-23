using System;
using UnityEngine;

namespace GameJam
{
    public class Present : MonoBehaviour
    {
        public bool IsBusy { get; set; } = false;

        public event EventHandler<Present> OnPresentDestroyed;

        private void OnDestroy()
        {
            OnPresentDestroyed?.Invoke(this, this);
        }
    }
}
