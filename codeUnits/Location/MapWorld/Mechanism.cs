using NTC.MonoCache;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    /// <summary>
    /// Original name: EnterHabitat
    /// </summary>
    public class Mechanism : InteractableObject
    {

        [SerializeField] private int m_MechanismID;

        [SerializeField] private int m_State;
        [SerializeField] private Animator m_Animator;

        [SerializeField] private ThrowBeastsToScene m_ThrowBeastsToScene;
        public async void Activate()
        {
            m_Animator.enabled = true;

            await Task.Delay(600);

            m_ThrowBeastsToScene?.Teleport();
        }


       /// [SerializeField] private Canvas Interact;
    }
}

