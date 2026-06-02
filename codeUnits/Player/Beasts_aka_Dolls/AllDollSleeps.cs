using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class AllDollSleeps : MonoBehaviour, IAllDolls
    {


        private static int m_DollsInGame = 1;
        [SerializeField] private DollInBed[] m_DollsInBeds;

        private List<DollInBed> m_DollsInBedsList = new List<DollInBed>();
        [SerializeField] private DollInBed[] m_SleepsPut; 
        private string fileName = "dInBeds.dat";
        [SerializeField] private List<bool> m_Sleeping;
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

        public event Action<bool> OnDollSleepStateChanged;

        protected void Awake()
        {


           //// Saver<DollInBed[]>.TryLoad(fileName, ref m_DollsInBeds);

           //// m_DollsInBedsList = m_DollsInBeds.ToList();
        }

        public List<bool> ReadSleeping()
        {

            Saver<DollInBed[]>.TryLoad(fileName, ref m_DollsInBeds);

            m_DollsInBedsList = m_DollsInBeds.ToList();

            m_Sleeping = new List<bool>();
            int i = 0;
            foreach (var db in m_DollsInBedsList)
            {
                m_Sleeping.Add(m_DollsInBeds[i].IsSleep);
                i++;
            }
            return m_Sleeping;
        }

        public void SetDollSleep(DollInBed dib)
        {
           
            m_DollsInBeds[dib.ID] = dib;
        }
        public void AddDollInBed(DollInBed dib)
        {
            m_DollsInBedsList.Add(dib);
            m_DollsInBeds = m_DollsInBedsList.ToArray();
        }
        public void SaveAllDolls()
        {
            Saver<DollInBed[]>.Save(fileName, m_DollsInBeds);
        }
        public bool GetDollInBed(int id)
        {
            return m_DollsInBeds[id].IsSleep;
        }

        public void InitSleeps()
        {

            m_DollsInBeds = m_SleepsPut;

            Saver<DollInBed[]>.TryLoad(fileName, ref m_DollsInBeds);

            m_DollsInBedsList = m_DollsInBeds.ToList();
        }

        public void WriteDollSleep(int id,  bool sleep)
        {
            m_DollsInBeds[id].SetSleepState(sleep);

            m_DollsInBedsList = m_DollsInBeds.ToList();
            m_Sleeping = new List<bool>();
            int i = 0;
            foreach (var db in m_DollsInBedsList)
            {
                m_Sleeping.Add(m_DollsInBeds[i].IsSleep);
                i++;
            }

            SaveAllDolls();
        }
        

        public bool GetSleepingByID(int id)
        {
           // if (!Instance) return false;
            
            return m_DollsInBeds[id].IsSleep;
        }
    }
}

