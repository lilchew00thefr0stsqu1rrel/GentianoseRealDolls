using UnityEngine;

public class AnimatorGuard : MonoBehaviour, IAnimatorController
{
    [Header("Tags: * idle \n+ walk " +
        "\n++ trot/amble/canter \n+++ gallop " +
        "\n. normal attack \n- charged attack " +
        "\n! lesser skill \n& spray")]

    [SerializeField] private Animator m_Animator;
    [SerializeField] private int m_AnimID;   
    

    public void SetAnimation(int anim)
    {
        m_AnimID = anim;
        m_Animator.SetInteger("Autom", m_AnimID);
        print($"Inflicted animation #{m_AnimID}");
    }
    public void LightOn()
    {
        m_Animator.SetBool("Light", true);
        print($"Light");
    }
    public void LightOff()
    {
        m_Animator.SetBool("Light", false);
        print($"Light--");
    }

    public float GetAnimationLength(string name)
    {
        RuntimeAnimatorController ac = m_Animator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            print(ac.animationClips[i]);
            if (ac.animationClips[i].name == name)
            {
                float length = ac.animationClips[i].length;
                // Дальнейшая обработка
                return length;
            }
        }
        return 0f;
    }

    public bool IsIdle()
    {
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("*"))
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

