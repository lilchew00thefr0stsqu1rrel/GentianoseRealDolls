using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{

    [Serializable]
    public class DollInBed
    {
        public int ID;

        public bool IsSleep;

        public void SetSleepState(bool sleep)
        {
            IsSleep = sleep;
        }
    }
    public class AllDollSleeps : MonoBehaviour, IAllDolls
    {

        private string path;


        //private string fileName = "dInBeds.dat";

        [SerializeField] private int[] m_Sleeping;

        [SerializeField] private UnityEngine.UI.Text m_DebugText;
        [SerializeField] private DollBase m_DollBase;


        public event Action<bool> OnDollSleepStateChanged;
        private string[] m_FieldNames = new string[]
        {
            "dollID", "inBed"
        };

        private void Awake()
        {
            DollSleep.OnSleepOrWake += WriteDoll();

        }

        private void Start()
        {
            ReadDolls();
        }

        private void OnDestroy()
        {
            DollSleep.OnSleepOrWake -= WriteDoll();
        }
        private Action<int[]> WriteDoll()
        {
            return (m_PositionsInt) =>
            {
                WriteDoll(m_PositionsInt);
            };
        }

        [SerializeField] private List<int> m_Dolls;

        [Tooltip("int[2n]")]
        public void ReadDolls()
        {
            m_Dolls.Clear();

            for (int i = 0; i < WhooSettings.NumberOfDolls; i++)
            {
                int[] slp = m_DollBase.GetRecord("dollSleeps", "dollID", i, m_FieldNames);

                m_Dolls.AddRange(slp[..2]);
            }
        }
        [Tooltip("int[2n]")]
        public List<int> GetDolls()
        {
            return m_Dolls;
        }

        [Tooltip("int[2]")]
        public void WriteDoll(int[] data)
        {

            if (m_DollBase.CheckRecordPresent(data[0], "dollSleeps"))
            {

                string query = $"UPDATE dollSleeps SET inBed='{data[1]}' WHERE dollID='{data[0]}';";
                m_DollBase.AddOrChangeRecord(query);
            }
            else
            {
                m_DollBase.AddOrChangeRecord(
                    "INSERT OR IGNORE INTO dollSleeps (dollID, inBed) VALUES ('" + data[0] +
                        "', '" + data[1] + "');");
            }
        }

        public int[] GetDoll(int id)
        {
            return m_Dolls.ToArray()[id..(id + 2)];
        }

    }
}

