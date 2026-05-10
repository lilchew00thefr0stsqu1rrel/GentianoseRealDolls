using System;
using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class TimePastStats : MonoBehaviour
    {
        private const string fileName1 = "timeCurr.dat";
        private const string fileName2 = "timePrev.dat";

        private long m_CurrentTime;
        private long m_PreviousTime;

        private long m_TimeDifference;

        public long ReadTime()
        {
            //Saver<long>.TryLoad(fileName1, ref m_CurrentTime);
            Saver<long>.TryLoad(fileName2, ref m_PreviousTime);

            m_CurrentTime = DateTime.Now.Ticks / 600000000;
            m_TimeDifference = m_CurrentTime - m_PreviousTime;

            return m_TimeDifference;
        }
        public void RefreshTime()
        {
        }

        public void WriteTimeDestroy()
        {
            Saver<long>.Save(fileName2, m_PreviousTime);
        }

        private void OnDestroy()
        {
            ////m_PreviousTime = m_CurrentTime;
            //
            //// Saver<long>.Save(fileName2, m_PreviousTime);
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
