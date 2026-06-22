using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GentianoseRealDolls
{

    public class SleepUI : DashboardBase
    {
        [SerializeField] private int[] m_AllSleepPoints;
        [SerializeField] private bool[] m_AllSleepStates;
        [SerializeField] private AllDollSleeps m_AllDollSleeps;
        [SerializeField] private AllDollCharacters m_AllDollCharacters;
        [SerializeField] private Text[] m_AllSleepPointTexts;
        [SerializeField] private Image[] m_AllSleepImages;
        [SerializeField] private Image[] m_AllAwakeImages;
        [SerializeField] private Text m_Warn;
        [SerializeField] private Party m_Party;

        public void SetDollSleep(int dollID, bool sleeping)
        {
            if (m_Party.ActiveDoll.DollID == dollID)
            {
                Warn();
            }

            else
            {
                InitDollSleep(dollID, sleeping);
            }
        }

        private void InitDollSleep(int dollID, bool sleeping)
        {

            m_AllSleepStates[dollID] = sleeping;

            m_AllAwakeImages[dollID].gameObject.SetActive(!sleeping);
            m_AllSleepImages[dollID].gameObject.SetActive(sleeping);

            m_AllDollSleeps.WriteDoll(new int[] { dollID, sleeping ? 1 : 0 });
        }
        private async void Warn()
        {
            m_Warn.enabled = true;
            await Task.Delay(1000);
            m_Warn.enabled = false;
        }
        public void WakeDoll(int dollID)
        {
            SetDollSleep(dollID, false);
        }
        public void GoDollToBed(int dollID)
        {
            SetDollSleep(dollID, true);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InitSleep();
        }

        private void OnEnable()
        {
            InitSleep();
        }

        public override void UpdateUI()
        {
            m_AllSleepPoints = new int[3];

            var dolls = m_AllDollCharacters.GetDolls();

            for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
            {
                m_AllSleepPoints[i] = dolls[8 * i + 7];
                m_AllSleepPointTexts[i].text = m_AllSleepPoints[i].ToString();

            }


        }

        private void InitSleep()
        {
            m_AllSleepStates = new bool[3];
            var sleep = m_AllDollSleeps.GetDolls();
            for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
            {
                InitDollSleep(i, sleep[2 * i + 1] == 1);
            }

            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

    }

}
