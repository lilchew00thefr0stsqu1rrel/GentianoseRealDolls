using UnityEngine;
using Common;
using TowerDefense;

namespace SpaceShooter
{
    public abstract class Spawner : MonoBehaviour
    {
        public enum SpawnMode
        {
            Start,
            Loop
        }

        /*
        /// <summary>
        /// —сылки на что спавнить
        /// </summary>
        [SerializeField] private Entity[] m_EntityPrefabs;
        */

        protected abstract GameObject GenerateSpawnedEntity();


        [SerializeField] private CylinderArea m_Area;

        [SerializeField] private SpawnMode m_SpawnMode;

        [SerializeField] private int m_NumSpawns;

        [SerializeField] private float m_RespawnTime;

        [SerializeField] private int m_TeamId;



        private float m_Timer;


        //[SerializeField] private EntitiesListManager m_Entities;

        private void Start()
        {
            //if (m_SpawnMode == SpawnMode.Start)
            {
                SpawnEnities();
            }

            m_Timer = m_RespawnTime;
        }

        private void Update()
        {
            if (m_Timer > 0)
                m_Timer -= Time.deltaTime;

            if (m_SpawnMode == SpawnMode.Loop && m_Timer <= 0)
            {
                SpawnEnities();

                m_Timer = m_RespawnTime;
            }
        }

        private void SpawnEnities()
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                var e = GenerateSpawnedEntity();
                e.transform.position = m_Area.GetRandomInsideZone();

            
               
            }
        }


    }
}
