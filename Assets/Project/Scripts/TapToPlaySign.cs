using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project
{
    public class TapToPlaySign : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _blockImage;
        [SerializeField] private TMP_Text _sign;

        public Subject<Unit> Tapped { get; } = new Subject<Unit>();
        
        public void Activate()
        {
            _blockImage.enabled = true;
            _sign.enabled = true;
        }
        
        public void Disable()
        {
            _blockImage.enabled = false;
            _sign.enabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Tapped.OnNext(Unit.Default);
        }
    }
}