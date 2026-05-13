using GentianoseRealDolls;
using SpaceShooter;
using UnityEngine;

public class HealSide : DollPart
{
    [SerializeField] private Collider m_HealTrigger;

    [SerializeField] private float m_CooldownDuration = 7f;
    [SerializeField] private int m_Heal;
    [SerializeField] private int m_StatusID = 3;
    [SerializeField] private int m_Multiplier = 1;

    private float m_Time;

    private bool m_Cooldown;
    public override void Use(Vector2 aimInput)
    {
        m_HealTrigger.enabled = true;
    }
    public void SetParty(Party p)
    {
        m_Party = p;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Cooldown)
        {
            m_Time += Time.deltaTime;
            if (m_Time > m_CooldownDuration)
            {
                m_Cooldown = false;
                m_Time = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null  &&
            // если не попадает коллайдер лечащего поля куклы
                !other.isTrigger)
        {
            Destructible dest = other.transform.root.GetComponent<Destructible>();

            if (dest != null)
            {
                if (dest.GetComponent<Doll>())
                {
                    if (!m_Cooldown)
                    {
                        print(dest.name);

                        print("++");
                        dest.RestoreHitPoints(m_Heal);
                        dest.ApplyBuff();
                        m_Cooldown = true;
                    }
                }
            }
        }
    }
}


