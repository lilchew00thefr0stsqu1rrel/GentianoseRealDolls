using UnityEngine;

namespace GentianoseRealDolls
{
    public abstract class DollComponent : MonoBehaviour
    {
        protected Doll m_Doll;
        protected Animator m_Animator;

        protected int m_DollIndexInParty;

        public virtual void SetProperties(Doll doll, Animator animator, int posInParty)
        {
            this.m_Doll = doll;
            this.m_Animator = animator;

            m_DollIndexInParty = posInParty;
        }
    }

}


