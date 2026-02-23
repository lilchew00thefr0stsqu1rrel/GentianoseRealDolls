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

    // Update is called once per frame
    void Update()
    {
        
    }

        protected override void OnDollCome(DollController doll)
        {
            if (doll.FullSleep)
            {
                Dashboard.Instance.ShowInteractTip(m_OffTipID);
            }
            else
            {
                Dashboard.Instance.ShowInteractTip(tipID);
            }
        }

     

       
    }
}