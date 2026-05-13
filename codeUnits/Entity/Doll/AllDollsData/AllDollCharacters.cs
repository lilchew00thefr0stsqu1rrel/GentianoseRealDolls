using System.Collections.Generic;
using System;
using TowerDefense;
using UnityEngine;
using System.Linq;

namespace GentianoseRealDolls
{
    [Serializable]
    public class DollScaleValues
    {
        public int dollID;
        public float LooPoo;
        public float AnalSprayAmount;
        public float LooPee;
        public float Bath;
        public float BrushTeeth;
        public float FoodHunger;
        public float Sleep;

        public int Scene;
        public Vector3[] Positions;
        public Quaternion Rotation;
    }

    public class AllDollCharacters : MonoBehaviour, IAllDolls
    {
        private const string fileName1 = "doll.dat";
        private const string path = "Assets/JSON/doll.dat";

        // включая меню
        private int m_Scene;

        [Tooltip("-1 meaning this scene is not a location")]

        [SerializeField] private DollScaleValues[] allScaleValues;
        [SerializeField] private List<DollScaleValues> allScaleValuesList = new List<DollScaleValues>();

        [SerializeField] private List<float[]> m_DollMap;

        private void Awake()
        {
        }

        public void InitStats()
        {
            Saver<DollScaleValues[]>.TryLoad2(path, ref allScaleValues);
            allScaleValuesList = allScaleValues.ToList();
            print("!!!!!!! " + allScaleValuesList[0].LooPoo);
        }

        public List<float[]> ReadStats()
        {
            Saver<DollScaleValues[]>.TryLoad2(path, ref allScaleValues);
            allScaleValuesList = allScaleValues.ToList();
            m_DollMap = new List<float[]>();
            int i = 0;
            foreach (var sv in allScaleValuesList)
            {
                var doll = new float[7];
                doll[0] = allScaleValuesList[i].LooPoo;
                doll[1] = allScaleValuesList[i].AnalSprayAmount;
                doll[2] = allScaleValuesList[i].LooPee;
                doll[3] = allScaleValuesList[i].Bath;
                doll[4] = allScaleValuesList[i].BrushTeeth;
                doll[5] = allScaleValuesList[i].FoodHunger;
                doll[6] = allScaleValuesList[i].Sleep;
                m_DollMap.Add(doll);
                i++;

            }
            return m_DollMap;
        }


        public void SetScene(int scene)
        {
            m_Scene = scene;
        }

        public DollScaleValues GetDoll(int id)
        {
            return allScaleValuesList[id];
        }

        public void AddDoll(DollScaleValues sv)
        {
            allScaleValuesList.Add(sv);
            allScaleValues = allScaleValuesList.ToArray();
        }
     
        public void SetDoll(DollScaleValues sv)
        {
            allScaleValues[sv.dollID] = sv;
            SaveAllDolls();
        }

        public void SaveAllDolls()
        {
           Saver<DollScaleValues[]>.Save2(path, allScaleValues);
        }


    }

}

