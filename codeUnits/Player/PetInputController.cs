using GentianoseRealDolls;
using NTC.MonoCache;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class PetInputController : MonoCache
{
    [SerializeField] private Party party;
    [SerializeField] private Dashboard dashboard;
    [SerializeField] private CombatDashboard combatDashboard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combatDashboard = dashboard.CombatUI;
    }

    protected override void Run()
    {
        base.Run();

        // ���� ����. �����


        combatDashboard.SetAim(aim);

    }

    // ���� ����� ���������� ��� ������� ������ Jump
    public void OnPoop(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;

        if (!SarvaToilet.CanPoop) return;

        // � �������� ���������� �������
        // ������ ������������ ������ �� 90 ��������

        party.ActiveDoll.DollController.PoopManager.ToPoop();
    }

    // ���� ����� ���������� ��� ������� ������ Map
    public void OnOpenMap(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;

        
        dashboard.ShowMap();
        
    }
    // ���� ����� ���������� ��� ������� ������ Esc
    public void OnEscape(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;

        // ��������� ��� ��������� ���������, ��� � Terraria
        dashboard.OnEscape();

    }   
    


    // ���� ����� ���������� ��� ������� ������� F
    public void OnInteract(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;


        // 
        if (dashboard.InteractTipActive)
            dashboard.Interact();
    }

    [SerializeField] Vector2 aim;
    public void OnEndSpray(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;

        // 1 - ���� 
        party.ActiveDoll.DollController.BattleManager.EndGreaterSkill(aim);
        dashboard.SetSprayChargeUIVisible(false);
    }
    public void OnStartSpray(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;

        party.ActiveDoll.DollController.BattleManager.StartGreaterSkill();
        dashboard.SetSprayChargeUIVisible(true);
    }
    public void OnCursorAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValue<Vector2>();


        party.ActiveDoll.DollController.SetAimInput(aim);

    }
    public void OnLesserSkill(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;

        party.ActiveDoll.DollController.BattleManager.LesserSkill();
    }


    public async void OnSprayStanceOnOff(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;

        bool coold = false;

        if (!coold)
        combatDashboard.SprayStanceOnOff();

         coold = true;
        await Task.Delay(400);
        coold = false;

    }

    public void OnDigitKey1(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;
        party.SetActiveDoll(0);
        
        dashboard.InitDoll();
    }
    public void OnDigitKey2(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;
        party.SetActiveDoll(1);


        dashboard.InitDoll();
    }
    public void OnDigitKey3(InputAction.CallbackContext context)
    {
        // ���������, ��� �������� ������ ���������,
        // � �� �������� ��� � ��������
        if (!context.performed) return;
        party.SetActiveDoll(2);


        dashboard.InitDoll();
    }
}
