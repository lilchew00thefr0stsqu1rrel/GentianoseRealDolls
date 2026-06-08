using NTC.MonoCache;
using UnityEngine;
using VContainer;

namespace GentianoseRealDolls
{
    public abstract class DollComponent : MonoCache
    {
        protected Dashboard m_Dashboard;

        protected Party m_Party;

        protected Doll m_Doll;
        public Doll Doll => m_Doll;
        //  protected Animator m_Animator;

        protected AnimatorGuard m_AnimatorGuard;

        protected int m_DollIndexInParty;
        [SerializeField] protected Inventory m_Inventory;

        public virtual void SetProperties(Doll doll, AnimatorGuard animatorGuard, int posInParty)
        {
            m_Doll = doll;
            m_AnimatorGuard = animatorGuard;
            m_DollIndexInParty = posInParty;
        }

        public virtual void ConstructDollCom(Party party)
        {
            m_Party = party;
        }
        public virtual void ConstructDollCom(Inventory inventory)
        {
            m_Inventory = inventory;
        }
    }

}





