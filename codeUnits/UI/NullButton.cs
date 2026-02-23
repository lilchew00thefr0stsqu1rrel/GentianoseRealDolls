using UnityEngine.EventSystems;

namespace GentianoseRealDolls
{
    public class NullButton : UIButton
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public new void OnDrag(PointerEventData eventData)
        {
            print("__");

        }
        public new void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public new void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}

