using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TowerDefense;

namespace GentianoseRealDolls
{



public class SceneHelper : MonoBehaviour
{

        

     static int index;
    

    private static int[] sceneToLevel = new int[4] { -1, 0, 1, 2};
    [SerializeField] private int[] m_ScenesAndLocations = new int[4] { -1, 0, 1, 2 };

    private static string baseHouse = "01-004+002+002";
    private static bool m_NotJustStart;

    private static Party party;
    public void Construct(Party obj)
    {
        party = obj;
    }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
    {

        sceneToLevel = m_ScenesAndLocations;


        //print(AddressManager.Instance.Address.Waypoints[0]);
      
    }
    
   
    // Update is called once per frame
    void Update()
    {
        
    }
    public static void EnterHouse(int houseID)
    {
        SceneManager.LoadScene(houseID);


        //print(Party.Instance.Address.Waypoints != null);
        //if (m_NotJustStart)
        //{
        //    Teleport(AddressManager.Instance.Address.Waypoints[0]);
        //}
        

        //GameMode = Mode.Habitat;
        index = 0;



        print("Enter!!");
    }

    public static void ExitHouse()
    {
        if (party.AreThereSleepingBeasts)
        {
            print("Some dolls sleep and team can't go outdoor");
        }
        else
        {
                // Teleport(AddressManager.Instance.Address.Waypoints[1]);

                SceneManager.LoadScene(2);
                // GameMode = Mode.OpenWorld;
                index = 1;

            print(party != null);

            print("Exit!!");

        }
        
    }

    public static event Action OnEnterNewLevel;

    public static int SceneIndex()
    {
        return index;
    }
    public static int SceneToLevel(int scene)
    {
     
            return sceneToLevel[scene];
     
    }

    public static int LevelToScene(int level)
    {
            for (int i = 0; i < sceneToLevel.Length; i++)
            {
                if (sceneToLevel[i] == level) return i;
            }
            return 0;
    }


       


    public static void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
}
