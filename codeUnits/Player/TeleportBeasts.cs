using GentianoseRealDolls;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using UnityEngine.UI;
using System.Threading.Tasks;

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
    [SerializeField] private Inventory m_Inventory;
    [SerializeField] private PoopStore m_PoopStore;

    [SerializeField] private Party m_Party;

    [SerializeField] private LevelAsset[] m_Levels;
    public LevelAsset[] Levels => m_Levels;

    [SerializeField] private Text m_DebugText;


    [Inject]
    public void Construct(AllDollCharacters obj)
    {
        m_AllCharacters = obj;
    }

    private bool m_NotJustStart;



    public async void Teleport(string posString, bool someBeastsSleep)
    {
        if (m_NotJustStart && someBeastsSleep)
        {
            print("Some dolls sleep and team can't go outdoor");
        }

        else
        {
            int city = stringCoordinates.GetLocationFromString(posString);

            Vector3 pos = stringCoordinates.GetPositionFromString(posString);

            int lv = SceneHelper.SceneToLevel(city);
            //  откуда                          куда
            if (currentScene.LocationIndex != SceneHelper.SceneToLevel(city))
            {
                SceneManager.LoadScene(city);

                m_Party.InitDolls(lv, 0L);

                await Task.Delay(1000);
                m_Party.PlaceSomeOrAllDolls(lv, pos);
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

        m_Party.InitDolls(levelID, m_TimePastStats.ReadTime());

        currentScene.SetLocationIndex(levelID);

        print("City: " + levelID + "  Legend: 0: Rusikova, 1: Kukly, 2: Punova");

        m_I = 0.12345679f;

        print("Chno Whew!");
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            InitScene(currentScene.LocationIndex);
        }
    }

}
