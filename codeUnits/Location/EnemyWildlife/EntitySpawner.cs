
using TowerDefense;
using UnityEngine;

namespace SpaceShooter
{
    public class EntitySpawner : Spawner
    {

        /// <summary>
        /// —сылки на что спавнить
        /// </summary>
        [SerializeField] private GameObject[] m_EntityPrefabs;


        [SerializeField] private Path m_Path;

        protected override GameObject GenerateSpawnedEntity()
        {
            return Instantiate(m_EntityPrefabs[Random.Range(0, m_EntityPrefabs.Length)]);
        }

    }

}








        


  
