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


        [SerializeField] private Image m_SprayIcon;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        public void InitDollSpray(Doll d)
        {

            m_CurrentDoll = d;

            m_FillImage.sprite = m_CurrentDoll.Asset.RSkillFill;
            m_SprayIcon.sprite = m_CurrentDoll.Asset.RSkillIcon;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_CurrentDoll != null)
            {
                m_FillImage.fillAmount = m_CurrentDoll.AnalSprayAmount / m_CurrentDoll.AnalGlandVolume;
                m_FluidText.text = $"{Mathf.Round(m_CurrentDoll.AnalSprayAmount * 10) / 10} / " +
                    $"{Mathf.Round(m_CurrentDoll.AnalGlandVolume * 10) / 10} мл";
            }
           
        }
    }
}

