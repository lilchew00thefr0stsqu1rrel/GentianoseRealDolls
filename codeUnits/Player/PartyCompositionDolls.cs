using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class PartyCompositionDolls : MonoBehaviour
    {
        private string fileName = "partyDolls.dat";

        [SerializeField] private int[] m_DollsParty;

        private void Start()
        {
            m_DollsParty = new int[3] { 0, 1, 3 };
        }

        public void SetDollInParty(string dolladdr)
        {
            int index = int.Parse(dolladdr[..1]);
            int dollID = int.Parse(dolladdr[1..]);
            m_DollsParty[index] = dollID;

            Saver<int[]>.Save(fileName, m_DollsParty);
        }

        public int[] GetDollsInParty()
        {
            Saver<int[]>.TryLoad(fileName, ref m_DollsParty);
            return m_DollsParty;
        }
    }

}
