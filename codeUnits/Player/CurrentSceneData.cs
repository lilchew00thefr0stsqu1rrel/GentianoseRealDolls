using GentianoseRealDolls;
using TowerDefense;
using UnityEngine;

public enum Mode
{
    Habitat,
    OpenWorld
}

public class CurrentSceneData : MonoBehaviour
{


    private void Awake()
    {
        ReadCurrScene();
    }

    [SerializeField] private int m_LocationIndex;
    public int LocationIndex => m_LocationIndex;

    [SerializeField] private Mode m_GameMode;
    public Mode GameMode => m_GameMode;

    [SerializeField] private UnityEngine.UI.Text m_DebugText;
    public void SetLocationIndex(int index)
    {
        if (m_LocationIndex < 0) return;
        m_LocationIndex = index;

        if (m_LocationIndex == 0 || m_LocationIndex == 2)
            m_GameMode = Mode.Habitat;
        if (m_LocationIndex == 1)
            m_GameMode = Mode.OpenWorld;

        WriteCurrScene();
    }
    public void WriteCurrScene()
    {
        Saver<int>.Save(WhooSettings.fileNameLoc, m_LocationIndex);
    }
    public void ReadCurrScene()
    { 
        Saver<int>.TryLoad(WhooSettings.fileNameLoc, ref m_LocationIndex);
    }
}
