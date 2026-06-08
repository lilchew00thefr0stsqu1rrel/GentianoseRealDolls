using GentianoseRealDolls;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AllDollBattle : MonoBehaviour, IAllDolls
{
    [SerializeField] private DollBase m_DollBase;

    [SerializeField]
    private string[] m_FieldNames = new string[]
        {
        "dollID",
        "hp"
        };
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

            m_Dolls.AddRange(stats[..2]);
        }

    }

    public void WriteDoll(int[] stats)
    {

        int id = stats[0];

        m_Dolls[id + 1] = stats[1];


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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
