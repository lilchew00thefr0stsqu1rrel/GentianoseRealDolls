using Common;
using GentianoseRealDolls;
using TowerDefense;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NTC.Pool;
using System.Threading.Tasks;

namespace GentianoseRealDolls
{
    public class Destructible : DestructibleBase
    {
        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;


        protected override void Use(EnemyAsset asset)
        {
            base.Use(asset);
            m_ScoreValue = asset.score;
        }
        
        // GRD 5XII2025
        [SerializeField] private InventoryItem m_Drop;

        [SerializeField] private int m_DropAmount;

        [SerializeField] private GameObject m_DropPrefab;
        [Tooltip("20001 - boar, 20201 - salmon")]
        [SerializeField] private string m_UnitID;
        public string UnitID => m_UnitID;
        [SerializeField] private Path m_Path;
        public void SetPath(Path path)
        {
            m_Path = path;
        }
        public void DropItems()
        {
            print("***Drop***");
            if (gameObject != null && m_Drop != null)
            {
                //Inventory.Instance.AddItemInstances(m_Drop, m_DropAmount);
                print(m_DropAmount);
                //InventoryController.Instance.InitAllItems();

                // Instantiate(m_DropPrefab, transform.position, Quaternion.identity);

                NightPool.Spawn(m_DropPrefab, transform.position, Quaternion.identity);
            }
        }

        int m_CurrentStatus;

        //
        [SerializeField] List<int> m_CurrentStatusList;


        // Stinky
        private const int SprayLiquidDamage = 134;
        private const int SpraySmellDamage = 67;

        // Heal
        private const int Heal = 168;


        public async void ApplyDamageOverTime(int damage, int durationSeconds)
        {
            for (int i = 0; i < durationSeconds; i++)
            {
                ApplyDamage(damage);

                

                await Task.Delay(1000);


            }


        }
        public void ApplyBuff()
        {


            IEnumerator HealDolls()
            {

                for (int i = 0; i < 3; i++)
                {
                    print(gameObject.name + 168);
                    RestoreHitPoints(168);
                   
                    yield return new WaitForSeconds(1);
                }
                m_CurrentStatus = 0;
            }


          
            StartCoroutine(HealDolls());
            


        }

        private bool m_OffField;


        public new void ApplyDamage(int damage)
        {
            if (!m_OffField)
            {
                base.ApplyDamage(damage);


               
            }
        }

        public void NavelEffect(bool enab)
        {
            m_OffField = enab;
        }

        private void Update()
        {
            

            // Spray cloud
           

        }
    }
}

