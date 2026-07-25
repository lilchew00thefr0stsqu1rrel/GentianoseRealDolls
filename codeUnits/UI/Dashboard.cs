using GentianoseRealDolls;
using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public enum UIType
{
    World,
    Inventory,
    Stove,
    Map,
    Shop
}

public class Dashboard : MonoBehaviour
{
    ////[Inject]
    //public void Construct(CurrentSceneData obj)
    //{
    //    currentScene = obj;
    //}

    [SerializeField] private Party m_Party;
    [SerializeField] private AllDollSleeps m_AllDollSleeps;
    [SerializeField] CurrentSceneData currentScene;
    [SerializeField] private HabitatInterface habitatUI;
    [SerializeField] private CombatDashboard combatUI;
    public CombatDashboard CombatUI => combatUI;
    [SerializeField] private InventoryDisplay inventoryDisplay;
    [SerializeField] private GameObject stoveUI;
    [SerializeField] private GameObject m_ShopDisplay;
    [SerializeField] private GameObject m_Map;
    [SerializeField] private GameObject m_VirtualJoystickForRotation;
    [SerializeField] private GameObject[] m_AllUI;
    [SerializeField] private SleepUI m_SleepUI;

    public event Action<UIType> ChangeWindow;

    private List<string> interactStrings;

    [SerializeField] private GaitDisplay m_GaitDisplay;


    [SerializeField] private GameObject interactTip;
    public bool InteractTipActive => interactTip.activeSelf;
    [SerializeField] private Text interactText;

    [SerializeField] private GiveResource m_ResourceTree;
    [SerializeField] private Mechanism m_Mechanism;

    [SerializeField] private GameObject[] m_ActiveDollIndic;

    [SerializeField] private GameObject[] m_DollSleepIndic;

    [SerializeField] private Image m_StaminaImageFill;

    private Doll m_CurrentDoll;
    [SerializeField] private DollController m_CurrentDollController;
    [SerializeField] private int m_ActiveDollIndexInParty;

    [SerializeField] private GameObject m_VirtualGamepad;

    [SerializeField] int tipID = -1;

    [SerializeField] private int tick;

    public void UpdateUI()
    {
        tick++;

        m_CurrentDoll = m_Party.ActiveDoll;
        if (m_CurrentDoll != null)
        {

            m_SleepUI.SetDoll(m_CurrentDoll);


            m_CurrentDollController = m_Party.ActiveDollController;

            habitatUI.SetDoll(m_CurrentDoll);
            combatUI.SetDoll(m_CurrentDoll);

            habitatUI.UpdateUI();
            combatUI.UpdateUI();

            m_GaitDisplay.UpdateGaitDisplay(m_Party.GaitMap);

            if (tick % 5 == 0)
            {
                var slp = m_AllDollSleeps.GetDolls();

                for (int i = 0; i < 3; i++)
                {
                    m_DollSleepIndic[i].SetActive(slp[i] == 1);
                }
            }
        }

        m_SleepUI.UpdateUI();
    }

    private IEnumerator UpdateUITick()
    {
        yield return new WaitForSeconds(0.1f);

        UpdateUI();

        StartCoroutine(UpdateUITick());

    }


    public void InitDoll()
    {
        m_CurrentDoll = m_Party.ActiveDoll;
        m_CurrentDollController = m_Party.ActiveDoll.DollController;
        habitatUI.SetDoll(m_CurrentDoll);
        combatUI.SetDoll(m_CurrentDoll);
    }

    Action<int, string, GiveResource> ShowInteract(int tipID, string itemName, GiveResource resource)
    {
        return (tipID, itemName, resource) =>
        {
            ShowInteractTip(tipID, itemName, resource);
        };
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GiveResource.OnWentToResource += ShowInteract(tipID, "+", m_ResourceTree); 
        GiveResource.OnLeaveResource += () => { HideInteractTip(); }; 

        interactStrings = new List<string>()
        {            
            "Приготовить",
            "Сесть за стол",
            "Покинуть чалку",
            "Войти в чалку",
            "Спать",
            "Встать",
            "<предмет>",
            "Лавка мыши",
            "Купаться",
            "Чистить зубы",
            "Какать"
        }; 
        
        StartCoroutine(LoadAllWhoo());


        inventoryDisplay.gameObject.SetActive(false);
        m_ShopDisplay.SetActive(false);
        stoveUI.SetActive(false);
        m_Map.SetActive(false);

        for (int i = 0; i < m_ActiveDollIndic.Length; i++)
        {
            m_ActiveDollIndic[i].SetActive(false);
        }

        m_Party.OnActiveDollChanged += ShowActiveDoll;

        StartCoroutine(UpdateUITick());

    }
    private void OnDestroy()
    {
        m_Party.OnActiveDollChanged -= ShowActiveDoll;
        Destroy(gameObject);
        GiveResource.OnWentToResource -= ShowInteract(tipID, "+", m_ResourceTree);
    }

