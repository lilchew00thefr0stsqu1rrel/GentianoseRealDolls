
using TowerDefense;
using SpaceShooter;
using UnityEngine;
using NTC.Pool;
using VContainer;

namespace GentianoseRealDolls
{
    public class EnemyWildlifeSpawner : Spawner
    {



        [SerializeField] private Dashboard dashboard;
        [Inject]
        public void Construct(Dashboard dashboard)
        {
            this.dashboard = dashboard;
        }

        /// <summary>
        /// Ссылки на что спавнить
        /// </summary>
        [SerializeField] private EnemyOrWildlife m_EnemyPrefab;


        [SerializeField] private Path m_Path;

        [SerializeField] private EnemyAsset[] m_EnemyAssets;



        protected override GameObject GenerateSpawnedEntity()
        {
            var e = NightPool.Spawn(m_EnemyPrefab);
            //e.Construct(dashboard);
            //e.Use(m_EnemyAssets[Random.Range(0, m_EnemyAssets.Length)]);
            if (e != null)
            {
                e.GetComponent<GentAIConroller>().SetPath(m_Path);
                return e.gameObject;
            }
            return null;
        }

    }

}
