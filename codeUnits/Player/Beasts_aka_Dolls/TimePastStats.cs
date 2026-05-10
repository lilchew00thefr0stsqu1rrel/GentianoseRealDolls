using System;
using System.Threading.Tasks;
using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class TimePastStats : MonoBehaviour
    {
        private const string fileName = "timePrev.dat";

        [SerializeField] private long m_CurrentTime;
        [SerializeField] private long m_PreviousTime;

        [SerializeField] private long m_TimeDifference;

        public long ReadTime()
        {
            Saver<long>.TryLoad(fileName, ref m_PreviousTime);

            m_CurrentTime = DateTime.Now.Ticks / 600000000;
            m_TimeDifference = m_CurrentTime - m_PreviousTime;

            return m_TimeDifference;
        }

        private void OnDestroy()
        {
            WriteTimeFinal();
        }

        public void WriteTimeFinal()
        {
            Saver<long>.Save(fileName, DateTime.Now.Ticks / 600000000);
        }

    }
}