    public void ShowActiveDoll(int index)
    {
        print("Eija");
        for (int i = 0; i < m_ActiveDollIndic.Length; i++)
        {
            m_ActiveDollIndic[i].SetActive(false);
        }
        m_ActiveDollIndic[index].SetActive(true);
    }

    private bool m_LoadReady;

    IEnumerator LoadAllWhoo()
    {
        yield return new WaitForSeconds(0.7f);
        m_LoadReady = true;
    }

    public void OnEscape()
    {
        if (m_CurrentUI == -1)
            OpenInventory();
        else
            CloseInventory();
    }

    public void OpenInventory()
    {

        SetUI(1);
        //m_VirtualJoystickForRotation.SetActive(false);

        habitatUI.HideAdditiveDashboard();
        m_Party.PauseAllDolls();
    }
    public void CloseInventory()
    {

        //m_VirtualJoystickForRotation.SetActive(true);
        
        m_Party.UnPauseAllDolls();

        SetUI(0);
    } 

    public void ShowMap()
    {
        m_Map.SetActive(true);
        uiType = UIType.Map;
    }
    public void Interact()
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
            m_Mechanism.Activate();
            HideInteractTip();
        }
        if (tipID == 3)
        {
            m_Mechanism.Activate();
            HideInteractTip();
        }
        if (tipID == 4)
        {
            m_CurrentDollController.GoToBed(true);
            HideInteractTip();
        }
        if (tipID == 5)
        {
            m_CurrentDollController.GoToBed(false);
            HideInteractTip();
        }
        if (tipID == 6)
        {
            m_ResourceTree.GiveResources();
            //HideInteractTip();
        }
        if (tipID == 7)
        {
            ShowShopUI();
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
        if (tipID == 10)
        {
            m_Party.ActiveDoll.DollController.PoopManager.ToPoop();
        }
    }

    [SerializeField] private ControlModeData control;

    void Update()
    {
        if (currentScene.GameMode == Mode.Habitat)
        {
            combatUI.gameObject.SetActive(false);
        }
        else
        {
            combatUI.gameObject.SetActive(true);
        }

        if (m_Party)
            m_StaminaImageFill.fillAmount = m_Party.Stamina / 37f;
    }
    public void Btn()
    {
        combatUI.RefreshCooldownButtonLesserSkill();
    }

    public void ShowShopUI()
    {
        uiType = UIType.Shop;
        m_ShopDisplay.SetActive(true);
    }

    public void SetUI(int uiID)
    {
        m_CurrentUI = uiID;


        for (int i = 0; i < m_AllUI.Length; i++)
        {
             m_AllUI[i].gameObject.SetActive(false);
        }

        if (uiID > 0 && m_AllUI[uiID - 1] != null)
        {
            m_AllUI[uiID - 1].gameObject.SetActive(true);
        }

    }

    public void UpdateCooldown(float time)
    {
        combatUI.UpdateShowCooldownTime(time);
    }

    [SerializeField] private Camera m_Camera;
    public void SetDoll(Doll doll)
    {
        m_CurrentDoll = doll;

        m_CurrentDollController = doll.DollController;

        SetDollHabitat(doll);


        if (currentScene.GameMode == Mode.OpenWorld)
        {
            SetDollOpenWorld(doll);
            SetDollOpenWorldCamera();
        }
    }
    public void SetDollHabitat(Doll doll)
    {
        habitatUI.SetDoll(doll);
    }
    public void SetDollOpenWorld(Doll doll)
    {
        combatUI.InitCurrentDollCombat(doll);
    }
    public void SetDollOpenWorldCamera()
    {
        combatUI.InitCurrentDollCamera(m_Camera);
    }

    public void SetCamera(Camera cam)
    {
        print(cam);
        combatUI.SetCamera(cam);
    }

    public void EnterStove()
    {
        stoveUI.gameObject.SetActive(true);
        uiType = UIType.Stove;
    }



    private UIType uiType;

    [SerializeField] private int m_CurrentUI;
    public UIType dashboardUIType => uiType;
    public void SetUIType(UIType type)
    {
        uiType = type;
    }

    public void ShowInteractTip(int interactID)
    {
        print("Interact!!");
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
    public void ShowInteractTip(int interactID, Mechanism mechanism)
    {
        tipID = interactID;
        interactText.text = interactStrings[tipID];
        interactTip.SetActive(true);
        m_Mechanism = mechanism;
    }
    public void HideInteractTip()
    {
        interactTip.SetActive(false);
        tipID = -1;
    }

    public void SetSprayChargeUIVisible(bool visible)
    {
        combatUI.SetSprayChargeUIVisible(visible);
    }


    public void Eat(InventoryItem food)
    {
        m_CurrentDoll.DollController.FoodManager.Eat(food);
        InventoryController.Instance.InitAllItems();
    }

    public void ToMainMenu()
    {
        CloseInventory();

        SceneHelper.ToMainMenu();
    }

}
