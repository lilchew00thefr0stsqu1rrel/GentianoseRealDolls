using SpaceShooter;
using UnityEngine;
using System.Threading.Tasks;
using VContainer;
using System.Collections;

namespace GentianoseRealDolls
{
    public class Buoyancy : MonoBehaviour
    {
        [SerializeField] private Party m_Party;
        [Inject]
        public void Construct(Party p)
        {
            m_Party = p;
        }
        [SerializeField] private BoxCollider m_BoxCollider;


        private void Start()
        {
            m_BoxCollider = GetComponent<BoxCollider>();
        
            UpdateByDollSize();
        }

        private async void UpdateByDollSize()
        {
            if (m_Party.ActiveDoll.DollSize == 2)
            {
                if (m_BoxCollider)
                    m_BoxCollider.enabled = false;
            }
            else
            {
                if (m_BoxCollider)
                    m_BoxCollider.enabled = true;
            }
            await Task.Delay(500);
            UpdateByDollSize();
        }

    }
}

