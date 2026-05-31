

namespace Common
{
    public class Stopwatch
    {
        private float m_CurrentTime;

        private float m_StopTime;

        public bool IsFinished => m_CurrentTime >= m_StopTime;

        public bool IsRunning;

        public string Tag;

        public Stopwatch(float stopTime, string tag)
        {
            m_StopTime = stopTime;
            Tag = tag;
        }

        public void Start()
        {
            IsRunning = true;
            m_CurrentTime = 0;
        }

        public void RemoveTime(float deltaTime)
        {
            if (m_CurrentTime >= m_StopTime) IsRunning = false;

            if (IsRunning == true)

                m_CurrentTime += deltaTime;


            //Debug.Log(Tag);

            //Debug.Log(m_CurrentTime + " Sn");
        }
    }

}


