using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIHoldableButton : UIButton
{
   
    [SerializeField] private Image m_HoldImage;

    public UnityEvent OnSelect;
    public UnityEvent OnUnSelect;

    [SerializeField] private Color m_NormalColor;
    [SerializeField] private Color m_HoldColor;

    public override void SetHold()
    {
        base.SetHold();

        print("***HOdl&&");

        if (m_HoldImage)
            m_HoldImage.color = m_HoldColor;
    }

    public override void SetUnHold()
    {
        base.SetUnHold();

        if (m_HoldImage)
            m_HoldImage.color = m_NormalColor;
    }

}
