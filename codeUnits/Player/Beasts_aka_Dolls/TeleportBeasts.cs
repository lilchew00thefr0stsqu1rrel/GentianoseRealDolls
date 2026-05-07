using GentianoseRealDolls;
using NUnit.Framework.Internal;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

// This script is primary gate to Dollia

public class TeleportBeasts : MonoBehaviour,  ISceneGate
{
    [SerializeField] private StringCoordinates stringCoordinates;
    [SerializeField] private CurrentSceneData currentScene;

    [SerializeField] private float m_I;

    [SerializeField] private AllDollSleeps m_AllSleeps;
    [SerializeField] private AllDollPositions m_AllPositions;
    [SerializeField] private AllDollCharacters m_AllCharacters;
    [SerializeField] private TimePastStats m_TimePastStats;

    [SerializeField] private Party m_Party;

    

    [SerializeField] private string[] m_Houses = new string[]
    {
        "02+00764,4+00024,7+00759,4",
        "02+01123,4+00029,6+01453,2"
    };

    //[Inject]
    public void Construct(AllDollSleeps obj)
    {
        m_AllSleeps = obj;
    }

    //[Inject]
    public void Construct(CurrentSceneData obj)
    {
        currentScene = obj;
    }

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    int levelID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    private bool m_NotJustStart;

    

    public void Teleport(string posString, bool someBeastsSleep)
    {


        if (m_NotJustStart && someBeastsSleep)
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

            print(" City " + SceneHelper.SceneToLevel(city));



            //  откуда                          куда
            if (currentScene.LocationIndex != SceneHelper.SceneToLevel(city))
            {

                SceneManager.LoadScene(city);


                //party.PlaceDolls(1, pos);

            }
            //  При перемещении в пределах домика
            if (currentScene.LocationIndex == SceneHelper.SceneToLevel(city))
            {
                //party.PlaceSomeDolls(1, pos);

            }

            print("City: " + city + "  Legend: 1: Rusikova, 2: Kukly, 3: Punova");

            currentScene.SetLocationIndex(SceneHelper.SceneToLevel(city));


           

            if (!m_NotJustStart)
            {
                m_NotJustStart = true;
            }


        }

    }

    public void TeleportLng(string posString, bool someBeastsSleep)
    {


        if (m_NotJustStart && someBeastsSleep)
        {
            print("Some dolls sleep and team can't go outdoor");

        }
        else
        {
            int city = stringCoordinates.GetLocationFromString(posString);

            Vector3 pos = stringCoordinates.GetPositionFromString(posString);

            print(" City " + SceneHelper.SceneToLevel(city));

            //  откуда                          куда
            if (currentScene.LocationIndex != SceneHelper.SceneToLevel(city))
            {
                SceneManager.LoadScene(city);

                m_Party.InitDolls(SceneHelper.SceneToLevel(city), 
                    m_AllCharacters, m_AllPositions, m_AllSleeps, m_TimePastStats.ReadTime());    

                // m_Party.PlaceDolls(SceneHelper.SceneToLevel(city), pos);
            }

            //  При перемещении в пределах домика
            if (currentScene.LocationIndex == SceneHelper.SceneToLevel(city))
            {
                m_Party.PlaceSomeDolls(SceneHelper.SceneToLevel(city), pos);
            }

            print("City: " + city + "  Legend: 1: Rusikova, 2: Kukly, 3: Punova");

            currentScene.SetLocationIndex(SceneHelper.SceneToLevel(city));



            if (!m_NotJustStart)
            {
                m_NotJustStart = true;
            }
        }
    }

    public void EnterScene(int levelID)
    {
        Level.SetArriveFromMenu();

        SceneManager.LoadScene(stringCoordinates.LevelsAsScenes[levelID]);


        m_Party.InitDolls(levelID, m_AllCharacters, m_AllPositions, m_AllSleeps, m_TimePastStats.ReadTime());

        currentScene.SetLocationIndex(levelID);

        print("City: " + levelID + "  Legend: 0: Rusikova, 1: Kukly, 2: Punova");


        m_I = 0.12345679f;
    }


}
