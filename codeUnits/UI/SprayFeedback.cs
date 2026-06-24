using UnityEngine;
using UnityEngine.UI;

namespace GentianoseRealDolls
{
    public class SprayFeedback : MonoBehaviour
    {
        [SerializeField] private Image m_FillImage;
        [SerializeField] private float m_AnalSprayAmount;
        [SerializeField] private Doll m_CurrentDoll;
        [SerializeField] private Text m_FluidText;

        [SerializeField] private int m_CurrentDollID;
        [SerializeField] private int m_CurrentDollIndexInParty;

        [SerializeField] private Image m_SprayIcon;

        [SerializeField] private Party m_Party;


        public void UpdateUI()
        {
            m_CurrentDoll = m_Party.ActiveDoll;
            m_CurrentDollID = m_Party.ActiveDollID;

            int analGlandVolume = m_Party.DollData.AnalGlandVolumeArray[m_CurrentDollID];


            m_FillImage.fillAmount = (float)m_CurrentDoll.AnalSprayAmount / analGlandVolume;
            m_FluidText.text = $"{Mathf.Round(m_CurrentDoll.AnalSprayAmount) / 10} / " +
                $"{analGlandVolume / 10} мл";
        }


    }
}

