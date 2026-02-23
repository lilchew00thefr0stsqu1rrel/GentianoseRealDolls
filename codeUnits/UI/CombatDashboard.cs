using GentianoseRealDolls;
using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;

public class CombatDashboard : MonoBehaviour
{
    const float keyCooldownDuration = 0.3f;
    [SerializeField] private Image m_HPFill;
    [SerializeField] private Text m_HPText;
    [SerializeField] private Text m_MaxHPText;

    [SerializeField] private Doll m_CurrentDoll;
    private DollBattleManager m_DollBattleManager;

    [SerializeField] private Image m_SprayChargeImage;
    [SerializeField] private GameObject m_SprayModeButton;
    [SerializeField] private GameObject m_SprayButton;
    [SerializeField] private GameObject m_SprayModeOffButton;

    [SerializeField] private GameObject m_SprayChargeUI;

    [SerializeField] private SprayFeedback m_SprayUI;

    [SerializeField] private SprayFeedback m_ToSprayUI;
    [SerializeField] private Text m_LesserSkillCooldownText;
    private void Awake()
    {

    }
    [SerializeField] private Camera m_Camera;

    public void InitCurrentDollCombat(Doll doll, Camera camera)
    {
        m_CurrentDoll = doll;
        m_DollBattleManager = m_CurrentDoll.DollController.BattleManager;
        m_Camera = camera;
        m_DollBattleManager.AssignTurretCamera(m_Camera);


        if (m_DollBattleManager != null)
        {
            m_ToSprayUI.InitDollSpray(m_CurrentDoll);

            m_DollBattleManager.OnTakeSprayStance += () =>
            {
                m_SprayModeButton.SetActive(false);
                m_SprayButton.SetActive(true);
                m_SprayModeOffButton.SetActive(true);


                m_SprayUI.InitDollSpray(m_CurrentDoll);
            };

            m_DollBattleManager.OnEndSprayStance += () =>
            {
                m_SprayButton.SetActive(false);
                m_SprayModeOffButton.SetActive(false);
                m_SprayModeButton.SetActive(true);

                m_ToSprayUI.InitDollSpray(m_CurrentDoll);
            };

            m_FlehmenButton.interactable = !m_DollBattleManager.FlehmenCooldown;
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (m_DollBattleManager != null)
        {
            m_DollBattleManager.OnTakeSprayStance += () =>
        {
            m_SprayModeButton.SetActive(false);
            m_SprayButton.SetActive(true);
            m_SprayModeOffButton.SetActive(true);
        };

            m_DollBattleManager.OnEndSprayStance += () =>
            {
                m_SprayButton.SetActive(false);
                m_SprayModeOffButton.SetActive(false);
                m_SprayModeButton.SetActive(true);
            };
        }

        m_LesserSkillCooldownText.gameObject.SetActive(false);
        // m_PetAsSpaceShip = m_CurrentDoll.PetAsSpaceShip;        
    }
    SpaceShip m_PetAsSpaceShip;
    // Update is called once per frame
    void Update()
    {
        if (m_CurrentDoll != null && m_DollBattleManager != null)
        {

            m_HPFill.fillAmount = (float)m_CurrentDoll.PetAsSpaceShip.HitPoints / m_CurrentDoll.PetAsSpaceShip.MaxHitPoints;
            m_HPText.text = m_CurrentDoll.PetAsSpaceShip.HitPoints.ToString();
            m_MaxHPText.text = $"/ {m_CurrentDoll.PetAsSpaceShip.MaxHitPoints}";

            m_SprayChargeImage.fillAmount = m_DollBattleManager.SprayChargeAmount;


            if (Input.GetKeyDown(KeyCode.E))
            {
                Flehmen();
            }

            // Spray.
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartSpray();
                print("dolldollws");
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                // With or without spray
                EndSpray();
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                print("**");
                if (timerU == 0)
                {
                    PrepareSpray();
                    keyCooldown = true;
                }
                if (timerU >= keyCooldownDuration)
                {
                    OutSpray();
                    timerU = 0;
                }
            }

            if (keyCooldown)
            {
                timerU += Time.deltaTime;
                if (timerU >= keyCooldownDuration)
                {
                    keyCooldown = false;
                }
            }


        }
    }



    private float timerU = 0;
    private bool keyCooldown = false;
    [SerializeField] private Button m_FlehmenButton;

    public void SetDoll(Doll doll)
    {
        m_CurrentDoll = doll;

    }

    private bool m_FlehmenOnCooldown;

    public void RefreshCooldownButtonLesserSkill()
    {
        m_FlehmenButton.interactable = true;
        m_LesserSkillCooldownText.gameObject.SetActive(false);
    }

    public void Flehmen()
    {
        //IEnumerator FlehmenSkill()
        //{
        //    print("Flehmen at CD");
        //    m_FlehmenOnCooldown = true;
        //    m_FlehmenButton.interactable = false;

        //    m_DollBattleManager.SetFlehmenCooldown();
        //    yield return new WaitForSeconds(m_DollBattleManager.Cooldown);
        //    m_FlehmenOnCooldown = false;
        //    m_FlehmenButton.interactable = true;

        //    print("Flehmen free");
        //};

        if (!m_FlehmenOnCooldown)
        {

            m_DollBattleManager.LesserSkill();
            m_LesserSkillCooldownText.gameObject.SetActive(true);
        }


        m_FlehmenButton.interactable = false;


        //StartCoroutine(FlehmenSkill());

    }


    public void PrepareSpray()
    {
         m_DollBattleManager.EnterSprayMode();
    }

    public void Idle()
    {
        m_DollBattleManager.Idle();
    }

   
    public void StartSpray()
    {
        m_DollBattleManager.StartGreaterSkill();
        m_SprayChargeUI.SetActive(true);
    }
    public void EndSpray()
    {
        m_DollBattleManager.EndGreaterSkill(); 
        m_SprayChargeUI.SetActive(false);
    }
    public void OutSpray()
    {
        m_DollBattleManager.ExitSprayMode();
    }
    public void StartAttack()
    {
        m_DollBattleManager.StartAttack();
    }
    public void EndAttack()
    {
        m_DollBattleManager.EndAttack();
    }

    public void SetBM(DollBattleManager battleManager)
    {
        m_DollBattleManager = battleManager;
    }

    public void SetCamera(Camera camera)
    {

        m_DollBattleManager.AssignTurretCamera(camera);
        print(camera != null);
    }

    public void UpdateShowCooldownTime(float time)
    {
        m_LesserSkillCooldownText.text = time.ToString("f1");
    }
}
