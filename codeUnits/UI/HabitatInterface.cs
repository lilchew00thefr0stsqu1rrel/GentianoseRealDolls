using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GentianoseRealDolls
{
    public class HabitatInterface : MonoBehaviour
    {
        [SerializeField] private Doll m_CurrentDoll;
        [SerializeField] private DollController m_CurrentDollController;
        [SerializeField] private Text m_BathroomText;

        [SerializeField] private Text m_FoodHungerText;

        [SerializeField] private Text m_SleepText;
        [SerializeField] private Text m_JoyText;

        [SerializeField] private GameObject m_ToiletHint;


        [SerializeField] private Button m_GoPoopToSilverWhiteTree;


        GRDPlayer m_Player = Player.Instance as GRDPlayer;

        [SerializeField] private DollPoopManager m_PoopManager;

        [SerializeField] private GameObject m_ToiletDashboard;



        
        private void Awake()
        {
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        void Start()
        {

        }
        public void DashboardVisible()
        {
            m_ToiletDashboard.SetActive(!m_ToiletDashboard.activeSelf);
        }


        // Update is called once per frame
        void Update()
        {
            if (m_CurrentDoll != null)
            {
                m_BathroomText.text = ((int) m_CurrentDoll.Bathroom).ToString();
                m_FoodHungerText.text = ((int) m_CurrentDoll.FoodHunger).ToString();
                m_SleepText.text = ((int)m_CurrentDoll.Sleep).ToString();
                m_JoyText.text = ((int) m_CurrentDoll.Joy).ToString();

                //print(m_CurrentDoll.PooPoints);
                m_ToiletHint.SetActive(SarvaToilet.CanPoop && !m_CurrentDoll.DollController.PoopManager.IsPooping && 
                    m_CurrentDoll.PooPoints <= 7.7f);
            }

            UpdatePoop();

            if (Input.GetKeyDown(KeyCode.F4))
            {
                m_CurrentDoll.OhPoop();
            }

        }



        public Action<Doll> Link()
        {
            return (Doll d) =>
            {
                m_CurrentDoll = d;
            };
        }

        public void SetCurrentDoll(Doll d)
        {
            m_CurrentDoll = d;
            m_CurrentDollController = d.DollController;
            m_PoopManager = m_CurrentDoll.DollController.PoopManager;
        }


        public void UpdateShow()
        {
            gameObject.SetActive(SceneHelper.GameMode == Mode.Habitat);
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }

        public void Wake()
        {
            m_CurrentDoll.DollController.WakeDoll();
        }
        public void UpdatePoop()
        {
            if (!m_CurrentDoll) return;

            if (m_CurrentDoll.PooPoints <= 7.7f)
            {
                if (SarvaToilet.CanPoop && Input.GetKeyDown(KeyCode.R))
                {
                    m_PoopManager.ToPoop();
                }
            }

          

            if (Input.GetKeyDown(KeyCode.R))
            {
                m_PoopManager.OutPoop();
            }



            #region TimerCooldown_invent

            if (Input.GetKeyDown(KeyCode.T))
            {
                m_PoopManager.ToTwerk();
            }

            //if (addTime)
            //{
            //    timer += Time.deltaTime;
            //}

            //if (timer >= 0.2f)
            //{
            //    addTime = false;
            //}

            if (Input.GetKeyDown(KeyCode.T))
            {
                m_PoopManager.OutTwerk();
            }

            #endregion
            if (Input.GetMouseButtonDown(1))
            {
                m_PoopManager.ToLiftTail(); 
            }
            if (Input.GetMouseButtonDown(1))
            {
               m_PoopManager.OutLiftTail();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                m_PoopManager.ToPee();
            }



        }
        public void AdditiveDashboardChangeVisible()
        {
            m_ToiletDashboard.SetActive(!m_ToiletDashboard.activeSelf);
            print("Beisht");
        }

        public void HideAdditiveDashboard()
        {
            m_ToiletDashboard.SetActive(false);
        }

        public void SilverWhiteTree()
        {
            PoopStore.Instance.GoPoopToSilverWhiteTree();
            InventoryController.Instance.InitAllItems();
        }


    }

}

