using GentianoseRealDolls;
using SpaceShooter;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class HealSide : DollPart
{
    [SerializeField] private float m_CooldownDuration = 7f;
    [SerializeField] private int m_Heal;
    [SerializeField] private int m_StatusID = 3;
    [SerializeField] private int m_Multiplier = 1;

    private float m_Time;

    private bool m_Cooldown;


    public override void SetActionTime(float time)
    {
        m_Time = time;
    }
    public override void Use()
    {
        Heal();
    }
    
    private void Heal()
    {
         m_Party?.RestoreHPAll(m_Heal * m_Multiplier);
    }

    public void SetParty(Party p)
    {
        m_Party = p;
    }

}

