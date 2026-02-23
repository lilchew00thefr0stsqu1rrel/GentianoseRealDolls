using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class AllDollSleeps : SingletonBase<AllDollSleeps>
    {


        private static int m_DollsInGame = 1;
        [SerializeField] private DollInBed[] m_DollsInBeds;

        private List<DollInBed> m_DollsInBedsList = new List<DollInBed>();
        [SerializeField] private DollInBed[] m_SleepsPut; 
        private string fileName = "dInBeds.dat";
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

        protected new void Awake()
        {
            base.Awake();


            Saver<DollInBed[]>.TryLoad(fileName, ref m_DollsInBeds);

            m_DollsInBedsList = m_DollsInBeds.ToList();
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
        public void SaveAllDollSleeps()
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
        }

        public void WriteDollSleep(int id,  bool sleep)
        {
            m_DollsInBeds[id].SetSleepState(sleep);
            Saver<DollInBed[]>.Save(fileName, m_DollsInBeds);




        }
        

        public static bool GetSleepingByID(int id)
        {
            if (!Instance) return false;
            
            return Instance.m_DollsInBeds[id].IsSleep;

            
               

        }
    }
}

