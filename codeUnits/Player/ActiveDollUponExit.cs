using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class ActiveDollUponExit : MonoBehaviour
    {
        private const string fileName = "activeDoll.dat";

        [SerializeField] private int m_ActiveDollInPartyIndex;

        public void SetActiveDoll(int activeDollInPartyIndex)
        {
            m_ActiveDollInPartyIndex = activeDollInPartyIndex;

            Saver<int>.Save(fileName, m_ActiveDollInPartyIndex);
        }

        public int GetActiveDoll()
        {
            Saver<int>.TryLoad(fileName, ref m_ActiveDollInPartyIndex);

            return m_ActiveDollInPartyIndex;
        }
    }

}

