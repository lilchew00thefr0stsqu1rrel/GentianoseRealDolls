using NTC.MonoCache;
using System;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class DollPart : MonoCache
    {
        // TODO: сделать поля открытыми для UI
        // звери больше не зависят от UI, требования принципа DI
        // Каждые 0.1 с обновляется HUD
        
        protected DollController m_DollContr;
        protected Party m_Party;
        protected Animator m_Animator;

        [SerializeField] protected GameObject m_EffectBale;

        public void Construct(DollController contr, Party party)
        {
            m_DollContr = contr;
            m_Party = party;
        }

        
        public virtual void SetAimInput(Vector2 aimInput) { }
        public virtual void SetActionTime(float time) { }
        

        public virtual void Use() { }
    }
}


