using Common;
using SpaceShooter;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using NTC.Pool;
using System.Threading.Tasks;

namespace GentianoseRealDolls
{


//    [RequireComponent(typeof(Doll))]

    /// <summary>
    /// Зверёк ходит в туалет
    /// В данной вселенной приматы, отличные от человека
    /// тоже ходят в уборную в определённом месте
    /// </summary>
    public class DollPoopManager : DollComponent
    {

        [SerializeField] private Turret m_AnusTurret;

        [SerializeField] private Turret m_PeeTurret;

        [SerializeField] private GameObject m_PeeSpotPrefab;

        [SerializeField] private PoopStore m_PoopStore;

        [SerializeField] private Poop m_PoopPrefab;

        [SerializeField] private bool m_InPoop;
        public void ConstructPoopStorage(PoopStore poopStore)
        {
            m_PoopStore = poopStore;
        }
        public event Action OnPoopDeposit;
        //private PoopPosition[] m_PooPosArray = new PoopPosition[31];
        private float poopOffset = -0.013f;
        private int poopNumber = 5;

        private GRDTimer t;

       // [SerializeField] private Doll m_ActiveDoll;

        private Transform m_Anus;
      //  [SerializeField] private Animator m_Animator;


        bool m_IsPooping = false;
        public bool IsPooping => m_IsPooping;

        private int maxPooPointsToPoop = 6;

        private float timer = 0;
        private bool addTime = false;

        int count = 0;
        private bool m_AfterTwerk;
        private bool m_AfterLiftTail;

        private int m_PrepareToPoopTime = 1500;
        private int m_PoopIntervTime = 500;
        
        private void Start()
        {
            t = new GRDTimer(5);
        }


        #region Poop API
        public async void ToPoop()
        {
            if (!m_InPoop)
            {
                poopNumber = (Doll.MaxLooStat - m_Doll.PooPoints) / 2;

                StartPosePoop();

                m_InPoop = true;

                await Task.Delay(m_PrepareToPoopTime);

                for (int i = 0; i < poopNumber; i++)
                {
                    await Task.Delay(m_PoopIntervTime);
                    print(i + "Poo~");
                    Poop();
                    StartPee();
                }
                EndPosePoop();

                m_InPoop = false;

            }
        }

        public void OutPoop()
        {
            EndPosePoop();
        }



        public void Poop()
        {
            print("Pooey~");
            var poop = NightPool.Spawn(m_PoopPrefab, 
            m_AnusTurret.transform.position, transform.rotation);
            // poop.InitPoop(m_Doll.Asset);
            m_PoopStore.AddPoop(poop);
            m_Doll.CareToiletStat(ToiletStat.Poo, 2);
            m_Doll.CareToiletStat(ToiletStat.AnalSpray, 1);
        }


        

        private void StartPosePoop()
        {
            print("5!!");
            FindAnyObjectByType<CameraAroundDoll>().Turn();

            m_Doll.State = 5;
            m_AnimatorGuard.SetAnimation(5);
            print("st");
        }

        private void EndPosePoop()
        {
            m_PoopStore.SavePoop();

            print("5--");
            print(timer);
            FindAnyObjectByType<CameraAroundDoll>().Turn();

            m_Doll.State = 0;

            m_AnimatorGuard.SetAnimation(0);

            print("end");
        }

        #endregion

        #region Coroutines
        IEnumerator WaitEndPooPee()
        {
            yield return new WaitForSeconds(6);

            EndPosePoop();
        }

        IEnumerator WaitTwerk()
        {
            m_AfterLiftTail = false;
            yield return new WaitForSeconds(0.2f);
            m_AfterTwerk = true;
        }
        #endregion

        #region Motions
        public void ToTwerk()
        {
            if (!m_AfterTwerk)
            {
                StartPosePoop();

                StartCoroutine(WaitTwerk());
            }
          
        }

        public void OutTwerk()
        {
            if (m_AfterTwerk)
            {
                EndPosePoop();
                timer = 0;

                m_AfterTwerk = false;
            }
           
        }
        #endregion

        #region Pee

        public void ToPee()
        {
            StartPee();
        }

        public void StartPee()
        {
            m_Doll.Sounds[9]?.Play();
            m_PeeTurret.Fire(Vector2.zero);
            m_Doll.CareToiletStat(ToiletStat.Pee, 2);

            //RaycastHit[] hit = Physics.RaycastAll(m_PeeTurret.transform.position, m_PeeTurret.transform.forward, 0.3f);

            //if (hit != null)
            //{
            //    for (int i = 0; i < hit.Length; i++)
            //    {
            //        if (hit[i].collider.transform.root.GetComponent<Doll>() == null)
            //        {
            //            Instantiate(m_PeeSpotPrefab, hit[i].point, transform.rotation);
            //        }
            //    }
            //}
        }

        public void EndPee()
        {
            m_Doll.CareToiletStat(ToiletStat.Pee, Doll.MaxLooStat);
        }
        #endregion

    }
}


