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
        [SerializeField] private long m_TimeDifference5min;
        [SerializeField] private long m_TimeDifference15min;
        [SerializeField] private UnityEngine.UI.Text m_DebugText;

        public enum TimeIntervals
        {
            PooTime,
            BathTime,
            FoodTime
        }

        public long ReadTime(TimeIntervals timeType = TimeIntervals.PooTime)
        {
            if (timeType == TimeIntervals.FoodTime)
            {
                Saver<long>.TryLoad(WhooSettings.fileNameTime, ref m_PreviousTime);
                if (m_PreviousTime == 0)
                {
                    m_PreviousTime = DateTime.Now.Ticks / 9000000000;
                }

                m_CurrentTime = DateTime.Now.Ticks / 9000000000;

                m_TimeDifference = m_CurrentTime - m_PreviousTime;

                return m_TimeDifference;
            }

            if (timeType == TimeIntervals.BathTime)
            {
                Saver<long>.TryLoad(WhooSettings.fileNameTime, ref m_PreviousTime);
                if (m_PreviousTime == 0)
                {
                    m_PreviousTime = DateTime.Now.Ticks / 3000000000;
                }

                m_CurrentTime = DateTime.Now.Ticks / 3000000000;

                m_TimeDifference = m_CurrentTime - m_PreviousTime;

                return m_TimeDifference;
            }

            Saver<long>.TryLoad(WhooSettings.fileNameTime, ref m_PreviousTime);
                if (m_PreviousTime == 0)
                {
                    m_PreviousTime = DateTime.Now.Ticks / 600000000;
                }

                m_CurrentTime = DateTime.Now.Ticks / 600000000;

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
            Saver<long>.Save(WhooSettings.fileNameTime, DateTime.Now.Ticks / 600000000);
        }

    }
}
