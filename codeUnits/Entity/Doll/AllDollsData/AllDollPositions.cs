using GentianoseRealDolls;
using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense;
using UnityEngine;



[Serializable]
public class DollPositions
{
    public int dollID;
    public int Scene;
    public Vector3[] Positions;
    public Quaternion Rotation;
}

public class AllDollPositions : MonoBehaviour, IAllDolls
{
    private const string fileName2 = "dPositions.dat";
    private const string path = "Assets/JSON/dPositions.dat";

    [Tooltip("-1 meaning this scene is not a location")]
    // включая меню
    private int m_Scene;



    [SerializeField] private DollPositions[] allPositions;
    private List<DollPositions> allPositionsList = new List<DollPositions>();
    private void Awake()
    {
        //Saver<DollPositions[]>.TryLoad(fileName2, ref allPositions);
        //allPositionsList = allPositions.ToList();
    }


    public void SetScene(int scene)
    {
        m_Scene = scene;
    }

    public void InitPositions()
    {
        Saver<DollPositions[]>.TryLoad2(path, ref allPositions);
        allPositionsList = allPositions.ToList();
    }


    public void AddDollPos(DollPositions dp)
    {
        allPositionsList.Add(dp);
        allPositions = allPositionsList.ToArray();
    }
    public DollPositions GetDollPos(int id)
    {
        if (allPositionsList == null)
        {
            allPositions = new DollPositions[WhooSettings.NumberOfDolls];
        }

        allPositionsList = allPositions.ToList();

        return allPositionsList[id];
    }
   

    public Vector3[] GetDollPositions(int id)
    {
        return GetDollPos(id).Positions;
    }
    public void SetDollPos(DollPositions dp)
    {
        allPositions[dp.dollID] = dp;
        SaveAllDolls();

    }
    public void SaveAllDolls()
    {
        Saver<DollPositions[]>.Save2(path, allPositions);
    }
}
