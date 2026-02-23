using GentianoseRealDolls;
using SpaceShooter;
using UnityEngine;

public class HealSide : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealParty();
    }

    private float m_Time;
    [SerializeField] private float m_CooldownDuration = 7f;
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

    [SerializeField] private int m_Heal;
    [SerializeField] private int m_StatusID = 3;
    [SerializeField] private int m_Multiplier = 1;

    private bool m_Cooldown;
    private void OnTriggerEnter(Collider other)
    {
        print("cll");


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

    public void HealParty()
    {
        Party.Instance.RegenHPAll(m_Heal);
    }
}

