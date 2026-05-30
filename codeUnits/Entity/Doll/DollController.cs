using SpaceShooter;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace GentianoseRealDolls
{
    [RequireComponent(typeof(Doll))]
    public class DollController : MonoBehaviour
    {
        // TODO: сделать поля открытыми для UI
        // звери больше не зависят от UI, требования принципа DI
        // Каждые 0.1 с обновляется HUD

        private Level m_Level;

        [SerializeField] private int m_LocationIndex;
        public void SetLocationIndex(int loc)
        {
            m_LocationIndex = loc;
        }

        private AllDollCharacters allDolls;
        private AllDollPositions allPositions;
        private AllDollSleeps allSleeps;
       

        [Header("Doll Base Attributes")]
        [SerializeField] private Doll m_Doll;
        public Doll Doll => m_Doll;

        [SerializeField] private bool m_IsActiveDollInParty;
        public bool ActiveDollInPartyStatus => m_IsActiveDollInParty;

        [SerializeField] private SpaceShip m_PetAsSpaceShip;
        public SpaceShip PetAsSpaceShip => m_PetAsSpaceShip;
        [SerializeField] private Animator m_Animator;
        public Animator Animator => m_Animator;
        [SerializeField] private AnimatorGuard m_AnimatorGuard;
        public AnimatorGuard AnimatorGuard => m_AnimatorGuard;
        [SerializeField] private BeastPositionManager positionManager;

        [SerializeField] private DollClimbing climbing;
        public DollClimbing Climbing => climbing;



        [Header("Doll Component")]


        [SerializeField] private DollGaitManager gaitGear;
        public DollGaitManager GaitManager => gaitGear;

        [SerializeField] private DollBattleManager battler;
        public DollBattleManager BattleManager => battler;

        [SerializeField] private DollPoopManager pooper;
        public DollPoopManager PoopManager => pooper;

        [SerializeField] private DollBath bathSystem;

        [SerializeField] private DollSleep sleepSystem;

        [SerializeField] private Inventory m_Inventory;

        
        [Header("Doll's particular parts, e.g. weapons and scent glands")]


        [SerializeField] private Vector2 m_AimInput;
        [SerializeField] private Vector2 m_MoveInput;

        private Dashboard m_Dashboard;
        private Party m_Party;
        public bool Sleeping => sleepSystem.Sleeping;

        private int m_DollIndexInParty;

        public int DollIndexInParty => m_DollIndexInParty;
        private DollScaleValues m_ScaleValues;
        private int dollID;
        private Rigidbody m_Rigidbody;


        public bool FullSleep => m_Doll.FullSleep;

        bool m_NavelEffect;
        [SerializeField] private GameObject m_NavelEffectPrefab;
        private GameObject m_NavelEffectShield;

        
        // Unity Event

        private void Awake()
        {


            dollID = m_Doll.DollID;
          //  InitAllDollComponents();

            m_Rigidbody = GetComponent<Rigidbody>();
        }


        private void Start()
        {
        }


        // Public API

        // Управление статусом куклы (активная или трейловая)

        public void SetDollAsActive(bool active)
        {
            m_IsActiveDollInParty = active;
        }

        // Установка индекса куклы в отряде (0 - 2)
        public void SetDollIndexInParty(int index)
        {
            m_DollIndexInParty = Mathf.Clamp(index, 0, 2);


            //InitAllDollComponents();
        }
        //  анимации
        public void SetAnimation(int anim)
        {
            if (!m_AnimatorGuard) return;
            m_AnimatorGuard.SetAnimation(anim);
        }

        // Сброс анимации
        public void SetIdle()
        {
            if (!m_AnimatorGuard) return;
            m_AnimatorGuard.SetAnimation(0);
        }


        // Передача положения курсора
        public void SetAimInput(Vector2 aimInput)
        {
            m_AimInput = aimInput;

            battler.SetAimInput(aimInput);


            
        }

        // Передача управления движением (WASD)
        public void UpdateMoveInput(Vector2 moveInput)
        {
            m_MoveInput = moveInput;
        }
        public void TriggerClimb(int gait)
        {
            climbing.ChangeClimb(gait);
        }

        // Инициализация всех 5 компонентов заботы и боя куклы
        private void InitAllDollComponents()
        {
            gaitGear.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
            pooper.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
            battler.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
            sleepSystem.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
            bathSystem.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
        }

        public void ConstructInventory(Inventory inventory)
        {
            m_Inventory = inventory;
            gaitGear.ConstructDollCom(m_Inventory);
            battler.ConstructDollCom(m_Inventory);

            pooper.ConstructDollCom(m_Inventory);

            bathSystem.ConstructDollCom(m_Inventory);
            sleepSystem.ConstructDollCom(m_Inventory);
        }

        public void ConstructSleep(List<bool> sleepData, AllDollSleeps sleeps)
        {
            allSleeps = sleeps;
            sleepSystem.ConstructDollCom(m_Party); ;
            sleepSystem.ConstructSleep(sleeps);
        }

        public void ConstructPos(AllDollPositions positions, AllDollSleeps sleeps)
        {
            allPositions = positions;

            positionManager.ConstructDolls( positions, sleeps);
        }

        public void ConstructPoop(PoopStore ps)
        {
            pooper.ConstructPoopStorage(ps);
        }

        public void ConstructDollParty(Party party)
        {
            m_Party = party;

            gaitGear.ConstructDollCom(m_Party);
            battler.ConstructDollCom(m_Party);

            pooper.ConstructDollCom(m_Party);

            bathSystem.ConstructDollCom(m_Party);
        }

        // TODO: Создать компонент DollEat
        public void ConstructDollStats(DollScaleValues[] dollData, AllDollCharacters dolls, AllDollPositions positions, Inventory inventory)
        {
            allDolls = dolls;

            m_Doll.ConstructDoll(dollData, dolls, positions, inventory);
        }
        public void SetDollProperties()
        {
            gaitGear.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
            battler.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
            pooper.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
            bathSystem.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
            sleepSystem.SetProperties(m_Doll, m_AnimatorGuard, m_DollIndexInParty);
        }
        // Настроить компоненты куклы (после создания куклы, работает как зависимости)
        public void ConstructDoll(List<float[]> dollData, AllDollCharacters dolls, AllDollPositions positions, 
            List<bool> sleepData, AllDollSleeps sleeps, Party party)
        {

            // Здесь ещё правильно
            print("Snowor" + dolls.GetDoll(0).LooPoo);
            // Начало бреда



            gaitGear.ConstructDollCom(m_Party);
            battler.ConstructDollCom(m_Party);

            pooper.ConstructDollCom(m_Party);

            bathSystem.ConstructDollCom(m_Party);
           
        }

        // Изменение значений характеристик заботы за время отсутствия игрока (в том числе сон)
        public void TimeActionStats(long timeDifference, int partyIndex, TeleportBeasts telep, DollScaleValues[] ds, AllDollSleeps allSleeps)
        {
            m_ScaleValues = ds[dollID];

            bool isSleeping = allSleeps.GetSleepingByID(m_Doll.DollID);

            if (isSleeping)
            {
                sleepSystem.GoToBed(partyIndex);
            }
            else
            {
                sleepSystem.WakeDoll(partyIndex);
            }

            if (sleepSystem.Sleeping)
            {
                sleepSystem.GoToBed(partyIndex);


                m_Doll.SetSleep(m_ScaleValues.Sleep + timeDifference);

                positionManager.WarpDoll(telep.Levels[m_LocationIndex].Beds[m_DollIndexInParty]);
                
            }

            else
            {
                m_Doll.SetSleep(m_ScaleValues.Sleep - timeDifference);
            }

            float poo = Mathf.Clamp(m_ScaleValues.LooPoo - timeDifference * Doll.StepLooStat, 0, Doll.MaxLooStat);

            float analSpray = Mathf.Clamp(m_ScaleValues.AnalSprayAmount +
                timeDifference * (Doll.StepAnalGlandSecretions * m_Doll.AnalGlandVolume), 0, m_Doll.AnalGlandVolume);

            float pee = Mathf.Clamp(m_ScaleValues.LooPee - timeDifference * Doll.StepLooStat, 0, Doll.MaxLooStat);

            float bath = Mathf.Clamp(m_ScaleValues.Bath - timeDifference * Doll.StepBath, 0, Doll.MaxBrushTeeth);
            float brushTeeth = Mathf.Clamp(m_ScaleValues.BrushTeeth - timeDifference * Doll.StepBrushTeeth, 0, Doll.MaxBrushTeeth);

            m_Doll.SetToiletStats(poo, 
                analSpray, 
                pee, 
                bath,
                brushTeeth);

            float food = Mathf.Clamp(m_ScaleValues.FoodHunger - timeDifference, 0, Doll.MaxStat);

            m_Doll.SetFoodHunger(food);
        }


        // Взять значения характеристик заботы, записанные в файле и попавшие в скрипт AllDollCharacters
        public void TakeStats(int partyIndex, AllDollCharacters allDoll, AllDollSleeps sleeps)
        {
            m_ScaleValues = allDoll.GetDoll(dollID);

            bool isSleeping = sleeps.GetSleepingByID(m_Doll.DollID);

            if (isSleeping)
            {
                sleepSystem.GoToBed(partyIndex);
            }
            else
            {
                sleepSystem.WakeDoll(partyIndex);
            }

            if (sleepSystem.Sleeping)
            {
                sleepSystem.GoToBed(partyIndex);
                m_Doll.SetSleep(m_ScaleValues.Sleep);
             /////   positionManager.WarpDoll(m_Level.Beds[dollID]);
            }

            else
            {
                print("Snegura");
                print("Snegura" + m_ScaleValues.Sleep);
                m_Doll.SetSleep(m_ScaleValues.Sleep);
                print("Snegure");
            }
            float poo = Mathf.Clamp(m_ScaleValues.LooPoo, 0, Doll.MaxLooStat);

            float analSpray = Mathf.Clamp(m_ScaleValues.AnalSprayAmount, 0, m_Doll.AnalGlandVolume);

            float pee = Mathf.Clamp(m_ScaleValues.LooPee, 0, Doll.MaxLooStat);

            float bath = Mathf.Clamp(m_ScaleValues.Bath, 0, Doll.MaxBrushTeeth);
            float brushTeeth = Mathf.Clamp(m_ScaleValues.BrushTeeth, 0, Doll.MaxBrushTeeth);

            m_Doll.SetToiletStats(poo,
                analSpray,
                pee,
                bath,
                brushTeeth);

            float food = Mathf.Clamp(m_ScaleValues.FoodHunger, 0, Doll.MaxStat);

            m_Doll.SetFoodHunger(food);
        }


        // Уложить питомца спать
        public void GoToBed()
        {
            sleepSystem.GoToBed(m_DollIndexInParty);

            m_Doll.GetComponent<GentAIConroller>().SleepPatrolBehaviour();
        }

        // Разбудить питомца
        public void WakeDoll()
        {
            if (!Sleeping) return;

            sleepSystem.WakeDoll(m_DollIndexInParty);

            if (!m_Doll.ActiveDollInPartyStatus)
                GetComponent<GentAIConroller>().WakePatrolBehaviour();
        }

        // Купание
        public void Wash()
        {
            bathSystem.Wash();
        }

        // Чистить зубы питомцу
        public void BrushTeeth()
        {
            bathSystem.BrushTeeth();
        }

        // Установить позицию из точки телепортации
        public void SetDollPosFromWaypoint(int loc, Vector3 waypoint, int index)
        {
            positionManager.SetDollPosFromWaypoint(loc, waypoint, index);
        }

        // Установить позицию по данным файла
        public void TakeAndSetDollPos(int loc, int index)
        {
            positionManager.TakeAndSetDollPos(loc, index);
        }

        public void ResetDollLocation()
        {
            positionManager.ResetLocation();
        }


      

        // Уменьшить значения характеристик со временем
        public void StatsReduce()
        {

            print("Drain");

            sleepSystem.ApplySleep();

            m_Doll.ReduceNonSleepStats();
        }



        // Пауза
        public void Pause()
        {
            m_Animator.enabled = false;
            m_Rigidbody.isKinematic = true;
        }


        // Возобновить
        public void UnPause()
        {
            m_Animator.enabled = true;
            m_Rigidbody.isKinematic = false;
        }



        // Карман
        public void NavelEffect(bool enab)
        {
            m_NavelEffect = enab;
            m_Doll.PetAsSpaceShip.NavelEffect(enab);
            if (enab)
            {
                if (m_NavelEffectShield != null) return;

                m_NavelEffectShield = Instantiate(m_NavelEffectPrefab, transform);
                m_NavelEffectShield.transform.SetParent(transform, false);

                m_Rigidbody.useGravity = false;
            }
            else
            {
                Destroy(m_NavelEffectShield);
                m_Rigidbody.useGravity = true;
            }
        }

    }
}

