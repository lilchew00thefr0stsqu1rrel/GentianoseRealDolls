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

        // включая меню
        private int m_Scene;

        [Tooltip("-1 meaning this scene is not a location")]

        [SerializeField] private DollScaleValues[] allScaleValues;
        private List<DollScaleValues> allScaleValuesList = new List<DollScaleValues>();
        [SerializeField] private DollCurrentStats m_CurrentStats;

        private void Awake()
        {
            Saver<DollScaleValues[]>.TryLoad(fileName1, ref allScaleValues);
            allScaleValuesList = allScaleValues.ToList();
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
            Saver<DollScaleValues[]>.Save(fileName1, allScaleValues);
        }


    }

}

