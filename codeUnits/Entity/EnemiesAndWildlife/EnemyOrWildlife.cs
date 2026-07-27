using NTC.Pool;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class EnemyOrWildlife : MonoBehaviour
    {
        [SerializeField] private GiveResource m_Yield;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //Despawn();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator Despawn()
        {
            yield return new WaitForSeconds(60);

            var colls = Physics.OverlapSphere(transform.position, 100);

            bool partyHere = false;

            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].GetComponent<Party>())
                {
                    partyHere = true;
                }
            }

            if (!partyHere)
            {
                NightPool.Despawn(gameObject);
            }
        }
    }
}

