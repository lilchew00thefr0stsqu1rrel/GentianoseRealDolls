using Common;
using SpaceShooter;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using NTC.Pool;

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

        //private PoopPosition[] m_PooPosArray = new PoopPosition[31];
        private float poopOffset = -0.013f;
        private int poopNumber = 5;

        private GRDTimer t;

       // [SerializeField] private Doll m_ActiveDoll;

        [SerializeField] private Poop m_PoopPrefab;
        private Transform m_Anus;
      //  [SerializeField] private Animator m_Animator;


        bool m_IsPooping = false;
        public bool IsPooping => m_IsPooping;

        private float minPooPointsToPoop = 6.6f;


        private void Start()
        {
            t = new GRDTimer(5);
        }

        public event Action OnPoopDeposit;

        [SerializeField] private PoopStore ps;

        public void SetPoopStore(PoopStore poopStore)
        {
            ps = poopStore;
        }

        public void Poop()
        {
            if (m_Doll.PooPoints <= minPooPointsToPoop && t.IsFinished)
            {
                var pooSet = new List<GameObject>();


                poopNumber = 2  + (int)((minPooPointsToPoop - m_Doll.PooPoints) / 2.2f); 

                for (int i = 0; i < poopNumber; i++)
                {
                    var poop = NightPool.Spawn(m_PoopPrefab, 
                        m_AnusTurret.transform.position + new Vector3(0, i * poopOffset, 0), transform.rotation);
                    poop.GetComponent<Poop>().InitPoop(m_Doll.Asset);
                    ps.AddPoop(poop);
                }
                m_Doll.SetMaxPooCare();
               // OnPoopDeposit();
                 
                
            }
        }

        private void Update()
        {
            UpdatePoop();
        }

        private float timer = 0;
        private bool addTime = false;


        IEnumerator WaitEndPooPee()
        {
            yield return new WaitForSeconds(6);

            EndPosePoop();
        }

        public void UpdatePoop()
        {
            

            if (m_IsPooping)
            {
                t?.RemoveTime(Time.deltaTime);
                if (t.IsFinished)
                {
                    Poop();

                    // Выделяем фуньку на каку
                    m_Doll.CareToiletStat(ToiletStat.AnalSpray, -m_Doll.AnalGlandVolume / 37.0f);
                    m_IsPooping = false;
                    EndPee();
                    addTime = true;

                    StartCoroutine(WaitEndPooPee());
                }
            }

            
        }
        int count = 0;
        IEnumerator BurstPee()
        {
            StartPee();
            count++;
            yield return new WaitForSeconds(1);
            if (count < 8)
            StartCoroutine(BurstPee());
        }

        private bool m_AfterTwerk;
        private bool m_AfterLiftTail;
        IEnumerator WaitTwerk()
        {
            m_AfterLiftTail = false;
            yield return new WaitForSeconds(0.2f);
            m_AfterTwerk = true;
        }
        IEnumerator WaitLiftTail()
        {
            m_AfterLiftTail = false;
            yield return new WaitForSeconds(0.2f);
            m_AfterLiftTail = true;
        }
        

        private void StartPosePoop()
        {
            print("5!!");
            FindFirstObjectByType<FollowCamera>().Turn(-1);

            m_Doll.State = 5;
            m_AnimatorGuard.SetAnimation(5);
           // m_Animator.SetBool("TailUp", true);

            //  OnPoop = true;
            print("st");
        }

        private void EndPosePoop()
        {
            print("5--");
            print(timer);
            FindFirstObjectByType<FollowCamera>().Turn(1);

            m_Doll.State = 0;

            m_AnimatorGuard.SetAnimation(0);

            print("end");
            PoopStore.SavePoop();
        }

        private void LiftTail()
        {
            FindFirstObjectByType<FollowCamera>().BirdEye();

            m_Doll.State = 6;
        }


        private void DownTail()
        {
            FindFirstObjectByType<FollowCamera>().ReBirdEye();

            m_Doll.State = 0;
        }

        public void ToPoop()
        {
            if (!m_AfterTwerk)
            {
                StartPosePoop();
                m_IsPooping = true;
                t.Start(5);

                StartCoroutine(BurstPee());

                StartCoroutine(WaitTwerk());
            }
            
        }

        public void OutPoop()
        {
            if (m_AfterTwerk)
            {
                EndPosePoop();
                addTime = false;
                timer = 0;

                m_AfterTwerk = false;
            }
           
        }
       
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

    }
}

