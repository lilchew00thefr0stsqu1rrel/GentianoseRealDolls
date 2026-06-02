using GentianoseRealDolls;
using NUnit.Framework.Internal;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

// This script is primary gate to Dollia

public class TeleportBeasts : MonoBehaviour, ISceneGate
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

    [SerializeField]
    private string[] m_Beds = new string[]
    {
        "01+00001,2+00004,0+00002,8",
        "01+00001,2+00004,0-00000,4",
        "01+00004,8+00000,5-00001,5"
    };
    public string[] Beds=> m_Beds;

    [Inject]
    public void Construct(AllDollCharacters obj)
    {
        m_AllCharacters = obj;
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
            int city = stringCoordinates.GetLocationFromString(posString);

            Vector3 pos = stringCoordinates.GetPositionFromString(posString);

            //  откуда                          куда
            if (currentScene.LocationIndex != SceneHelper.SceneToLevel(city))
            {
                SceneManager.LoadScene(city);

                m_Party.InitDolls(SceneHelper.SceneToLevel(city), m_AllCharacters.ReadStats(),
                    m_AllCharacters, m_AllPositions, m_AllSleeps.ReadSleeping(), m_AllSleeps, 0);
            }

            //  При перемещении в пределах домика
            if (currentScene.LocationIndex == SceneHelper.SceneToLevel(city))
            {
                m_Party.PlaceSomeOrAllDolls(SceneHelper.SceneToLevel(city), pos);
            }

            print("City: " + city + "  Legend: 1: Rusikova, 2: Kukly, 3: Punova");

            currentScene.SetLocationIndex(SceneHelper.SceneToLevel(city));

            if (!m_NotJustStart)
            {
                m_NotJustStart = true;
            }
        }

    }


    private void OnDestroy()
    {
        print("No! Whew");
    }

    public void EnterScene(int levelID)
    {
        Level.SetArriveFromMenu();

        SceneManager.LoadScene(stringCoordinates.LevelsAsScenes[levelID]);

        m_I = 0.12345679f;
    }

    public void InitScene(int levelID)
    {
        Level.SetArriveFromMenu();

        print("~~~!!!!~~~~" + m_AllCharacters.ReadStats()[0][0]);

        m_Party.InitDolls(levelID, m_AllCharacters.ReadStats(), m_AllCharacters, m_AllPositions,
            m_AllSleeps.ReadSleeping(), m_AllSleeps, m_TimePastStats.ReadTime());
        
        currentScene.SetLocationIndex(levelID);

        print("City: " + levelID + "  Legend: 0: Rusikova, 1: Kukly, 2: Punova");

        m_I = 0.12345679f;

        print("Chno Whew!");
    }
}
