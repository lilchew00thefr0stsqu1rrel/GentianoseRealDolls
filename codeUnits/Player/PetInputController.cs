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
    [SerializeField] private CameraAroundDoll cameraAroundDoll;

    [SerializeField] private VirtualGamePad m_VirtualGamePad;
    private Vector2 m_FirstFing;
    private Vector2 m_SecondFing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combatDashboard = dashboard.CombatUI;
    }

    protected override void Run()
    {
        base.Run();

        // ради обыч. атаки


        combatDashboard.SetAim(aim);

        if (m_FirstFing.x < -0.1f && m_SecondFing.x > 0.1f) 
        {
            cameraAroundDoll.Zoom(-1);
        }
        if (m_FirstFing.x > 0.1f && m_SecondFing.x < -0.1f)
        {
            cameraAroundDoll.Zoom(1);
        }


        if (m_VirtualGamePad.VirtualJoystickRotation.Value.x > 0)
        {
            cameraAroundDoll.Rotate(-1);
        }
        if (m_VirtualGamePad.VirtualJoystickRotation.Value.x < 0)
        {
            cameraAroundDoll.Rotate(1);
        }

    }

    // Этот метод вызывается при нажатии кнопки Jump
    public void OnPoop(InputAction.CallbackContext context)
    {
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
        if (!context.performed) return;

        if (!SarvaToilet.CanPoop) return;

        // В качестве наглядного эффекта
        // просто поворачиваем объект на 90 градусов

        party.ActiveDoll.DollController.PoopManager.ToPoop();
    }

    // Этот метод вызывается при нажатии кнопки Map
    public void OnOpenMap(InputAction.CallbackContext context)
    {
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
        if (!context.performed) return;

        
        dashboard.ShowMap();
        
    }
    // Этот метод вызывается при нажатии кнопки Esc
    public void OnEscape(InputAction.CallbackContext context)
    {
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
        if (!context.performed) return;

        // Открывает или закрывает инвентарь, как в Terraria
        dashboard.OnEscape();

    }   
    


    // Этот метод вызывается при нажатии клаваиа F
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
        if (!context.performed) return;


        // 
        if (dashboard.InteractTipActive)
            dashboard.Interact();
    }

    [SerializeField] Vector2 aim;
    public void OnEndSpray(InputAction.CallbackContext context)
    {
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
        if (!context.performed) return;

        // 1 - анус 
        party.ActiveDoll.DollController.BattleManager.EndGreaterSkill();
        dashboard.SetSprayChargeUIVisible(false);
    }
    public void OnStartSpray(InputAction.CallbackContext context)
    {
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
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
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
        if (!context.performed) return;

        party.ActiveDoll.DollController.BattleManager.LesserSkill();
    }


    public async void OnSprayStanceOnOff(InputAction.CallbackContext context)
    {
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
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
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
        if (!context.performed) return;
        party.InitDoll(0);
        
        dashboard.InitDoll();
    }
    public void OnDigitKey2(InputAction.CallbackContext context)
    {
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
        if (!context.performed) return;
        party.InitDoll(1);


        dashboard.InitDoll();
    }
    public void OnDigitKey3(InputAction.CallbackContext context)
    {
        // Проверяем, что действие именно выполнено,
        // а не отменено или в процессе
        if (!context.performed) return;
        party.InitDoll(2);


        dashboard.InitDoll();
    }
    private float m_Wheel;
    private float m_MouseX;
    
    public void OnZoom(InputAction.CallbackContext context)
    {
        m_Wheel = context.ReadValue<float>();
        cameraAroundDoll.Zoom(m_Wheel > 0 ? 1 : -1);
    }

    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        m_MouseX = context.ReadValue<float>();
        cameraAroundDoll.Rotate(m_MouseX > 0 ? 1 : -1);
    }

    public void OnFirstFing(InputAction.CallbackContext context)
    {
        m_FirstFing = context.ReadValue<Vector2>();
    }

    public void OnSecondFing(InputAction.CallbackContext context)
    {
        m_SecondFing = context.ReadValue<Vector2>();
    }
}
