using GentianoseRealDolls;
using SpaceShooter;
using UnityEngine;

public class AttackSide : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    { 
    }
    Collider[] targets;

    [SerializeField] private bool m_SprayType;



    [SerializeField] private int m_StatusID;

    [SerializeField] private int m_AttackDamage;

    [SerializeField] private float m_CooldownDuration = 0.1f;

    [SerializeField] private bool m_BelongsToDoll;

    private float m_Time;

    private bool m_Cooldown;
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

    public void SetDamage(int damage)
    {
        m_AttackDamage = damage;
    }

    public void SetParent(Destructible destructible)
    {
       parent = destructible;
    }

    [SerializeField] private Destructible parent;

    [SerializeField] private float m_Multiplier = 1;
 
    private void OnTriggerEnter(Collider other)
    {
        print("cll");
        if (parent == null) return;

        
        if (other != null &&

                // если не попадает коллайдер лечащего поля куклы
                !other.isTrigger)
        {
            Destructible dest = other.transform.root.GetComponent<Destructible>();

            if (dest != null)
            {
                

                if (m_BelongsToDoll && !dest.GetComponent<Doll>() || !m_BelongsToDoll)
                {
                    if (dest != parent)
                    {
                        if (!m_Cooldown)
                        {
                            if (m_SprayType)
                            {
                                dest.ApplyDamage(m_AttackDamage);
                                dest.ApplyDebuff(m_StatusID, m_Multiplier, 14);
                                m_Cooldown = true;
                            }
                            else
                            {
                                dest.ApplyDamage(m_AttackDamage);
                                m_Cooldown = true;
                            }
                        }



                    }
                }
                    
            }


        }
    }
       

}

