using UnityEngine;

namespace GentianoseRealDolls
{
    public class PartyMenu : MonoBehaviour
    {
        [SerializeField] private PartyCompositionDolls partyCompositionDolls;

        [SerializeField] private UIButton[] m_Buttons;

        private void OnEnable()
        {
            foreach (var button in m_Buttons)
            {
                button.SetInteractable(true);
            }
            m_Buttons[partyCompositionDolls.GetDollsInParty()[2]].SetInteractable(false);
        }
    }
}
