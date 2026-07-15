using UnityEngine;

namespace GentianoseRealDolls
{

    public class DollFood : DollComponent
    {
        [SerializeField] private InventoryItem[] m_FaveFood;
        [SerializeField] private AllDollPetStats m_AllDollPetStats;
        
        public void SetAllPet(AllDollPetStats allDollPetStats)
        {
            m_AllDollPetStats = allDollPetStats;
        }

        public void Eat(InventoryItem item)
        {
            bool fave = false;

            for (int i = 0; i < m_FaveFood.Length; i++)
            {
                if (m_FaveFood[i] == item)
                {
                    fave = true;
                }
            }

            var doll = m_AllDollPetStats.GetDoll(m_Doll.DollID);

            if (fave) 
            {
                doll[6] += 35;
            }
            else
            {
                doll[6] += 25;
            }
            m_AllDollPetStats.WriteDoll(doll);
            m_Doll.FillStats(doll);
        }
    }

}
