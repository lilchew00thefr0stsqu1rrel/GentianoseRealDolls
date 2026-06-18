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

        [SerializeField] private PoopPosition[] m_PooPosArray;
        [SerializeField] private List<PoopPosition> m_PooPositions;
        [SerializeField] private List<Poop> m_PooList;
        [SerializeField] private List<int> m_PooPosIntList;

        [SerializeField] private Mesh[] m_PooShapePrefabs;
        [SerializeField] private Poop[] m_PooPrefabs;
        [SerializeField] private Material[] m_PooMaterials;

        [SerializeField] private DollBase m_DollBase;

        private  void Awake()
        {
            // level = GetComponent<Level>();
            //m_PooPosArray = new PoopPosition[31];
            // m_PooPositions = new List<PoopPosition>();
            //m_PooList = new List<Poop>();


            //Saver<PoopPosition[]>.TryLoad(WhooSettings.fileNamePoo, ref m_PooPosArray);
            //m_PooPositions = m_PooPosArray.ToList();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            //if (m_PoopPrefab)
            //{
            //    foreach (var poopPos in m_PooPositions)
            //    {
            //        var poop = Instantiate(m_PoopPrefab, poopPos.GetPoopPosition(), Quaternion.identity);
            //        poop.SetShape(poopPos.m_DollID, m_PooMaterials[poopPos.m_DollID],
            //            m_PooShapePrefabs[poopPos.m_DollID], poopPos.m_Size,
            //            poopPos.GetPoopScale(), poopPos.m_Mass);
            //        m_PooList.Add(poop);
            //    }

            //}

        }
       
        public void InitPoop()
        {
            //m_PooPositions = new List<PoopPosition>();
            //m_PooList = new List<Poop>();

            //Saver<PoopPosition[]>.TryLoad(WhooSettings.fileNamePoo, ref m_PooPosArray);


            //if (m_PooPosArray == null)
            //{
            //    m_PooPosArray= new PoopPosition[0];
            //}

            //m_PooPositions = m_PooPosArray.ToList();

            //if (m_PooPosArray.Length > 0)
            //{
            //    if (m_PoopPrefab)
            //    {
            //        foreach (var poopPos in m_PooPositions)
            //        {
            //            var poop =  NightPool.Spawn(m_PoopPrefab, poopPos.GetPoopPosition(), Quaternion.identity);
            //            poop.SetShape(poopPos.m_DollID, m_PooMaterials[poopPos.m_DollID],
            //                m_PooShapePrefabs[poopPos.m_DollID], poopPos.m_Size,
            //                m_DollIDToScale[poopPos.m_DollID], m_DollModelToMass[poopPos.m_Size - 1]);
            //            m_PooList.Add(poop);
            //        }

            //    }

            //    m_PooPositions = m_PooPosArray.ToList();
            //}
            
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
                if (m_PoopPrefab)
                {
                    var poop = NightPool.Spawn(m_PooPrefabs[poopData[0]], poopPos, Quaternion.identity);
                }
            }
            

        }

        private int PoopAmount;



        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    SavePoop();
            //}

            PoopAmount = m_PooList.Count;

        }



        //private List<GameObject> m_PooList;


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

            //Saver<PoopPosition[]>.Save(WhooSettings.fileNamePoo, m_PooPosArray);

            print("Bola");
        }
        public void SavePoop()
        {
            SavePoopArray();
        }
        
        public void GoPoopToSilverWhiteTree()
        {
            m_Inventory.AddKuklons(m_PooList.Count * 500);
            InventoryController.Instance.InitAllItems();

            foreach (var poop in m_PooList)
            {
                NightPool.Despawn(poop.gameObject);
            }
            m_PooList.Clear();  

            m_PooPositions.Clear();
            m_PooPosArray = m_PooPositions.ToArray() ;

            m_PooPosIntList.Clear();

            SavePoopArray();

        }

        public void AddPoop(Poop poop)
        {
            m_PooList.Add(poop);
            ////m_PooPositions.Add(new PoopPosition(poop.transform.position, poop.Size, poop.DollID));

            if (m_PooList.Count > 72)
            {
                NightPool.Despawn(m_PooList[0]);
                m_PooList.RemoveAt(0);
            }
            m_PooList.Clear();

            m_PooPosIntList.Add(poop.DollID);
            m_PooPosIntList.Add(1);
            m_PooPosIntList.Add((int)(poop.transform.position.x * 100));
            m_PooPosIntList.Add((int)(poop.transform.position.y * 100));
            m_PooPosIntList.Add((int)(poop.transform.position.z * 100));

            SavePoopArray();
        }


    }

}
