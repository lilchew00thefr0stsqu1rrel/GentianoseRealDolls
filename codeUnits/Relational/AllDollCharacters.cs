using System.Collections.Generic;
using System;
using TowerDefense;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace GentianoseRealDolls
{
    [Serializable]
    public class DollScaleValues
    {
        public int dollID;
        public int LooPoo;
        public int AnalSprayAmount;
        public int LooPee;
        public int Bath;
        public int BrushTeeth;
        public int FoodHunger;
        public int Sleep;

        public DollScaleValues(int id, int poo, int spray, int pee, int bath, int brush, int food, int sleep)
        {
            dollID = id;
            LooPoo = poo;
            AnalSprayAmount = spray;
            LooPee = pee;
            Bath = bath;
            BrushTeeth = brush;
            FoodHunger = food;
            Sleep = sleep;
        }
        public DollScaleValues(int id)
        {
            dollID = id;
        }
    }

    public class AllDollCharacters : MonoBehaviour, IAllDolls
    {

        [SerializeField]
        private string[] m_FieldNames = new string[]
        {
        "dollID",
        "poo",
        "analSpray",
        "pee",
        "bath",
        "brushTeeth",
        "food",
        "sleep"
        };
        // включая меню
        private int m_Scene;

        [Tooltip("-1 meaning this scene is not a location")]

        [SerializeField] private DollScaleValues[] allScaleValues;
        [SerializeField] private List<DollScaleValues> allScaleValuesList = new List<DollScaleValues>();



        [SerializeField] private UnityEngine.UI.Text m_DebugText;
        [SerializeField] private DollBase m_DollBase;
        private void Awake()
        {
        }

        private void Start()
        {
            var q3 = "CREATE TABLE IF NOT EXISTS dollStats (dollID INTEGER PRIMARY KEY, " +
                "poo INTEGER, analSpray INTEGER, pee INTEGER, bath INTEGER, brushTeeth INTEGER, " +
                "food INTEGER, sleep INTEGER)";
            m_DollBase.CreateTable(q3);

            // Seed data.
            for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
            {
                m_DollBase.AddOrChangeRecord("INSERT OR IGNORE INTO dollStats " +
                    "(dollID, poo, analSpray, pee, bath, brushTeeth, food, sleep) " +
                    "VALUES ('" + i +
                        "', '" + 0 + "', '" + 0 + "', '" + 60 + "', '" + 0 + "', '" + 0 + "', '" +
                        0 + "', '" + 0 + "');");
            }
        }

        public DollScaleValues[] ReadStats()
        {

            if (m_DollBase.GetRecordAmount("dollStats") > 0)
            {
                allScaleValues = new DollScaleValues[WhooSettings.NumberOfDolls];

                for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
                {
                    if (m_DollBase.CheckRecordPresent(i, "dollStats"))
                    {
                        allScaleValues[i] = new DollScaleValues(i);
                        int[] statsInt = m_DollBase.GetRecord("dollStats", "dollID", i, m_FieldNames);

                        print($"{i} {statsInt.Length} Rundix");

                        allScaleValues[i].LooPoo = statsInt[1];
                        allScaleValues[i].AnalSprayAmount = statsInt[2];
                        allScaleValues[i].LooPee = statsInt[3];
                        allScaleValues[i].Bath = statsInt[4];
                        allScaleValues[i].BrushTeeth = statsInt[5];
                        allScaleValues[i].FoodHunger = statsInt[6];
                        allScaleValues[i].Sleep = statsInt[7];
                        
                    }
                }
            }


            allScaleValuesList = allScaleValues.ToList();
       
         

            return allScaleValues;

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
            for (int i = 0; i < 3; i++)
            {
                if (m_DollBase.CheckDollPositionPresent(i))
                {
                    ChangeDoll(i);
                }
                else
                {
                    m_DollBase.AddOrChangeRecord("INSERT OR IGNORE INTO dollStats " +
                    "(dollID, poo, analSpray, pee, bath, brushTeeth, food, sleep) " +
                    "VALUES ('" + i +
                        "', '" + 0 + "', '" + 0 + "', '" + 60 + "', '" + 0 + "', '" + 0 + "', '" +
                        0 + "', '" + 0 + "');");
                }
            }
        }


        private void ChangeDoll(int dollID)
        {
            int poo = (int)Mathf.Ceil(allScaleValues[dollID].LooPoo);
            int asa = (int)Mathf.Ceil(allScaleValues[dollID].AnalSprayAmount);
            int pee = (int)Mathf.Ceil(allScaleValues[dollID].LooPee);
            int bath = (int)Mathf.Ceil(allScaleValues[dollID].Bath);
            int teeth = (int)Mathf.Ceil(allScaleValues[dollID].BrushTeeth);
            int food = (int)Mathf.Ceil(allScaleValues[dollID].FoodHunger);
            int sleep = (int)Mathf.Ceil(allScaleValues[dollID].Sleep);

            string query = $"UPDATE dollStats SET poo='{poo}'," +
                            $"analSpray='{asa}', pee='{pee}', bath='{bath}', brushTeeth='{teeth}'," +
                            $" food='{food}', sleep='{sleep}' WHERE dollID='{dollID}';";
            m_DollBase.AddOrChangeRecord(query);
        }

    }

}



