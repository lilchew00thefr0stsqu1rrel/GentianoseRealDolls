
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GentianoseRealDolls
{
    public class Dashboard : SingletonBase<Dashboard>
    {
        [SerializeField] private HabitatInterface habitatUI;
        [SerializeField] private CombatDashboard combatUI;

        public event Action<UIType> ChangeWindow;

        private Dictionary<int, string> interactStrings;

        [SerializeField] private GaitDisplay m_GaitDisplay;
        [SerializeField] private GameObject m_Map;

        [SerializeField] private InventoryDisplay inventoryDisplay;

        [SerializeField] private GameObject interactTip;
        [SerializeField] private Text interactText;

        [SerializeField] private GiveResource m_ResourceTree;
        [SerializeField] private GameObject m_ShopDisplay;

        [SerializeField] private GameObject[] m_ActiveDollIndic;

        [SerializeField] private GameObject[] m_DollSleepIndic;

        [SerializeField] private Image m_StaminaImageFill;

        private Doll m_CurrentDoll;
        [SerializeField] private DollController m_CurrentDollController;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            interactStrings = new Dictionary<int, string>()
            {
                [0] = "Приготовить",
                [1] = "Сесть за стол",
                [2] = "Покинуть чалку",
                [3] = "Войти в чалку",
                [4] = "Спать",
                [5] = "Встать",
                [6] = "<предмет>",
                [7] = "Лавка мыши",
                [8] = "Купаться",
                [9] = "Чистить зубы",
            };

            //m_CurrentDoll = Party.Instance.ActiveDoll;

            StartCoroutine(LoadAllWhoo());


            inventoryDisplay.gameObject.SetActive(false);
            m_ShopDisplay.SetActive(false);
            stoveUI.SetActive(false);
            m_Map.SetActive(false);

            for (int i = 0; i < m_ActiveDollIndic.Length; i++)
            {
                m_ActiveDollIndic[i].SetActive(false);
            }

        }

        public void ShowActiveDoll(int index)
        {
            m_ActiveDollIndic[index].SetActive(true);

            for (int i = 0; i < m_ActiveDollIndic.Length; i++)
            {
                if (i != index)
                {
                    m_ActiveDollIndic[i].SetActive(false);
                }
            }
            
        }
        public void SetSleepDoll(int index, bool sleep)
        {
            m_DollSleepIndic[index].SetActive(sleep);

        }
        int tipID = -1;

        private bool m_LoadReady;

        IEnumerator LoadAllWhoo()
        {
            yield return new WaitForSeconds(0.7f);
            m_LoadReady = true;
        }

        public void OpenInventory()
        {
            habitatUI.HideAdditiveDashboard();
            inventoryDisplay.gameObject.SetActive(true);
            uiType = UIType.Inventory;
            Party.Instance.PauseAllDolls();
        }
        public void CloseInventory()
        {
            inventoryDisplay.gameObject.SetActive(false);
            uiType = UIType.World;
            Party.Instance.UnPauseAllDolls();
        }

        // Update is called once per frame
        void Update()
        {
            
            if (SceneHelper.GameMode == Mode.Habitat)
            {
              //  habitatUI.gameObject.SetActive(true);
                combatUI.gameObject.SetActive(false);
            }
            else
            {
                // habitatUI.gameObject.SetActive(false);
                combatUI.gameObject.SetActive(true);

            }




            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (uiType == UIType.World)
                {
                    OpenInventory();
                }
                else
                {
                    if (uiType == UIType.Inventory)
                    {
                        CloseInventory();
                    }
                    if (uiType == UIType.Map)
                    {
                        m_Map.SetActive(false);
                        uiType = UIType.World;
                    }
                }
                
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (tipID == 0)
                {
                    EnterStove();
                }
                if (tipID == 1)
                {
                    OpenInventory();
                }
                if (tipID == 2)
                {
                    //SceneHelper.ExitHouse();
                    DynamicObjects.Instance.Toggle(0, true);
                    HideInteractTip();
                }
                if (tipID == 3)
                {
                    SceneHelper.EnterHouse();
                    HideInteractTip();
                }
                if (tipID == 4)
                {
                    m_CurrentDollController.GoToBed();
                    HideInteractTip();
                }
                if (tipID == 5)
                {
                    m_CurrentDollController.WakeDoll();
                    HideInteractTip();
                }
                if (tipID == 6)
                {
                    m_ResourceTree.GiveResources();
                    HideInteractTip();
                }
                if (tipID == 7)
                {
                    m_ShopDisplay.SetActive(true);
                    HideInteractTip();
                }
                if (tipID == 8)
                {
                    BathInterface.Instance.Wash(m_CurrentDoll);
                }
                if (tipID == 9)
                {
                    BathInterface.Instance.BrushTeeth(m_CurrentDoll);
                }
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
              
                m_Map.SetActive(true);
                uiType = UIType.Map;
               
            }

            if (Party.Instance)
                m_StaminaImageFill.fillAmount = Party.Instance.Stamina / 37f;


        }
      
        public void Btn()
        {
            combatUI.RefreshCooldownButtonLesserSkill();
        }

       

        public void UpdateCooldown(float time)
        {
            combatUI.UpdateShowCooldownTime(time);
        }

        [SerializeField] private Camera m_Camera;
        public void SetDoll(Doll doll, Camera camera)
        {
            m_CurrentDoll = doll;
            m_CurrentDollController = doll.DollController;


            // if (SceneHelper.GameMode == Mode.Habitat)
            SetDollHabitat(doll);

            m_Camera = camera;

            if (SceneHelper.GameMode == Mode.OpenWorld)
                SetDollOpenWorld(doll, m_Camera);
        }
        public void SetDollHabitat(Doll doll)
        {
                habitatUI.SetCurrentDoll(doll);
        }
        public void SetDollOpenWorld(Doll doll, Camera camera)
        {
                combatUI.InitCurrentDollCombat(doll, Camera.main);
        }

        public void SetCamera(Camera cam)
        {
            print(cam);
            combatUI.SetCamera(cam);
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }

        [SerializeField] private GameObject stoveUI;

        public void EnterStove()
        {
            stoveUI.gameObject.SetActive(true);
            uiType = UIType.Stove;
        }


    
        private UIType uiType;
        public UIType dashboardUIType => uiType;
        public void SetUIType(UIType type)
        {
            uiType = type;
        }

        public void ShowInteractTip(int interactID)
        {
            tipID = interactID;
            interactText.text = interactStrings[tipID];
            interactTip.SetActive(true);
        }
        public void ShowInteractTip(int interactID, string itemName, GiveResource resTile)
        {
            tipID = interactID;
            interactText.text = itemName;
            interactTip.SetActive(true);
            m_ResourceTree = resTile;
        }
        public void HideInteractTip()
        {
            interactTip.SetActive(false);
        }

        

        public void Eat(InventoryItem food)
        {
            m_CurrentDoll.Eat(food);
            InventoryController.Instance.InitAllItems();
        }
        public void ToMainMenu()
        {
            inventoryDisplay.gameObject.SetActive(false);
            m_Map.SetActive(false);
            SceneHelper.ToMainMenu();
        }
    }
    public enum UIType
    {
        World,
        Inventory,
        Stove,
        Map,
        Shop
    }

    

    
}

