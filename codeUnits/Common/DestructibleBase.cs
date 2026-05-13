
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using UnityEngine.Events;

using NTC.Pool;


namespace Common 

{

    /// <summary>
    /// Уничтожаемый объект на сцене. То, что может иметь хит поинты.
    /// </summary>
    public abstract class DestructibleBase : Entity
    {
        #region Properties
        /// <summary>
        /// Объект игнорирует повреждения.
        /// </summary>
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Стартовое количество хит поинтов
        /// </summary>
        [SerializeField] private int m_HitPoints;
        public int MaxHitPoints => m_HitPoints;

        /// <summary>
        /// Текущие хит поинты.
        /// </summary>
        [SerializeField] private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;


        [SerializeField] private int m_OriginalHitPoints;
        public int OriginalHitPoints => m_OriginalHitPoints;


        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;


        protected void MinHPState()
        {
            m_CurrentHitPoints = Mathf.Clamp(m_CurrentHitPoints, 1, m_HitPoints);
        }


        #endregion


        #region UnityEvents

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;

            transform.SetParent(null);  // У меня и без этой строки объект помещается в корень сцены.
        }

        #endregion

        #region Public API

        protected virtual void SetImpactParentShipDamage(ImpactEffect eff)
        {
            
        }

        [SerializeField] private bool m_Resilient;
        /// <summary>
        /// Применение дамага к объекту.
        /// </summary>
        /// <param name="damage"> Урон наносимый объекту</param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;


            m_CurrentHitPoints -= damage;

            // print(m_CurrentHitPoints); 

            if (m_Resilient && m_CurrentHitPoints <= 0)
            {
                m_CurrentHitPoints = 1;
            }
            ;

            if (m_CurrentHitPoints <= 0)
                OnDeath();

        }

        public void SetMaxHitPoints(int hitPoints)
        {
            if (m_Indestructible) return;            

            m_HitPoints = hitPoints;

            RecordMaxHP(hitPoints);
        }

        public virtual void RecordMaxHP(int hitPoints) { }

        public virtual void IncreaseMaxHitPoints(int addedMaxHitPoints)
        {
            SetMaxHitPoints(m_HitPoints + addedMaxHitPoints);
        }

        public virtual void ResetMaxHitPoints()
        {
            SetMaxHitPoints(OriginalHitPoints); 
        }

        public void RestoreHitPoints(int hitPoints)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints += hitPoints;

            if (m_CurrentHitPoints > m_HitPoints)
                m_CurrentHitPoints = m_HitPoints;
        }

        #endregion


        

        /// <summary>
        /// Переопределяемое событие уничтожения объекта, когда хит поинты ниже нуля.
        /// </summary>
        protected virtual void OnDeath()
        {
           
            // Impact effect after death (explosion, gore, etc).
            if (m_ImpactEffectPrefab != null)
            {
                ImpactEffect impactEffect = Instantiate(m_ImpactEffectPrefab, gameObject.transform.position, Quaternion.identity);
            }

            m_EventBeforeDeath?.Invoke();

          //  Destroy(gameObject);
            NightPool.Despawn(gameObject);
            m_EventOnDeath?.Invoke();

        }

 

        

        private static HashSet<DestructibleBase> m_AllDestructibles;

        public static IReadOnlyCollection<DestructibleBase> AlLDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)            
                m_AllDestructibles = new HashSet<DestructibleBase>();
            
            m_AllDestructibles.Add(this);          
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

   

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;


        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        

        [SerializeField] private UnityEvent m_EventBeforeDeath;
        public UnityEvent EventBeforeDeath => m_EventBeforeDeath;

        protected virtual void Use(EnemyAsset asset)
        {
            m_HitPoints = asset.hp;
        }

        protected virtual void SetIndestructible(bool indestr)
        {
            m_Indestructible = indestr;
        }

    }
}
