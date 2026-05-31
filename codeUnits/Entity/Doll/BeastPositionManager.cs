using GentianoseRealDolls;
using SpaceShooter;
using System;
using System.Collections;
using UnityEngine;
using NTC.MonoCache;

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


    public void ResetLocation()
    {
        m_Location = 0;
    }

    public void ConstructDolls( AllDollPositions positions, AllDollSleeps sleeps)
    {
        allDollPositions = positions;
        allDollSleeps = sleeps;

        InitDoll();
    }


    private void InitDoll()
    {
        m_Doll = GetComponent<Doll>();
        dollID = m_Doll.DollID;
        /// начало бреда
        /// 
        
        m_DollPosition = allDollPositions.GetDollPos(dollID);

        if (m_DollPosition == null) 
        {
            m_DollPosition = new DollPosition(dollID, 1, Vector3.zero, Quaternion.identity);
        }

        m_Position = m_DollPosition.Position;

        m_Rotation = Quaternion.identity;


        StartCoroutine(AndSavePos());


    }

    

    // Update is called once per frame
    protected override void Run()
    {
        if (m_Position == null) return;

        m_Position = transform.position;


        m_DollPosition.dollID = m_Doll.DollID;

        m_DollPosition.Scene = Location;
        m_DollPosition.Position = m_Position;

        m_Rotation = transform.rotation;
        m_DollPosition.Rotation = m_Rotation;
    }

    public void WarpDoll(string address)
    {
        int scene = int.Parse(address[..2]);

        float x = float.Parse(address[2..10]);

        float y = float.Parse(address[10..18]);

        float z = float.Parse(address[18..]);
        print($"{x}; {y}; {z}");

        transform.position = new Vector3(x, y, z);

        SavePos();
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
    

    public void TakeAndSetDollPos(int loc, int index)
    {
        m_Location = loc;

        m_Position = allDollPositions.GetDollPositions(dollID) + new Vector3(0, 1.6f, 0);

        if (m_Position == null)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            transform.position = m_Position;
        }


        print("What " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);

    }

    private void OnDestroy()
    {

        m_Position = transform.position;
        m_Rotation = Quaternion.identity;

        m_DollPosition.Position = m_Position;
        m_DollPosition.Rotation = m_Rotation;

        SavePos();
    }

   
    public void SavePos()
    {
        m_DollPosition.dollID = dollID;
        m_DollPosition.Position = m_Position;
        m_DollPosition.Rotation = m_Rotation;
        m_DollPosition.Scene = m_Location;



        allDollPositions.SetDollPos(m_DollPosition);
    }

    private bool m_IsSleeping;
    public void TimeActionStats(long timeDifference)
    {
        m_IsSleeping = allDollSleeps.GetSleepingByID(dollID);

        if (m_IsSleeping)
        {
            //WarpDoll(m_Level.Beds[dollID]);
        }
        SavePos();
    }
}
