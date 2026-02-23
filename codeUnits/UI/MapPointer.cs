using UnityEngine;

namespace GentianoseRealDolls
{
    public class MapPointer : MonoBehaviour
    {
        [SerializeField] private RectTransform m_Pointer;

        private void OnEnable()
        {

            Transform dollTransform = Party.Instance.ActiveDoll.transform;
            Camera camera = ActiveCamera.Instance.GetComponent<Camera>();

            print(dollTransform.position);

            m_Pointer.anchoredPosition = new Vector2(dollTransform.position.x, dollTransform.position.z);

            m_Pointer.rotation = new Quaternion(0, 0, -dollTransform.rotation.y,
                dollTransform.rotation.w);

        }

    }
}

