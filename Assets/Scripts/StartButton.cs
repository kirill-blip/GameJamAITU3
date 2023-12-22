using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameJam
{
    public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Material _yellowMaterial;
        [SerializeField] private Material _defaultMaterial;

        [SerializeField] private List<Light> _lights;
        [SerializeField] private List<Renderer> _rendrers;
        [SerializeField] private Renderer _doorRendrer;


        public void OnPointerEnter(PointerEventData eventData)
        {
            _lights.ForEach(x => x.gameObject.SetActive(true));
            _rendrers.ForEach(x => x.materials[1].color = _yellowMaterial.color);
            _doorRendrer.materials[2].color = _yellowMaterial.color;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _lights.ForEach(x => x.gameObject.SetActive(false));
            _rendrers.ForEach(x => x.materials[1].color = _defaultMaterial.color);
            _doorRendrer.materials[2].color = _defaultMaterial.color;
        }
    }
}
