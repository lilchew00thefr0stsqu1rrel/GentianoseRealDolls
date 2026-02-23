using System.Collections;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class BathInterface : SingletonBase<BathInterface>
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (m_WaterParticlesBath)
                m_WaterParticlesBath.Stop();

        }

        // Update is called once per frame
        void Update()
        {

        }

        [SerializeField] private Doll m_Doll;
        [SerializeField] private ParticleSystem m_WaterParticlesBath;
        [SerializeField] private ParticleSystem m_WaterParticlesSink;

        public void Wash(Doll doll)
        {
            m_Doll = doll;
            m_Doll.DollController.Wash();

            if (m_WaterParticlesBath != null)
            {
                m_WaterParticlesBath.Play();

                IEnumerator WaterTime()
                {
                    yield return new WaitForSeconds(7);
                    m_WaterParticlesBath.Stop();
                }

                StartCoroutine(WaterTime());
            }
        }
        public void BrushTeeth(Doll doll)
        {
            m_Doll = doll;
            m_Doll.DollController.BrushTeeth();

            if (m_WaterParticlesSink != null)
            {
                m_WaterParticlesSink.Play();
            }
        }

    }
}

