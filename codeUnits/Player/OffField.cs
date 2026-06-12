using UnityEngine;


namespace GentianoseRealDolls
{

    public class OffField : MonoBehaviour
    {

        DollPart[] m_OffFieldParts;

        public void Use(Vector2 aimInput, float time)
        {
            for (int i = 0; i < m_OffFieldParts.Length; i++)
            {
                m_OffFieldParts[i].Use(aimInput, time);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
