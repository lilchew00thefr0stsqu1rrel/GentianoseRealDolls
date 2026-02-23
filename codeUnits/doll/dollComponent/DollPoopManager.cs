using Common;
using SpaceShooter;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
        //[Serializable]
        //private class PoopPosition
        //{
        //    public float x, y, z;
            
        //    public PoopPosition(Vector3 position)
        //    {
        //        this.x = position.x;
        //        this.y = position.y;
        //        this.z = position.z;
        //    }

        //}

        ////private List<GameObject> m_PooList;
        //private List<PoopPosition> m_PooPositions;


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

            
                //m_ActiveDoll = transform.parent.GetComponent<Doll>();
            m_Anus = m_Doll.AnusNipplesTurret.transform;

            //foreach (var poop in m_PooList)
            //{
            //    var pos = poop.transform.position;
            //    m_PooPositions.Add(new PoopPosition(pos.x, pos.y, pos.z));
            //}
            ps = PoopStore.Instance;

            //m_Animator = GetComponentInChildren<Animator>();

        }

        public event Action OnPoopDeposit;

        PoopStore ps;

        public void SetPoopStore()
        {
            ps = PoopStore.Instance;
        }

        public void Poop()
        {
            if (m_Doll.PooPoints <= minPooPointsToPoop && t.IsFinished)
            {
                var pooSet = new List<GameObject>();


                poopNumber = 2  + (int)((minPooPointsToPoop - m_Doll.PooPoints) / 2.2f); 

                for (int i = 0; i < poopNumber; i++)
                {
                    var poop = Instantiate(m_PoopPrefab, m_Anus.position + new Vector3(0, i * poopOffset, 0), transform.rotation);
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

            if (Input.GetKeyUp(KeyCode.F3))
            {
                ToPoop();
            }
        }

        private float timer = 0;
        private bool addTime = false;

       // public bool OnPoop { get; internal set; }

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
                    m_Doll.CareToiletStat(ToiletStat.AnalSpray, -m_Doll.AnalGlandVolume / 37);
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

        public void UpdateCooldown(string inputType, float cooldownTime, Action actionStart, Action actionEnd)
        {
            if (timer == 0 && Input.GetKeyDown(KeyCode.T))
            {
                actionStart();
                addTime = true;
            }

            if (addTime)
            {
                timer += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.T) && timer >= 0.4f)
            {
                actionEnd();
                addTime = false;
                timer = 0;
            }
        }

        private void StartPosePoop()
        {
            print("5!!");
            FindFirstObjectByType<FollowCamera>().Turn(-1);

            m_Doll.State = 5;
            m_Animator.SetInteger("Autom", 5);
            m_Animator.SetBool("TailUp", true);

            //  OnPoop = true;
            print("st");
        }

        private void EndPosePoop()
        {
            print("5--");
            print(timer);
            FindFirstObjectByType<FollowCamera>().Turn(1);

            m_Doll.State = 0;

            m_Animator.SetInteger("Autom", 0);
            m_Animator.SetBool("TailUp", false);

            //OnPoop = false;
            print("end");
            PoopStore.SavePoop();
        }

        private void LiftTail()
        {
            
            FindFirstObjectByType<FollowCamera>().BirdEye();

            m_Doll.State = 6;

            //m_Animator.SetInteger("Autom", 6);

            m_Animator.SetTrigger("TailUp");

        }


        private void DownTail()
        {

            FindFirstObjectByType<FollowCamera>().ReBirdEye();

            m_Doll.State = 0;

            //m_Animator.SetInteger("Autom", 6);

            m_Animator.ResetTrigger("TailUp");

        }

        public void ToPoop()
        {
            if (!m_AfterTwerk)
            {
                StartPosePoop();
                m_IsPooping = true;
                t.Start(5);

                //m_ActiveDoll.StartPee();

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

        public void ToLiftTail()
        {
            if (!m_AfterLiftTail)
            {
                LiftTail();
                //  addTime = true;
                StartCoroutine(WaitLiftTail());
            }
           
        }

        public void OutLiftTail()
        {
            if (m_AfterLiftTail)
            {
                DownTail();


                m_AfterLiftTail = false;
            }
           
        }

        public void ToPee()
        {
            StartPee();
        }

        [SerializeField] private Turret m_PeeTurret;

        [SerializeField] private GameObject m_PeeSpotPrefab;

        public void StartPee()
        {
            m_Doll.Sounds[9].Play();
            m_PeeTurret.Fire();

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

