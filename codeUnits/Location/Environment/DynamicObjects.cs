using UnityEngine;

namespace GentianoseRealDolls
{
    public class DynamicObjects : SingletonBase<DynamicObjects>
    {
        [SerializeField] private Animator[] m_AnimatorObjects;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Toggle(int id, bool onOff)
        {
            if (m_AnimatorObjects[id] != null)
            {
                m_AnimatorObjects[id].enabled = onOff;

            }
        }
    }

}
