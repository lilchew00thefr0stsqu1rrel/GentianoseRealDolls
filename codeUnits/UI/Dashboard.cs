using GentianoseRealDolls;
using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIType
{
    World,
    Inventory,
    Stove,
    Map,
    Shop
}

public class Dashboard : MonoBehaviour, IDependency<FollowCamera>, IDependency<Party>, IDependency<CurrentScene>
{
    private Party m_Party;
    public void Construct(Party obj)
    {
        m_Party = obj;
    }
    public void Construct(FollowCamera obj)
    {
        m_Camera = obj.ProperCamera;
    }
    CurrentScene currentScene;
    public void Construct(CurrentScene obj)
    {
        currentScene = obj;
    }

    [SerializeField] private HabitatInterface habitatUI;
    [SerializeField] private CombatDashboard combatUI;
    [SerializeField] private InventoryDisplay inventoryDisplay;
    [SerializeField] private GameObject stoveUI;
    [SerializeField] private GameObject m_ShopDisplay;
    [SerializeField] private GameObject m_Map;

    public event Action<UIType> ChangeWindow;

    private Dictionary<int, string> interactStrings;

    [SerializeField] private GaitDisplay m_GaitDisplay;


    [SerializeField] private GameObject interactTip;
    public bool InteractTipActive => interactTip.activeSelf;
    [SerializeField] private Text interactText;

    [SerializeField] private GiveResource m_ResourceTree;

    [SerializeField] private GameObject[] m_ActiveDollIndic;

    [SerializeField] private GameObject[] m_DollSleepIndic;

    [SerializeField] private Image m_StaminaImageFill;

    private Doll m_CurrentDoll;
    [SerializeField] private DollController m_CurrentDollController;
    [SerializeField] private int m_ActiveDollIndexInParty;

    [SerializeField] private GameObject m_VirtualGamepad;


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
        }; StartCoroutine(LoadAllWhoo());


        inventoryDisplay.gameObject.SetActive(false);
        m_ShopDisplay.SetActive(false);
        stoveUI.SetActive(false);
        m_Map.SetActive(false);

        for (int i = 0; i < m_ActiveDollIndic.Length; i++)
        {
            m_ActiveDollIndic[i].SetActive(false);
        }

        m_Party.OnActiveDollChanged += ShowActiveDoll;

    }
    private void OnDestroy()
    {
        m_Party.OnActiveDollChanged -= ShowActiveDoll;
        Destroy(gameObject);
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
        m_Party.PauseAllDolls();
    }
    public void CloseInventory()
    {
        inventoryDisplay.gameObject.SetActive(false);

        m_Map.SetActive(false);
        uiType = UIType.World;
        m_Party.UnPauseAllDolls();


    } // Update is called once per frame

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
            HideInteractTip();
        }
        if (tipID == 3)
        {
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

        if (control.Control == ControlModeData.ControlMode.Keyboard)
        {
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
                Interact();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                ShowMap();
            }

        }




        if (m_Party)
            m_StaminaImageFill.fillAmount = m_Party.Stamina / 37f;
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
        CloseInventory();


        SceneHelper.ToMainMenu();
    }

}
  



