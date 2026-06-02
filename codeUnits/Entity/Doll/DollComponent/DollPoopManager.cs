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

        private float minPooPointsToPoop = 6.6f;

        private float timer = 0;
        private bool addTime = false;

        int count = 0;
        private bool m_AfterTwerk;
        private bool m_AfterLiftTail;
        private void Start()
        {
            t = new GRDTimer(5);
        }


        #region Poop API
        public async void ToPoop()
        {
            StartPosePoop();
            await Task.Delay(5000);
            if (m_Doll.PooPoints <= minPooPointsToPoop)
            {
                poopNumber = 2 + (int)(Mathf.Ceil(minPooPointsToPoop - m_Doll.PooPoints) / 2.2f);
                for (int i = 0; i < poopNumber; i++)
                {
                    Poop();
                    StartPee();
                    await Task.Delay(1000);
                }
                EndPosePoop();
            }
            else
            {
                EndPosePoop() ;
            }
        }

        public void OutPoop()
        {
            EndPosePoop();
        }



        public void Poop()
        {
            var poop = NightPool.Spawn(m_PoopPrefab, 
            m_AnusTurret.transform.position, transform.rotation);
            poop.GetComponent<Poop>().InitPoop(m_Doll.Asset);
            m_PoopStore.AddPoop(poop);
            m_PoopStore.SavePoop();
            m_Doll.CareToiletStat(ToiletStat.Poo, 2.2f);
            m_Doll.CareToiletStat(ToiletStat.Pee, 2.2f);
            m_Doll.CareToiletStat(ToiletStat.AnalSpray, m_Doll.AnalGlandVolume * 0.037f);
        }


        

        private void StartPosePoop()
        {
            print("5!!");
            FindAnyObjectByType<FollowCamera>().Turn(-1);

            m_Doll.State = 5;
            m_AnimatorGuard.SetAnimation(5);
            print("st");
        }

        private void EndPosePoop()
        {
            print("5--");
            print(timer);
            FindAnyObjectByType<FollowCamera>().Turn(1);

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
            m_Doll.Sounds[9].Play();
            m_PeeTurret.Fire(Vector2.zero);

            RaycastHit[] hit = Physics.RaycastAll(m_PeeTurret.transform.position, m_PeeTurret.transform.forward, 0.3f);

            if (hit != null)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider.transform.root.GetComponent<Doll>() == null)
                    {
                        Instantiate(m_PeeSpotPrefab, hit[i].point, transform.rotation);
                    }
                }
            }
        }

        public void EndPee()
        {
            m_Doll.CareToiletStat(ToiletStat.Pee, Doll.MaxLooStat);
        }
        #endregion

    }
}

