
using TowerDefense;
using SpaceShooter;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class EnemyWildlifeSpawner : Spawner
    {

     


        /// <summary>
        /// —сылки на что спавнить
        /// </summary>
        [SerializeField] private EnemyOrWildlife m_EnemyPrefab;


        [SerializeField] private Path m_Path;

        [SerializeField] private EnemyAsset[] m_EnemyAssets;


        

        protected override GameObject GenerateSpawnedEntity()
        {
            var e = Instantiate(m_EnemyPrefab);
            //e.Use(m_EnemyAssets[Random.Range(0, m_EnemyAssets.Length)]);
            e.GetComponent<GentAIConroller>().SetPath(m_Path);
            return e.gameObject;
        }

    }

}








        


  
