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
            gameObject.SetActive(m_MiniMap);
        }


        [SerializeField] private RectTransform m_Pointer;

        [SerializeField] private FollowCamera m_Camera;
        

        [SerializeField] private Scrollbar m_ScrollbarHor;
        [SerializeField] private Scrollbar m_ScrollbarVert;
        [SerializeField] private bool m_MiniMap;

        protected override void OnEnabled()
        {
            if (m_Party != null && m_Party.ActiveDoll != null)
            {
                Transform dollTransform = m_Party.ActiveDoll.transform;

                print(dollTransform.position);

                if (!m_MiniMap)
                m_Pointer.anchoredPosition = new Vector2(dollTransform.position.x, dollTransform.position.z);

                m_Pointer.rotation = new Quaternion(0, 0, -dollTransform.rotation.y,
                    dollTransform.rotation.w);
                if (m_ScrollbarHor && m_ScrollbarVert) 
                { 
                    m_ScrollbarHor.value = 0;
                    m_ScrollbarVert.value = 0;
                }
            }
            

           

        }

        private void Update()
        {
            if (m_Party != null && m_Party.ActiveDoll != null)
            {
                Transform dollTransform = m_Party.ActiveDoll.transform;

                print(dollTransform.position);


                m_Pointer.rotation = new Quaternion(0, 0, -dollTransform.rotation.y,
                    dollTransform.rotation.w);
            }
        }

    }
}

