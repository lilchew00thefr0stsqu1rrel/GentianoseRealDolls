using System;
using System.Threading.Tasks;
using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class TimePastStats : MonoBehaviour
    {


        [SerializeField] private long m_CurrentTime;
        [SerializeField] private long m_PreviousTime;

        [SerializeField] private long m_TimeDifference;

        [SerializeField] private UnityEngine.UI.Text m_DebugText;

        private const long TicksInMinute = 600000000;
        public long ReadTime()
        {
            Saver<long>.TryLoad(WhooSettings.fileNameTime, ref m_PreviousTime);
            if (m_PreviousTime == 0)
            {
                m_PreviousTime = DateTime.Now.Ticks / TicksInMinute;
            }

            m_CurrentTime = DateTime.Now.Ticks / TicksInMinute;

            m_TimeDifference = m_CurrentTime - m_PreviousTime;

            return m_TimeDifference;
        }

        private void OnDestroy()
        {
            WriteTimeFinal();
        }

        //16V26 14:42
        private void OnApplicationPause(bool pause)
        {  
            if (pause) //В 0.4.1
                WriteTimeFinal();
        }

        public void WriteTimeFinal()
        {
            Saver<long>.Save(WhooSettings.fileNameTime, DateTime.Now.Ticks / TicksInMinute);
        }

    }
}
