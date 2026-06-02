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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }


        public void UpdateUI()
        {
            m_FillImage.fillAmount = (float)m_Party.ActiveDoll.AnalSprayAmount / m_Party.ActiveDoll.AnalGlandVolume;
            m_FluidText.text = $"{Mathf.Round(m_Party.ActiveDoll.AnalSprayAmount) / 10} / " +
                $"{Mathf.Round(m_Party.ActiveDoll.AnalGlandVolume) / 10} мл";
        }


        public void InitDollSpray(Doll d)
        {
            m_CurrentDoll = d;

            m_FillImage.sprite = d.Asset.RSkillFill;
            m_SprayIcon.sprite = d.Asset.RSkillIcon;
        }

        // Update is called once per frame
        void Update()
        {
            //if (m_CurrentDoll != null)
            //{
            //    m_FillImage.fillAmount = m_CurrentDoll.AnalSprayAmount / m_CurrentDoll.AnalGlandVolume;
            //    m_FluidText.text = $"{m_CurrentDoll.AnalSprayAmount / 10} / " +
            //        $"{m_CurrentDoll.AnalGlandVolume / 10} мл";
            //}
           
        }
    }
}

