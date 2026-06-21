using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GentianoseRealDolls
{

    public class OffField : MonoBehaviour
    {

        DollPart[] m_OffFieldParts;
        
        public void SetSummon(DollPart part, int id)
        {
            m_OffFieldParts[id] = part;
        }


        public void Use()
        {
            for (int i = 0; i < m_OffFieldParts.Length; i++)
            {
                if (m_OffFieldParts[i] != null)
                {
                    m_OffFieldParts[i].Use();
                }
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_OffFieldParts = new DollPart[3];
            StartCoroutine(UseRoutine());
        }

        IEnumerator UseRoutine()
        {
            Use();

            

            yield return new WaitForSeconds(1);
            StartCoroutine(UseRoutine());
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
