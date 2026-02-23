using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class PoopStore : SingletonBase<PoopStore>
    {

        private string fileName = "pooStore";

        [SerializeField] private Poop m_PoopPrefab;

        [Serializable]
        private class PoopPosition
        {
            public float x, y, z;

            public float l, b, h;

            public int m_Size;
            public int m_DollID;
            public float m_Mass;
            public PoopPosition(Vector3 position, int poopSize, int dollD, Vector3 scale, float mass)
            {
                this.x = position.x;
                this.y = position.y;
                this.z = position.z;
                m_Size = poopSize;
                m_DollID = dollD;

                l = scale.x;
                b = scale.y;
                h = scale.z;

                m_Mass = mass;
            }

            public Vector3 GetPoopPosition()
            {
                return new Vector3(x, y, z);
            }
            public Vector3 GetPoopScale()
            {
                return new Vector3(l, b, h);
            }

        }

        Level level;

        private PoopPosition[] m_PooPosArray;
        private List<PoopPosition> m_PooPositions;
        private List<Poop> m_PooList;

        [SerializeField] private Mesh[] m_PooShapePrefabs;
        [SerializeField] private Material[] m_PooMaterials;
        private new void Awake()
        {
            base.Awake(); 
            
            level = GetComponent<Level>();
            m_PooPosArray = new PoopPosition[31];
            m_PooPositions = new List<PoopPosition>(); 
            m_PooList = new List<Poop>();
            
          
            Saver<PoopPosition[]>.TryLoad(fileName, ref m_PooPosArray);
            m_PooPositions = m_PooPosArray.ToList();
            
            
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
            if (m_PoopPrefab)
            {
                foreach (var poopPos in m_PooPositions)
                {
                    var poop = Instantiate(m_PoopPrefab, poopPos.GetPoopPosition(), Quaternion.identity);
                    poop.SetShape(poopPos.m_DollID, m_PooMaterials[poopPos.m_DollID],
                        m_PooShapePrefabs[poopPos.m_DollID], poopPos.m_Size,
                        poopPos.GetPoopScale(), poopPos.m_Mass
                        );
                    m_PooList.Add(poop);
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
            m_PooPositions.Clear();

            if (m_PooList != null)
            {
                foreach (var poop in m_PooList)
                {
                    if (poop)
                    {

                        m_PooPositions.Add(new PoopPosition(poop.transform.position, poop.Size, poop.DollID, poop.Scale, 
                            poop.Mass));
                    }
                }
                m_PooPosArray = m_PooPositions.ToArray();
            }
          

            Saver<PoopPosition[]>.Save(fileName, m_PooPosArray);

            print("Bola");
        }
        public static void SavePoop()
        {
            print(Instance == null);
            Instance.SavePoopArray();
        }

        public void GoPoopToSilverWhiteTree()
        {
            Inventory.Instance.AddKuklons(m_PooList.Count * 500);
            InventoryController.Instance.InitAllItems();

            foreach (var poop in m_PooList)
            {
                DestroyImmediate(poop.gameObject);
            }
            m_PooPositions.Clear();
            m_PooPosArray = m_PooPositions.ToArray();
            Saver<PoopPosition[]>.Save(fileName, m_PooPosArray);

        }

        public void AddPoop(Poop poop)
        {
            m_PooList.Add(poop);
        }

 
    }

}
