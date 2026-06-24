using GentianoseRealDolls;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AllDollCombatStats : MonoBehaviour, IAllDolls
{
    [SerializeField] private DollBase m_DollBase;

    [SerializeField]
    private string[] m_FieldNames = new string[]
        {
        "dollID",
        "hp"
        };


    private const int m_FieldNumber = 2;

    [SerializeField] private List<int> m_Dolls;

    private Action<int[]> SaveDoll()
    {
        return (stats) =>
        {
            WriteDoll(stats);
        };
    }

    private void Awake()
    {
        Doll.OnSaveCombat += SaveDoll();
    }

    private void OnDestroy()
    {
        Doll.OnSaveCombat -= SaveDoll();
    }

    public List<int> GetDolls()
    {
        ReadDolls();

        return m_Dolls;
    }

    public void ReadDolls()
    {
        m_Dolls.Clear();
        int[] stats = new int[2];

        for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
        {
            stats = m_DollBase.GetRecord("dollBattle", "dollID", i, m_FieldNames);

            m_Dolls.AddRange(stats[..m_FieldNumber]);
        }

    }

    public void WriteDoll(int[] stats)
    {
        int id = stats[0];

        m_Dolls[id * m_FieldNumber + 1] = stats[1];


        if (m_DollBase.CheckRecordPresent(id, "dollBattle"))
        {
            string query = $"UPDATE dollBattle SET hp='{stats[1]}' WHERE dollID='{stats[0]}';";
            m_DollBase.AddOrChangeRecord(query);
        }
        else
        {
            m_DollBase.AddOrChangeRecord("INSERT OR IGNORE INTO dollBattle " +
            "(dollID, hp) " +
            "VALUES ('" + id +
                "', '" + 0 + "');");
        }
        
       
    }
    public void WriteDolls(List<int> allStats)
    {
        m_Dolls = allStats;

        for (int i = 0; i < 3; i++)
        {
            int[] sts = new int[2] { m_Dolls[i * m_FieldNumber], m_Dolls[i * m_FieldNumber + 1] };

            WriteDoll(sts);
        }
    }
    public int[] GetDoll(int dollID)
    {
        ReadDolls();

        int[] doll = new int[m_FieldNumber];
        for (int i = 0; i < m_FieldNumber; i++)
        {
            doll[i] = m_Dolls[dollID * m_FieldNumber + i];
        }
        return doll;
    }

    // Лечение 
    public void RestoreHPAll(int m_HealAmount)
    {
        m_Dolls[1] = Mathf.Clamp(m_Dolls[1] + m_HealAmount, 1, 1000);

        m_Dolls[3] = Mathf.Clamp(m_Dolls[3] + m_HealAmount, 1, 1111);

        m_Dolls[5] = Mathf.Clamp(m_Dolls[5] + m_HealAmount, 1, 1332);

       WriteDolls(m_Dolls);
    }
}
