using NTC.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerDefense;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GentianoseRealDolls
{
    /// <summary>
    /// C 14 05 2026 в Куклии возможно существование не более 336 единиц г**на
    /// Если какают сверх этого, кака удаляется, и приходят куклоны
    /// Координаты едины для домиков и городов
    /// Если другое измерение, то каки трассируются
    /// </summary>
    public class PoopStore : MonoBehaviour
    {
        private const int m_PoopNumber = 64;

        [Inject]
        public void Construct(Inventory obj)
        {
            m_Inventory = obj;
        }
        //private string fileName = "pooStore";
        private string path = "Assets/JSON/pooStore.dat";

        [SerializeField] private Poop m_PoopPrefab;
        [SerializeField] private Inventory m_Inventory
            ;
        [SerializeField] private Text m_DebugText;

        [SerializeField] private float[] m_DollModelToMass = {0.0001f, 0.0004f, 0.0016f} ;
        public float[] DollModelToMass => m_DollModelToMass;
        [SerializeField] private Vector3[] m_DollIDToScale;
        public Vector3[] DollIDToScale => m_DollIDToScale;

        [SerializeField]
        private string[] m_FieldNames = new string[]
  {
        "dollID",
        "mapID",
        "x",
        "y",
        "z"

  };

        [Serializable]
        private class PoopPosition
        {
            public float x, y, z;

            public int m_Size;
            public int m_DollID;
            public PoopPosition(Vector3 position, int poopSize, int dollD)
            {
                this.x = position.x;
                this.y = position.y;
                this.z = position.z;
                m_Size = poopSize;
                m_DollID = dollD;
            }

            public Vector3 GetPoopPosition()
            {
                return new Vector3(x, y, z);
            }

        }

        Level level;

        [SerializeField] private List<Poop> m_PooList;
        [SerializeField] private Poop[] m_PooArray;

        [SerializeField] private List<int> m_PooPosIntList;

        [SerializeField] private Mesh[] m_PooShapePrefabs;
        [SerializeField] private Poop[] m_PooPrefabs;
        [SerializeField] private Material[] m_PooMaterials;

        [SerializeField] private DollBase m_DollBase;
        [SerializeField] private int m_Caret;

        private  void Awake()
        {
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }
       
        public void InitPoop()
        {

            m_PooArray = new Poop[m_PoopNumber];
            
            m_PooPosIntList = m_DollBase.GetAllRecords("poop", m_FieldNames);
            int numPoop = m_PooPosIntList.Count / 5;
            for (int i = 0; i < numPoop; i++) 
            {
                int[] poopData = new int[5];
                poopData[0] = m_PooPosIntList[i*5];
                poopData[1] = m_PooPosIntList[i*5+1];
                poopData[2] = m_PooPosIntList[i*5+2];
                poopData[3] = m_PooPosIntList[i*5+3];
                poopData[4] = m_PooPosIntList[i*5+4];

                int pooDollID = poopData[0];

                Vector3 poopPos = new Vector3(poopData[2] / 100f, (poopData[3] + 0.5f)/ 100, poopData[4] / 100f);
                if (m_PooPrefabs[poopData[0]])
                {
                    var poop = NightPool.Spawn(m_PooPrefabs[poopData[0]], poopPos, Quaternion.identity);

                    m_PooArray[m_Caret] = poop; 

                    if (m_Caret < m_PoopNumber - 1)
                        m_Caret++;
                }
            }
        }

        private int PoopAmount;



        // Update is called once per frame
        void Update()
        {

            PoopAmount = m_PooList.Count;

        }


        private void SavePoopArray()
        {
            m_DollBase.AddOrChangeRecord("DELETE FROM poop");

            //m_PooPosArray = m_PooPositions.ToArray();

            int numPoop = m_PooPosIntList.Count / 5;

            for (int i = 0; i < numPoop; i++)
            {
                int[] poopData = new int[5];
                poopData[0] = m_PooPosIntList[i * 5];
                poopData[1] = m_PooPosIntList[i * 5 + 1];
                poopData[2] = m_PooPosIntList[i * 5 + 2];
                poopData[3] = m_PooPosIntList[i * 5 + 3];
                poopData[4] = m_PooPosIntList[i * 5 + 4]; 
                
                

                m_DollBase.AddOrChangeRecord("INSERT OR IGNORE INTO poop " +
                    "(dollID, mapID, x, y, z) " +
                    "VALUES ('" + poopData[0] +
                        "', '" + poopData[1] + "', '" + poopData[2] + "', '" 
                        + poopData[3] + "', '" + poopData[4] + "');");
            }


            print("Bola");
        }
        public void SavePoop()
        {
            SavePoopArray();
        }
        
        public void GoPoopToSilverWhiteTree()
        {
            print("Requiem of Apep's Auspicious Phlogiston");
            int i = 0;
            foreach (var poop in m_PooArray)
            {
                if (poop != null)
                {
                    m_Inventory.AddKuklons(poop.Size * 300);
                    NightPool.Despawn(poop.gameObject);
                    i++;
                }
            }
            InventoryController.Instance.InitAllItems();


            m_PooPosIntList.Clear();

            SavePoopArray();

        }
        public void AddPoop(Poop poop)
        {
            m_PooArray[m_Caret] = poop;

            if (m_Caret < m_PoopNumber - 1)
                m_Caret++;


            m_PooPosIntList.Add(poop.DollID);
            m_PooPosIntList.Add(poop.Size);
            m_PooPosIntList.Add((int)(poop.transform.position.x * 100));
            m_PooPosIntList.Add((int)(poop.transform.position.y * 100));
            m_PooPosIntList.Add((int)(poop.transform.position.z * 100));

            SavePoopArray();
        }


    }

}
