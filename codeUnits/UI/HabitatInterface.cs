using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GentianoseRealDolls
{
    public class HabitatInterface : MonoBehaviour
    {
        [SerializeField] private Party m_Party;
        ////[Inject]
        //public void Construct(Party obj)
        //{
        //    m_Party = obj;
        //}
        [SerializeField] CurrentSceneData currentScene;
        //[Inject]
        //public void Construct(CurrentSceneData obj)
        //{
        //    currentScene = obj;
        //}

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


        [SerializeField] private Text m_DollPitchText;
        
        private void Awake()
        {
        }

        public void UpdateDash(Doll activeDoll)
        {
            if (m_Party == null) 
            {
                print("NO PArty");
                return;
            }


            if (m_Party.ActiveDoll == null)
            {
                print("NO ACTIVEDOLL");
                return;
            }

            m_FoodHungerText.text = m_Party.ActiveDoll.FoodHunger.ToString();
            m_BathroomText.text = m_Party.ActiveDoll.Bathroom.ToString("F0");
            m_SleepText.text = m_Party.ActiveDoll.Sleep.ToString();
            
            SetCurrentDoll(activeDoll);
        }

        public void StartPoop()
        {
            m_PoopManager.ToPoop();
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
            if (m_CurrentDoll)
                print("Doll bathroom "+ m_CurrentDoll.Bathroom);

            if (m_CurrentDoll != null)
            {
                m_BathroomText.text = ((int) m_CurrentDoll.Bathroom).ToString();
                m_FoodHungerText.text = ((int) m_CurrentDoll.FoodHunger).ToString();
                m_SleepText.text = ((int)m_CurrentDoll.Sleep).ToString();
                m_JoyText.text = ((int) m_CurrentDoll.Joy).ToString();

                //print(m_CurrentDoll.PooPoints);
                m_ToiletHint.SetActive(SarvaToilet.CanPoop && !m_CurrentDoll.DollController.PoopManager.IsPooping && 
                    m_CurrentDoll.PooPoints <= 7.7f);


                m_DollPitchText.text = m_CurrentDoll.DollController.Climbing.HillAngle.ToString();
            }

            UpdatePoop();

            //if (Input.GetKeyDown(KeyCode.F4))
            //{
            //    m_CurrentDoll.OhPoop();
            //}

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
            gameObject.SetActive(currentScene.GameMode == Mode.Habitat);
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
        [SerializeField] private PoopStore m_PoopStore;

        public void SilverWhiteTree()
        {
            m_PoopStore.GoPoopToSilverWhiteTree();
            InventoryController.Instance.InitAllItems();
        }


    }

}

