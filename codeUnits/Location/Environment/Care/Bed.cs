using UnityEngine;

namespace GentianoseRealDolls
{


    public class Bed : InteractableObject
    {
        [SerializeField] private Material m_SheetMaterial;
        [SerializeField] private MeshRenderer m_MattressSheet;
        [SerializeField] private Material m_WoodMaterial;
        [SerializeField] private MeshRenderer[] m_Wood;


        [SerializeField] private int m_OffTipID = 5;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tipID = 4;
            if (m_SheetMaterial != null && m_MattressSheet != null)
            {
                m_MattressSheet.material = m_SheetMaterial;
            }
            if (m_WoodMaterial != null && m_Wood != null)
            {
                foreach (var wood in m_Wood)
                {
                    wood.material = m_WoodMaterial;
                }
            }
        }

    

        protected override void OnDollCome(Party partyWisp)
        {
            if (partyWisp.ActiveDoll.DollController.SleepSystem.IsSleeping)
            {
                m_Dashboard.ShowInteractTip(m_OffTipID);
            }

            else
            {
                m_Dashboard.ShowInteractTip(tipID);
            }
        }       
    }
}
