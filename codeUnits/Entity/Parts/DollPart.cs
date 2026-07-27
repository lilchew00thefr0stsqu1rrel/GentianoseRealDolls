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

        // 20vii26

        [SerializeField] protected bool m_TogetherWithNormalAttack;
        public bool TogetherWithNormalAttack => m_TogetherWithNormalAttack;

        [SerializeField] private bool m_IsRepeating;
        public bool Repeating => m_IsRepeating;

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



