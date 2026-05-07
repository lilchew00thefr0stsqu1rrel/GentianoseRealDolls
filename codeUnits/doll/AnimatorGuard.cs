using UnityEngine;

public class AnimatorGuard : MonoBehaviour, IAnimatorController
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private int m_AnimID;

    public void SetAnimation(int anim)
    {
        m_Animator.SetInteger("Autom", anim);
        m_AnimID = anim;
    }

    public bool IsIdle()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("-"))
            return true;
        return false;
    }
    public bool IsGallop()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("+++"))
            return true;
        return false;
    }
    public float NormalizedTime()
    {
        return m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    public bool IsMotion()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("+")
            || m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("++")
            || m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("+++"))
            return true;
        return false;
    }
    public bool IsTag(string tag)
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsTag(tag))
            return true;
        return false;
    }
}

interface IAnimatorController
{
    void SetAnimation(int anim);
}

