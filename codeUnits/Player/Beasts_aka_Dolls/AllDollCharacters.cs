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

    [Serializable]
    public class DollPositions
    {
        public int dollID;
        public int Scene;
        public Vector3[] Positions;
        public Quaternion Rotation;
    }
    public class AllDollCharacters : SingletonBase<AllDollCharacters>
    {
        private const string fileName1 = "doll.dat";
        private const string fileName2 = "dPositions.dat";
        private const string fileName3 = "dSleeps.dat";

        // включая меню
        private int m_Scene;

        [Tooltip("-1 meaning this scene is not a location")]

        [SerializeField] private DollScaleValues[] allScaleValues;
        private List<DollPositions> allPositionsList = new List<DollPositions>();

        [SerializeField] private DollPositions[] allPositions;
        private List<DollScaleValues> allScaleValuesList = new List<DollScaleValues>();

        private new void Awake()
        {
            base.Awake();

            Saver<DollScaleValues[]>.TryLoad(fileName1, ref allScaleValues);
            allScaleValuesList = allScaleValues.ToList();

            Saver<DollPositions[]>.TryLoad(fileName2, ref allPositions);
            allPositionsList =allPositions.ToList();

           // print(allPositions[1].Positions[0]);
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

         //   print("Start "+ allScaleValues[1].Positions[0]);
        }

        // Update is called once per frame
        void Update()
        {

       //     print(allScaleValues[1].Positions[0]);
        }
        public DollScaleValues FindDollByID(int id)
        {
            foreach (var kvp in allScaleValues)
            {
                if (kvp.dollID == id)
                {
                    return kvp;
                }
            }
            return null;
        }



        public void SetScene(int scene)
        {
            m_Scene = scene;
        }
        //public void TakeAndSetDollPos(int id)
        //{
        //    m_Scene = id;


        //    transform.position = m_Positions[sceneToLevel[m_Scene]];
        //    transform.rotation = m_Rotation;

        //    transform.SetPositionAndRotation(m_Positions[sceneToLevel[m_Scene]], m_Rotation);
        //}
    
        [SerializeField] private DollCurrentStats m_CurrentStats;

  
        public void SaveAllDollStats()
        {
            Saver<DollScaleValues[]>.Save(fileName1, allScaleValues);
        }
       

        public void AddDoll(DollScaleValues sv)
        {
            allScaleValuesList.Add(sv);
            allScaleValues = allScaleValuesList.ToArray();
        }
     
        public DollScaleValues GetDollData(int id)
        {
            return allScaleValuesList[id];
        }
      
        public void SetDollData(DollScaleValues sv)
        {
            allScaleValues[sv.dollID] = sv;
            SaveAllDollStats();

        }
        public void SaveAllDollPositions()
        {
            Saver<DollPositions[]>.Save(fileName2, allPositions);
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
                allPositions = new DollPositions[3];

                Saver<DollPositions[]>.Save(fileName2, allPositions);
            }


            Saver<DollPositions[]>.TryLoad(fileName2, ref allPositions);
            allPositionsList = allPositions.ToList();


            return allPositionsList[id];
        }
        public void SetDollPos(DollPositions dp)
        {
            allPositions[dp.dollID] = dp;
            SaveAllDollPositions();

        }

        public Vector3[] GetDollPositions(int id)
        {
            //  return allScaleValues[id].Positions;
            return GetDollPos(id).Positions;
        }
    }

}

