using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui
{
    public class InputPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            GameActions.OnTap?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }
    }
}