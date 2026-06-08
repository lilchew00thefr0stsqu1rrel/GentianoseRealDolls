using GentianoseRealDolls;
using SpaceShooter;
using System;
using System.Collections;
using UnityEngine;
using NTC.MonoCache;
using System.ComponentModel;

/// <summary>
/// Положение зверька определяется 1 целым числом (сцена-локация, город, домик) и 1 вектором-3.  
/// </summary>
[RequireComponent (typeof(Doll))]
public class BeastPositionManager : MonoCache
{
    

    [SerializeField] private AllDollSleeps allDollSleeps;
    [SerializeField] private AllDollPositions allDollPositions;


    [SerializeField] private DollPosition m_DollPosition;

    [SerializeField] private int m_Location;
    public int Location => m_Location;

    private Doll m_Doll;

    private Vector3 m_Position;
    private Quaternion m_Rotation;

    private int dollID;

    private Vector3 m_WaypointWarpPosition;
    [SerializeField] private int m_MapsNumber = 3;

    [SerializeField] private int[] m_LocPos;
    private void Awake()
    {
        m_LocPos = new int[5];

        m_LocPos[1] = m_Location;
    }

    public void ResetLocation()
    {
        m_Location = 0;
    }

    public void Fill(int[] pos)
    {
        m_LocPos = pos;
        
        m_Location = m_LocPos[1];

        transform.position = new Vector3(m_LocPos[2], m_LocPos[3], m_LocPos[4]);
    }

    public int[] Fetch()
    {
        m_LocPos[2] = (int)Mathf.Ceil(transform.position.x);
        m_LocPos[3] = (int)Mathf.Ceil(transform.position.y);
        m_LocPos[4] = (int)Mathf.Ceil(transform.position.z);

        return m_LocPos;    
    }


    // Update is called once per frame
    protected override void Run()
    {
        if (m_Position == null) return;
        if (m_DollPosition == null) return;

        m_Position = transform.position;

    }

    public void WarpDoll(string address)
    {
        //int scene = int.Parse(address[..2]);

        //float x = float.Parse(address[2..10]);

        //float y = float.Parse(address[10..18]);

        //float z = float.Parse(address[18..]);
        //print($"{x}; {y}; {z}");

        //transform.position = new Vector3(x, y, z);

        //SavePos();
    }

    IEnumerator AndSavePos()
    {

        int numberOfLocations = Doll.LocationsNumber;

        m_DollPosition.Position = new Vector3();
        for (int i = 0; i < numberOfLocations; i++)
        {
            m_DollPosition.Position[i] = m_Position[i];

        }


        yield return new WaitForSeconds(1);

        StartCoroutine(AndSavePos());
    }


    /// <summary>
    /// Телепортировать зверьков 
    /// </summary>
    /// <param name="loc">локация (домик или город)</param>
    /// <param name="waypoint">координаты точки телепортации</param>
    /// <param name="index">индекс куклы (нужен для того, чтобы зверьки (вся команда) не были отправлены в одну точку)"</param>
    
    public void SetDollPosFromWaypoint(int loc, Vector3 waypoint, int index)
    {
        print("Warp " + waypoint.x + ", " + waypoint.y + ", " + waypoint.z);
        m_WaypointWarpPosition = waypoint + index * Vector3.right;
        transform.SetPositionAndRotation(m_WaypointWarpPosition, m_Rotation);

        print("What " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);

        m_Location = loc;
    }
    

    private void OnDestroy()
    {


    }

   
    public void SavePos()
    {
        m_LocPos[0] = dollID;
        m_LocPos[1] = m_Location;
        m_LocPos[2] = (int)Mathf.Ceil(transform.position.x);
        m_LocPos[3] = (int)Mathf.Ceil(transform.position.y);
        m_LocPos[4] = (int)Mathf.Ceil(transform.position.z);


        OnSavePos(m_LocPos);
    }

    public static event Action<int[]> OnSavePos;

    private bool m_IsSleeping;
}
