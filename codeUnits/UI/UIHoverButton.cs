
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIHoverButton : UIButton
{
    [SerializeField] private Image hoverImage;

    public UnityEvent OnSelect;
    public UnityEvent OnUnSelect;

    private void Start()
    {
        PointerEnter += OnPointerEnter;
        PointerExit += OnPointerExit;
    }


    private void OnDestroy()
    {
        PointerEnter -= OnPointerEnter;
        PointerExit -= OnPointerExit;
    }

    private void OnPointerEnter(UIButton button)
    {
        SelectButton();
    }
    private void OnPointerExit(UIButton button)
    {
        UnSelectButton();
    }

    private void SelectButton()
    {
        if (Interactable == false) return;

        SetFocuse();
    }
    private void UnSelectButton()
    {
        if (Interactable == false) return;

        SetUnFocuse();
    }
    public override void SetFocuse()
    {
        base.SetFocuse();

        hoverImage.enabled = true;
        OnSelect?.Invoke();
    }
    public override void SetUnFocuse()
    {
        base.SetUnFocuse();

        hoverImage.enabled = false;
        OnUnSelect?.Invoke();
    }
}

