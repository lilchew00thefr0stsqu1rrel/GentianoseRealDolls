using GentianoseRealDolls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TowerDefense;
using UnityEngine;
using UnityEngine.UI;



[Serializable]
public class DollPosition
{
    public int dollID;
    public int Scene;
    public Vector3 Position;
    public Quaternion Rotation;

    public DollPosition(int dollID, int scene, Vector3 position, Quaternion rotation)
    {
        this.dollID = dollID;
        Scene = scene;
        Position = position;
        Rotation = rotation;
    }
}

public class AllDollPositions : MonoBehaviour
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
        "dollID",
        "levelID",
        "x",
        "y",
        "z"

    };


    [SerializeField] private DollBase m_SimpleDB;


    private void Awake()
    {
        BeastPositionManager.OnSavePos += WriteDoll();   
    }

    private void OnDestroy()
    {
        BeastPositionManager.OnSavePos -= WriteDoll();

    }

    private Action<int[]> WriteDoll()
    {
        return (m_PositionsInt) =>
        {
            SetDoll(m_PositionsInt);
        };
    }

    [Tooltip("int[5]")]
    public int[] GetDoll()
    {
        int[] positionsInt = m_SimpleDB.GetRecord("positions", "dollID", 0, m_FieldNames);
        positionsInt = positionsInt[0..5];
        return positionsInt;
    }



    [Tooltip("int[5]")]
    public void SetDoll(int[] dp)
    {
        if (m_SimpleDB.CheckDollPositionPresent(0))
        {
            int x = (int)Mathf.Ceil(dp[2]);
            int y = (int)Mathf.Ceil(dp[3]);
            int z = (int)Mathf.Ceil(dp[4]);

            string query = $"UPDATE positions SET levelID='{dp[1]}'," +
                            $"x='{x}', y='{y}', z='{z}' WHERE dollID='0';";
            m_SimpleDB.AddOrChangeRecord(query);
        }
        else
        {
            m_SimpleDB.AddOrChangeRecord("INSERT OR IGNORE INTO positions (dollID, levelID, x, y, z) VALUES ('" + dp[0] +
                    "', '" + 0 + "', '" + 0 + "', '" + 0 + "', '" + 0 + "');");
        }
        
    }

}
