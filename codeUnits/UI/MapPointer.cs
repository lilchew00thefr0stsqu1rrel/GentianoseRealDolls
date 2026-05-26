using NTC.MonoCache;
using SpaceShooter;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GentianoseRealDolls
{
    public class MapPointer : MonoCache
    {
        [SerializeField] private Party m_Party;
        ////[Inject]
        //public void Construct(Party obj)
        //{
        //    m_Party = obj;
        //}
        ////[Inject]
        //public void Construct(FollowCamera obj)
        //{
        //    m_Camera = obj;
        //}

        private void Start()
        {
            gameObject.SetActive(false);
        }


        [SerializeField] private RectTransform m_Pointer;

        [SerializeField] private FollowCamera m_Camera;
        

        [SerializeField] private Scrollbar m_ScrollbarHor;
        [SerializeField] private Scrollbar m_ScrollbarVert;

        protected override void OnEnabled()
        {
            if (m_Party != null)
            {
                Transform dollTransform = m_Party.ActiveDoll.transform;

                print(dollTransform.position);

                m_Pointer.anchoredPosition = new Vector2(dollTransform.position.x, dollTransform.position.z);

                m_Pointer.rotation = new Quaternion(0, 0, -dollTransform.rotation.y,
                    dollTransform.rotation.w);

                m_ScrollbarHor.value = 0;
                m_ScrollbarVert.value = 0;
            }
            

           

        }

    }
}

