using System;
using System.Collections.Generic;
using UnityEngine;
namespace GentianoseRealDolls
{
    public enum Mode
    {
        Habitat,
        OpenWorld
    }
    public class Level : SingletonBase<Level>
    {
        


        [SerializeField] private Mode m_Mode;
        public Mode CurrentMode => m_Mode;

        [SerializeField] private Transform m_SpawnPoint;
        public Transform SpawnPoint => m_SpawnPoint;

        [SerializeField] private int levelID;
        public int LevelID => levelID;  



        public static event Action<int> OnEnterNewLevel;
        public static event Action OnExitNewLevel;

        public static int location = -336;
        public static bool fromWaypoint = false;
        public static bool fromMenu = false;

       [SerializeField] private string[] m_Beds;

        public string[] Beds => m_Beds;
        private new void Awake()
        {
            base.Awake();
            dolls = new List<Doll>();

            print("**\\**\\**\\");
            print(location + " Loctn");

        }

        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
           

            
            if (levelID != location || fromMenu)
            {
                location = levelID;

                //  PlayerInit.Instance.InitializeDolls(location);

                AllDollSleeps.Instance.InitSleeps();
                Party.Instance.InitDolls(location);
            }

            print(levelID);


        }

        // Update is called once per frame
        void Update()
        {

        }



        private List<Doll> dolls;

        public void AddDoll(Doll doll)
        {
            dolls.Add(doll);
        }

        public void DollsClear()
        {
            foreach (var d in dolls)
            {
                d.gameObject.SetActive(false);
            }
        }

        public static void SetArriveFromMenu()
        {
            fromMenu = true;
        }

    }

}
