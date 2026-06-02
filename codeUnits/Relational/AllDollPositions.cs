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

public class AllDollPositions : MonoBehaviour, IAllDolls
{
    private const string fileName2 = "dPositions.dat";

    [Tooltip("-1 meaning this scene is not a location")]
    // включая меню
    private int m_Scene;

    [SerializeField] private Text m_DebugText;
    [SerializeField] private DollPosition[] allPositions;


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
    private List<DollPosition> allPositionsList = new List<DollPosition>();

    private void Awake()
    {
        //Saver<DollPositions[]>.TryLoad(fileName2, ref allPositions);
        //allPositionsList = allPositions.ToList();

        print("~~~~~" + m_SimpleDB.GetDollPosition(0).Position);


    }







    public void SetScene(int scene)
    {
        m_Scene = scene;
    }
    
    public void InitPositions()
    {

        //Saver<DollPosition[]>.TryLoad(WhooSettings.fileNamePos, ref allPositions);

        //if (allPositions.Length == 0)
        //{
        //    allPositions = new DollPosition[WhooSettings.NumberOfDolls];
        //    Saver<DollPosition[]>.Save(WhooSettings.fileNamePos, allPositions);
        //}



        if (m_SimpleDB.GetDollAmount() > 0)
        {
            allPositions = new DollPosition[WhooSettings.NumberOfDolls];

            for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
            {
                if (m_SimpleDB.CheckDollPositionPresent(i))
                {
                    //allPositions[i] = m_SimpleDB.GetDollPosition(i);

                    int[] positionsInt = new int[5];

                    m_SimpleDB.GetRecord(i, $"SELECT * FROM positions WHERE dollID = {i};", ref positionsInt, m_FieldNames);

                    allPositions[i] = new DollPosition(positionsInt[0], positionsInt[1], 
                        new Vector3(positionsInt[2], positionsInt[3], positionsInt[4]), Quaternion.identity);
                }
            }
        }

        m_DebugText.text = m_SimpleDB.GetDollAmount().ToString();

        allPositionsList = allPositions.ToList();

        print("KUKLYYNN " + m_SimpleDB.GetDollAmount());
    }

    public void AddDollPos(DollPosition dp)
    {
        allPositionsList.Add(dp);
        allPositions = allPositionsList.ToArray();
    }

    public DollPosition GetDollPos(int id)
    {

        if (allPositionsList == null)
        {
            allPositions = new DollPosition[WhooSettings.NumberOfDolls]; 
        }

        


        allPositionsList = allPositions.ToList();

        // return allPositionsList[id];

        if (m_SimpleDB.CheckDollPositionPresent(id))
        {
            allPositions[id] = m_SimpleDB.GetDollPosition(id);
        }

        return allPositions[id];
    }
   

    public Vector3 GetDollPositions(int id)
    {
        return GetDollPos(id).Position;
    }

    public void SetDollPos(DollPosition dp)
    {
        allPositions[dp.dollID] = dp;
        SaveAllDolls();
    }

    public void SaveAllDolls()
    {
        //Saver<DollPosition[]>.Save(WhooSettings.fileNamePos, allPositions);

        for (int i = 0; i < 3; i++)
        {
            if (m_SimpleDB.CheckDollPositionPresent(i))
            {
                ChangeDoll(i);
            }
            else
            {
                m_SimpleDB.AddDollPosition(i, 1, 0, 0, 0);
            }
        }
    }

    private void ChangeDoll(int dollID)
    {
        int x = (int)Mathf.Ceil(allPositions[dollID].Position.x);
        int y = (int) Mathf.Ceil(allPositions[dollID].Position.y);
        int z = (int)Mathf.Ceil(allPositions[dollID].Position.z);

        string query = $"UPDATE positions SET levelID='{allPositions[dollID].Scene}'," +
                        $"x='{x}', y='{y}', z='{z}' WHERE dollID='{dollID}';";
        m_SimpleDB.AddOrChangeRecord(query);
    }
}
