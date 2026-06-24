using GentianoseRealDolls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TowerDefense;
using UnityEngine;
using UnityEngine.UI;




public class ActiveDollPosition : MonoBehaviour
{
    private const string fileName2 = "dPositions.dat";

    [Tooltip("-1 meaning this scene is not a location")]
    // включая меню
    private int m_Scene;

    [SerializeField] private Text m_DebugText;


    [SerializeField] private int[] m_PositionsInt;

    [SerializeField]
    private string[] m_FieldNames = new string[]
    {
        "levelID",
        "x",
        "y",
        "z"
    };


    [SerializeField] private DollBase m_SimpleDB;


    private void Awake()
    {
    }

    private void Start()
    {
        GetDoll();
    }

    private void OnDestroy()
    {

    }


    [Tooltip("int[4]")]
    public int[] GetDoll()
    {
        m_PositionsInt = m_SimpleDB.GetOnlyRecord("positions", m_FieldNames)[0..4];
        
        return m_PositionsInt;
    }

    [Tooltip("V3")]
    public Vector3 GetDollPos()
    {
        Vector3 pos = new Vector3(m_PositionsInt[1], m_PositionsInt[2], m_PositionsInt[3]);

        return pos;
    }


    [Tooltip("int[4]")]
    public void SetDoll(int[] dp)
    {
        int x = (int)Mathf.Ceil(dp[1]);
        int y = (int)Mathf.Ceil(dp[2]);
        int z = (int)Mathf.Ceil(dp[3]);

        m_PositionsInt = dp;


        string query = $"UPDATE positions SET levelID='{dp[0]}', x='{x}', y='{y}', z='{z}';";
        m_SimpleDB.AddOrChangeRecord(query);
        
    }

}
