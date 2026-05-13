using SpaceShooter;
using UnityEngine;
using TowerDefense;

public class SceneUnitPatrols : SingletonBase<SceneUnitPatrols>
{
    [SerializeField] private Path[] m_PatrolPaths;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignUnitPath(Destructible unit)
    {
        // Êàáàíèé ïóòü
        if (unit.UnitID == "20001")
            unit.SetPath(m_PatrolPaths[0]);
        // Ëîñîñÿ ïóòü
        if (unit.UnitID == "20201")
            unit.SetPath(m_PatrolPaths[1]);
    }
}
