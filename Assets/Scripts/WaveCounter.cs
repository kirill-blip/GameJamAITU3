using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class WaveCounter : MonoBehaviour
    {
        [SerializeField] private List<Image> _numbers;
        [SerializeField] private List<Sprite> _colorNumbers;

        public void DisplayFirstNumber()
        {
            _numbers[0].sprite = _colorNumbers[0];
        }

        public void DisplaySecondNumber()
        {
            _numbers[1].sprite = _colorNumbers[1];
        }

        public void DisplayThirdNumber()
        {
            _numbers[2].sprite = _colorNumbers[2];
        }
    }
}
