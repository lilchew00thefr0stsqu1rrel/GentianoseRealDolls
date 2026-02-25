using UnityEngine;
using UnityEngine.SceneManagement;
using GentianoseRealDolls;
using System;

public class SceneHelper : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        sceneToLevel = m_ScenesAndLocations;


        print(AddressManager.Instance.Address.Waypoints[0]);
        

      
    }
    
   
    // Update is called once per frame
    void Update()
    {
        
    }

    static int index;
    
    public static Mode GameMode;

    private static int[] sceneToLevel = new int[4] { -1, 0, 1, 2};
    [SerializeField] private int[] m_ScenesAndLocations = new int[4] { -1, 0, 1, 2 };

    private static string baseHouse = "01-004+002+002";
    private static bool m_NotJustStart;
    public static void EnterHouse()
    {


        SceneManager.LoadScene(1);
        //print(Party.Instance.Address.Waypoints != null);
        if (m_NotJustStart)
        {
            Teleport(AddressManager.Instance.Address.Waypoints[0]);
        }
        

        GameMode = Mode.Habitat;
        index = 0;



        print("Enter!!");


    }

    public static void ExitHouse()
    {
        if (Party.Instance.AreThereSleepingBeasts)
        {
            print("Some dolls sleep and team can't go outdoor");
        }
        else
        {
            Teleport(AddressManager.Instance.Address.Waypoints[1]);

            SceneManager.LoadScene(2);
            GameMode = Mode.OpenWorld;
            index = 1;

            print(Party.Instance != null);

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


    public static void Teleport(string posString)
    {
        

        if (m_NotJustStart && Party.Instance.AreThereSleepingBeasts)
        {
            print("Some dolls sleep and team can't go outdoor");
        }
        else
        {
            int city = int.Parse(posString[..2]);
            int x = int.Parse(posString.Substring(2, 4));
            int y = int.Parse(posString.Substring(6, 4));
            int z = int.Parse(posString.Substring(10, 4));

            Vector3 pos = new Vector3(x, y, z);

            if (index != SceneToLevel(city))
            {
                SceneManager.LoadScene(city);
            }

            print("City: " + city + "  Legend: 1: Rusikova, 2: Kukly, 3: Punova");

            index = SceneToLevel(city);




            Party.Instance.PlaceDolls(1, pos);

            if (city == 1)
            {
                GameMode = Mode.Habitat;
            }
            if (city == 2)
            {
                GameMode = Mode.OpenWorld;
            }

            if (!m_NotJustStart)
            {
                m_NotJustStart = true;
            }

        }

    }

    public static void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
