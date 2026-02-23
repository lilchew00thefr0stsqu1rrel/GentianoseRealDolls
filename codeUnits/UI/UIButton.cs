using GentianoseRealDolls;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private GameObject m_DraggingIcon;
    public event Action Skill;

    private Doll m_CurrentDoll;


    [SerializeField] private Image m_Image;
    [SerializeField] private RectTransform m_ButtonTransform;

    public void SetCurrentDoll(Doll doll)
    {
        // Doll should be in party.
        m_CurrentDoll = doll;
    }

    private void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    [SerializeField] protected bool Interactable;

    private bool focuse = false;
    public bool Focuse => focuse;

    private bool hold = false;
    public bool Hold => hold;

    public UnityEvent OnClick;

    public UnityEvent OnAntiClick;

    public event UnityAction<UIButton> PointerEnter;
    public event UnityAction<UIButton> PointerExit;
    public event UnityAction<UIButton> PointerClick;

    public event UnityAction<UIButton> PointerDown;
    public event UnityAction<UIButton> PointerUp;

    public Vector3 Value { get; private set; }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ButtonTransform,
            eventData.position,
            eventData.pressEventCamera,
            out position);

        if (
            position.x >= m_ButtonTransform.rect.xMin && 
            position.x <= m_ButtonTransform.rect.xMax &&
            position.y >= m_ButtonTransform.rect.yMin &&
            position.y <= m_ButtonTransform.rect.yMax) 
        {
            print("X");
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Interactable == false) return;

        PointerDown?.Invoke(this);

        OnClick?.Invoke();

        print("mdtrkk");
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Interactable == false) return;

        PointerUp?.Invoke(this);


        OnAntiClick?.Invoke();

        Value = Vector3.zero;
        print("Pointer Up");
    }
    public virtual void SetHold()
    {
        if (Interactable == false) return;

        hold = true;


    }
    public virtual void SetUnHold()
    {
        if (Interactable == false) return;

        hold = false;
    }

    public virtual void SetFocuse()
    {
        if (Interactable == false) return;

        focuse = true;
    }
    public virtual void SetUnFocuse()
    {
        if (Interactable == false) return;

        focuse = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Interactable == false) return;

        PointerEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Interactable == false) return;

        PointerExit?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Interactable == false) return;

        PointerClick?.Invoke(this);
        OnClick?.Invoke();
    }


}
