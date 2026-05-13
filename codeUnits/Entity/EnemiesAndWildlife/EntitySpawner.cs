
using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class EntitySpawner : Spawner
    {

        /// <summary>
        /// Ссылки на что спавнить
        /// </summary>
        [SerializeField] private GameObject[] m_EntityPrefabs;


        [SerializeField] private Path m_Path;

        protected override GameObject GenerateSpawnedEntity()
        {
            return Instantiate(m_EntityPrefabs[Random.Range(0, m_EntityPrefabs.Length)]);
        }

    }

}








        


  
