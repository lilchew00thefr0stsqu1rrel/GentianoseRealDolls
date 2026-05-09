using GentianoseRealDolls;
using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;
using NTC.MonoCache;
using Unity.VisualScripting;
using System;

public class CombatDashboard : MonoCache
{
    const float keyCooldownDuration = 0.3f;
    

    [SerializeField] private Image m_HPFill;
    [SerializeField] private Text m_HPText;
    [SerializeField] private Text m_MaxHPText;

    [SerializeField] private Doll m_CurrentDoll;
    [SerializeField] private DollAsset m_CurrentDollAsset;
    private DollBattleManager m_DollBattleManager;

    [SerializeField] private Image m_SprayChargeImage;
    [SerializeField] private GameObject m_SprayModeButton;
    [SerializeField] private GameObject m_SprayButton;
    [SerializeField] private GameObject m_SprayModeOffButton;


    [SerializeField] private GameObject m_SprayChargeUI;

    [SerializeField] private SprayFeedback m_SprayUI;

    [SerializeField] private SprayFeedback m_ToSprayUI;
    [SerializeField] private Text m_LesserSkillCooldownText;

    [SerializeField] private Party m_Party;

    private void Awake()
    {
    }
    [SerializeField] private Camera m_Camera;

    public void InitCurrentDollCombat(Doll doll)
    {
        if (m_CurrentDoll)
        {
            EndDoll(m_CurrentDoll);
        }

        m_CurrentDoll = doll;
        m_DollBattleManager = m_CurrentDoll.DollController.BattleManager;

        StartDoll(doll);

        if (m_DollBattleManager != null)
        {

            m_FlehmenButton.SetInteractable(!m_DollBattleManager.FlehmenCooldown);
        }

    }
    public void InitCurrentDollCamera(Camera camera)
    {
        m_Camera = camera;
        m_DollBattleManager.AssignTurretCamera(m_Camera);

        m_ToSprayUI.InitDollSpray(m_CurrentDoll);



    }

    [SerializeField] private UIHoldableButton m_NormalAttackButtonScreen;
    public UIHoldableButton NormalAttackButtonScreen => m_NormalAttackButtonScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_SprayButton.SetActive(false);
        m_SprayModeOffButton.SetActive(false);
        m_SprayModeButton.SetActive(true);  
        
    }
    public void SetEmptyScreenActiveAsNA(bool interactable)
    {
        m_NormalAttackButtonScreen.gameObject.SetActive(interactable);
    }

    

    private void StartDoll(Doll doll)
    {
        m_DollBattleManager = doll.DollController.BattleManager;
        if (m_DollBattleManager != null)
        {
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
        }
        m_LesserSkillCooldownText.gameObject.SetActive(false);


    }

    private void EndDoll(Doll doll)
    {
        m_DollBattleManager = doll.DollController.BattleManager;
        if (m_DollBattleManager != null)
        {
            m_DollBattleManager.OnTakeSprayStance -= () =>
            {
                m_SprayModeButton.SetActive(false);
                m_SprayButton.SetActive(true);
                m_SprayModeOffButton.SetActive(true);

                m_SprayUI.InitDollSpray(m_CurrentDoll);
            };

            m_DollBattleManager.OnEndSprayStance -= () =>
            {
                m_SprayButton.SetActive(false);
                m_SprayModeOffButton.SetActive(false);
                m_SprayModeButton.SetActive(true);


                m_ToSprayUI.InitDollSpray(m_CurrentDoll);
            };
        }
        m_LesserSkillCooldownText.gameObject.SetActive(false);
    }

    SpaceShip m_PetAsSpaceShip;
    // Update is called once per frame
    protected override void Run()
    {
        if (m_CurrentDoll != null && m_DollBattleManager != null)
        {

            m_HPFill.fillAmount = (float)m_CurrentDoll.PetAsSpaceShip.HitPoints / m_CurrentDoll.PetAsSpaceShip.MaxHitPoints;
            m_HPText.text = m_CurrentDoll.PetAsSpaceShip.HitPoints.ToString();
            m_MaxHPText.text = $"/ {m_CurrentDoll.PetAsSpaceShip.MaxHitPoints}";

            m_SprayChargeImage.fillAmount = m_DollBattleManager.SprayChargeAmount;



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
    [SerializeField] private UIButton m_FlehmenButton;

    public void SetDoll(Doll doll)
    {
        m_CurrentDoll = doll;

    }

    private bool m_FlehmenOnCooldown;

    public void RefreshCooldownButtonLesserSkill()
    {
        m_FlehmenButton.SetInteractable(true);
        m_LesserSkillCooldownText.gameObject.SetActive(false);
    }

    public void StartAttack()
    {
        m_DollBattleManager.StartAttack();
    }
    public void EndAttack()
    {
        m_DollBattleManager.EndAttack(m_AimInput);
    }

    public void Flehmen()
    {
        if (!m_FlehmenOnCooldown)
        {
            m_DollBattleManager.LesserSkill();
            m_LesserSkillCooldownText.gameObject.SetActive(true);
        }

        m_FlehmenButton.SetInteractable(false);
    }


    public void SprayStanceOnOff()
    {
        m_DollBattleManager.SprayModeOnOff();

        m_SprayModeButton.SetActive(!m_SprayModeButton.activeSelf);
        m_SprayButton.SetActive(!m_SprayButton.activeSelf);
        m_SprayModeOffButton.SetActive(!m_SprayModeOffButton.activeSelf);
    }


    public void Idle()
    {
        m_DollBattleManager.Idle();
    }


    public void SetSprayChargeUIVisible(bool visible)
    {
        m_SprayChargeUI.SetActive(visible);
    }


    private Vector2 m_AimInput;
   
    public void SetAim(Vector2 AimInput)
    {
        m_AimInput = AimInput;
    }

    public void StartSpray()
    {
        m_DollBattleManager.StartGreaterSkill();
        m_SprayChargeUI.SetActive(true);
    }
    
    public void EndSpray()
    {
        m_DollBattleManager.EndGreaterSkill(m_AimInput);
        m_SprayChargeUI.SetActive(false);
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
        m_LesserSkillCooldownText.text = time.ToString();
    }

    public void UpdateDash()
    {
        m_CurrentDoll = m_Party.ActiveDoll;
        m_DollBattleManager = m_CurrentDoll.DollController.BattleManager;


        m_SprayUI.UpdateUI();

        if (m_DollBattleManager.FlehmenCooldown)
        {
            m_LesserSkillCooldownText.gameObject.SetActive(true);
            m_LesserSkillCooldownText.text =
                m_DollBattleManager.LesserSkillCooldownTime.ToString();
            m_FlehmenButton.SetInteractable(false);
        }
        else
        {
            m_LesserSkillCooldownText.gameObject.SetActive(false);
            m_FlehmenButton.SetInteractable(true);
        }
    }

   
}
