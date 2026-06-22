using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
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
    /// <summary>
    /// CRUD
    /// </summary>
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

        [SerializeField]
        private int[] AnalGlandCapacities =
            new int[]
            {
                180,
                60,
                80,
                70,
                15
            };

        
        [SerializeField] private UnityEngine.UI.Text m_DebugText;
        [SerializeField] private DollBase m_DollBase;

        [SerializeField] private List<int> m_Dolls;

        /// <summary>
        /// Doll has 7 stats. Together with doll ID gets 8 numbers 
        /// </summary>
        private const int NumberOfStatsWithDollID = 8;
        private Action<int[]> SaveDoll()
        {
            return (stats) =>
            {
                WriteDoll(stats);
            };
        }

        private void Awake()
        {
            Doll.OnSave += SaveDoll();

        }
        private void Start()
        {
        }

        private void OnDestroy()
        {
            Doll.OnSave -= SaveDoll();
        }
        

        public void ReadDolls()
        {
            m_Dolls.Clear();
            int[] stats = new int[8];

            for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
            {
                stats = m_DollBase.GetRecord("dollStats", "dollID", i, m_FieldNames);

                m_Dolls.AddRange(stats);
            }
        }

        [Tooltip("int[8n]")]
        public List<int> GetDolls()
        {
            ReadDolls();

            return m_Dolls;

        }

        [Tooltip("int[8]")]
        public int[] GetDoll(int dollID)
        {
            ReadDolls();

            int[] doll = new int[8];
            for (int i = 0; i < 8; i++)
            {
                doll[i] = m_Dolls[dollID * 8 + i];
            }
            return doll;
        }
        [Tooltip("int[8]")]
        public void WriteDoll(int[] stats)
        {
            ReadDolls();

            int id = stats[0];

            m_Dolls[id + 1] = stats[1];
            m_Dolls[id + 2] = stats[2];
            m_Dolls[id + 3] = stats[3];
            m_Dolls[id + 4] = stats[4];
            m_Dolls[id + 5] = stats[5];
            m_Dolls[id + 6] = stats[6];
            m_Dolls[id + 7] = stats[7];

            
                if (m_DollBase.CheckRecordPresent(id, "dollStats"))
                {

                    string query = $"UPDATE dollStats SET poo='{stats[1]}'," +
                                    $"analSpray='{stats[2]}', pee='{stats[3]}', bath='{stats[4]}', brushTeeth='{stats[5]}'," +
                                    $" food='{stats[6]}', sleep='{stats[7]}' WHERE dollID='{stats[0]}';";
                    m_DollBase.AddOrChangeRecord(query);
                }
                else
                {
                    m_DollBase.AddOrChangeRecord("INSERT OR IGNORE INTO dollStats " +
                    "(dollID, poo, analSpray, pee, bath, brushTeeth, food, sleep) " +
                    "VALUES ('" + id +
                        "', '" + 0 + "', '" + 0 + "', '" + 60 + "', '" + 0 + "', '" + 0 + "', '" +
                        0 + "', '" + 0 + "');");
                }
            
        }

        public void ReduceNonSleepStats()
        {
            for (int i = 0; i < 3; i++)
            {
                int[] doll = new int[8];
                doll[0] = m_Dolls[i * 8];
                doll[1] = Mathf.Clamp(m_Dolls[i * 8 + 1] - 1, 0, 10);

                doll[2] = Mathf.Clamp(m_Dolls[i * 8 + 2] - 1, 0, AnalGlandCapacities[i]);

                doll[3] = Mathf.Clamp(m_Dolls[i * 8 + 3] - 1, 0, 10);

                doll[4] = Mathf.Clamp(m_Dolls[i * 8 + 4] - 1, 0, 40);

                doll[5] = Mathf.Clamp(m_Dolls[i * 8 + 5] - 1, 0, 30);

                doll[6] = Mathf.Clamp(m_Dolls[i * 8 + 6] - 1, 0, 100);

                doll[7] = m_Dolls[i * 8 + 7];

                WriteDoll(doll);



            }
        }
       
        public void ChangeSleepStat(List<int> sleepMap)
        {
            for (int i = 0; i < 3; i++)
            {
                int[] doll = new int[8];


                doll[0] = m_Dolls[i * 8];
                doll[1] = m_Dolls[i * 8 + 1];
                doll[2] = m_Dolls[i * 8 + 2];
                doll[3] = m_Dolls[i * 8 + 3];
                doll[4] = m_Dolls[i * 8 + 4];
                doll[5] = m_Dolls[i * 8 + 5];
                doll[6] = m_Dolls[i * 8 + 6];
                if (sleepMap[i * 2 + 1] == 1)
                    doll[i * 8 + 7] = Mathf.Clamp(m_Dolls[i * 8 + 7] + 1, 0, 100);
                if (sleepMap[i * 2 + 1] == 0)
                    doll[i * 8 + 7] = Mathf.Clamp(m_Dolls[i * 8 + 7] - 1, 0, 100);


                WriteDoll(doll);



            }
        }

    }

}

