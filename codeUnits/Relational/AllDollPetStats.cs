using System;
using System.Collections.Generic;
using UnityEngine;

namespace GentianoseRealDolls
{
    
    /// <summary>
    /// CRUD
    /// Original name: AllDollCharacters
    /// </summary>
    public class AllDollPetStats : MonoBehaviour, IAllDolls
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
        private DollDataLists m_DollsData;

        private int m_FieldNumber = 8;

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

        private int m_NumberOfDolls = 7;

        public void ReadDolls()
        {
            m_Dolls.Clear();
            int[] stats = new int[8];

            for (int i = 0; i < m_NumberOfDolls; i++)
            {
                stats = m_DollBase.GetRecord("dollStats", "dollID", i, m_FieldNames);

                m_Dolls.AddRange(stats);
            }

            m_Dolls.Add(336);
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
            if (m_Dolls.Count == 0) return;

            int id = stats[0];

            m_Dolls[id * m_FieldNumber + 1] = stats[1];
            m_Dolls[id * m_FieldNumber + 2] = stats[2];
            m_Dolls[id * m_FieldNumber + 3] = stats[3];
            m_Dolls[id * m_FieldNumber + 4] = stats[4];
            m_Dolls[id * m_FieldNumber + 5] = stats[5];
            m_Dolls[id * m_FieldNumber + 6] = stats[6];
            m_Dolls[id * m_FieldNumber + 7] = stats[7];

            
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


        /// <summary>
        /// /// Фунька добавляется 0.1 мл (1 очко) каждые 15 мин
        /// Туалет (кал и моча): 1 каждые 15 мин
        /// Ванная: 1 каждые 6 минут 
        /// Чистка зубов: 1 каждые 5 мин
        /// Еда и сон: 1 каждую минуту
        /// </summary>
        
        public void ReduceNonSleepStats()
        {
            for (int i = 0; i < m_NumberOfDolls; i++)
            {
                int[] doll = m_Dolls.ToArray()[(i * 8)..((i + 1) * 8)];

                doll[1] = Mathf.Clamp(doll[1] - 1, 0, 10);

                doll[2] = Mathf.Clamp(doll[2] + 1, 0, m_DollsData.AnalGlandVolumeArray[i]);

                doll[3] = Mathf.Clamp(doll[3] - 1, 0, 10);

                doll[4] = Mathf.Clamp(doll[4] - 1, 0, 40);

                doll[5] = Mathf.Clamp(doll[5] - 1, 0, 30);

                doll[6] = Mathf.Clamp(doll[6] - 1, 0, 100);

                WriteDoll(doll);
            }
        }
       
        public void ChangeSleepStat(List<int> sleepMap)
        {
            for (int i = 0; i < m_NumberOfDolls; i++)
            {
                int[] doll = m_Dolls.ToArray()[(i * 8)..((i + 1) * 8)];
                print(i + '~' +doll[7]);
                if (sleepMap[i] == 1)
                    doll[7] = Mathf.Clamp(doll[7] + 1, 0, 100);
                if (sleepMap[i] == 0)
                    doll[7] = Mathf.Clamp(doll[7] - 1, 0, 100);

                WriteDoll(doll);
            }
        }

    }

}


