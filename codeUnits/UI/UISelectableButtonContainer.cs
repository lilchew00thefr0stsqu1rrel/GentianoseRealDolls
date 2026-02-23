
using UnityEngine;

public class UISelectableButtonContainer : MonoBehaviour
{
    [SerializeField] private Transform buttonsContainer;

    public bool Interactable = true;

    public bool SetInteractable(bool interactable) => Interactable = interactable;

    private UIHoverButton[] buttons;

    private int selectButtonIndex = -1;

    private void Start()
    {
        buttons = buttonsContainer.GetComponentsInChildren<UIHoverButton>();

        if (buttons == null)
            Debug.LogError("Button list is empty!");

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].PointerEnter += OnPointerEnter;
        }

        if (Interactable == false) return;

        buttons[selectButtonIndex].SetFocuse();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].PointerEnter -= OnPointerEnter;
        }
    }

    private void OnPointerEnter(UIButton button)
    {
        SelectButton(button);
    }

    private void SelectButton(UIButton button)
    {
        if (Interactable == false) return;

        if (selectButtonIndex != -1) 
            buttons[selectButtonIndex].SetUnFocuse();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == button)
            {
                selectButtonIndex = i;
                button.SetFocuse();
                break;
            }
        }
    }

    public void SelectNext() { }

    public void SelectPrevious() { }
}

