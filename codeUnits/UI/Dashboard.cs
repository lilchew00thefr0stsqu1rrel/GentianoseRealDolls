using GentianoseRealDolls;
using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using static UnityEditor.Progress;

public enum UIType
{
    World,
    Inventory,
    Stove,
    Map,
    Shop
}

public class Dashboard : MonoBehaviour, IDashboard, IDollSettable
{
    ////[Inject]
    //public void Construct(CurrentSceneData obj)
    //{
    //    currentScene = obj;
    //}

    [SerializeField] private Party m_Party;
    [SerializeField] CurrentSceneData currentScene;
    [SerializeField] private HabitatInterface habitatUI;
    [SerializeField] private CombatDashboard combatUI;
    public CombatDashboard CombatUI => combatUI;
    [SerializeField] private InventoryDisplay inventoryDisplay;
    [SerializeField] private GameObject stoveUI;
    [SerializeField] private GameObject m_ShopDisplay;
    [SerializeField] private GameObject m_Map;

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


    public void Show(GameObject ui)
    {
        ui.SetActive(true);
    }
    public void Hide(GameObject ui)
    {
        ui.SetActive(false);
    }

    private IEnumerator UpdateUI()
    {
        yield return new WaitForSeconds(0.5f);

        habitatUI.UpdateDash();
        combatUI.UpdateDash();

        m_GaitDisplay.UpdateGaitDisplay(m_Party.GaitMap);

        StartCoroutine(UpdateUI());

        m_CurrentDoll = m_Party.ActiveDoll;
        m_CurrentDollController = m_Party.ActiveDollController;

        for (int i = 0; i < m_Party.PartyDollSleeps.Length; i++)
        {
            SetSleepDoll(i, m_Party.PartyDollSleeps[i]);
        }
    }

    public void InitDoll()
    {
        m_CurrentDoll = m_Party.ActiveDoll;
        m_CurrentDollController = m_Party.ActiveDoll.DollController;
        habitatUI.SetCurrentDoll(m_CurrentDoll);
        combatUI.SetDoll(m_CurrentDoll);

        for (int i = 0; i < m_Party.PartyDollSleeps.Length; i++)
        {
            SetSleepDoll(i, m_Party.PartyDollSleeps[i]);
        }
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

        StartCoroutine(UpdateUI());

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
    public void SetSleepDoll(int index, bool sleep)
    {
        m_DollSleepIndic[index].SetActive(sleep);
    }
    [SerializeField] int tipID = -1;

    private bool m_LoadReady;

    IEnumerator LoadAllWhoo()
    {
        yield return new WaitForSeconds(0.7f);
        m_LoadReady = true;
    }

    public void OnEscape()
    {
        if (uiType == UIType.World)
            OpenInventory();
        else
            CloseInventory();
    }

    public void OpenInventory()
    {
        habitatUI.HideAdditiveDashboard();
        Show(inventoryDisplay.gameObject);
        uiType = UIType.Inventory;
        m_Party.PauseAllDolls();
    }
    public void CloseInventory()
    {
        Hide(inventoryDisplay.gameObject);

        Hide(m_Map);
        Hide(stoveUI);
        Hide(m_ShopDisplay);
        
        uiType = UIType.World;
        m_Party.UnPauseAllDolls();
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
        habitatUI.SetCurrentDoll(doll);
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
    }

    public void SetSprayChargeUIVisible(bool visible)
    {
        combatUI.SetSprayChargeUIVisible(visible);
    }


    public void Eat(InventoryItem food)
    {
        m_CurrentDoll.Eat(food);
        InventoryController.Instance.InitAllItems();
    }
    public void ToMainMenu()
    {
        CloseInventory();

        SceneHelper.ToMainMenu();
    }

}
