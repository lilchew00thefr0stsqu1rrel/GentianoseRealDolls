using GentianoseRealDolls;
using SpaceShooter;
using System;
using System.Collections;
using UnityEngine;
using NTC.MonoCache;

[RequireComponent (typeof(Doll))]
public class BeastPositionManager : MonoCache
{
    

    [SerializeField] private AllDollCharacters allDolls;
    [SerializeField] private AllDollSleeps allDollSleeps;
    [SerializeField] private AllDollPositions allDollPositions;


    [SerializeField] private DollPositions m_DollPositions;

    [SerializeField] private int m_Location;
    public int Location => m_Location;

    private Doll m_Doll;

    private Vector3[] m_Positions;
    private Quaternion m_Rotation;

    private int dollID;

    private Vector3 m_WaypointWarpPosition;
    [SerializeField] private int m_MapsNumber = 3;


    public void ResetLocation()
    {
        m_Location = 0;
    }

    public void ConstructDolls(AllDollCharacters dolls, AllDollPositions positions, AllDollSleeps sleeps)
    {
        allDolls = dolls;
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

        if (allDollPositions.GetDollPositions(dollID).Length == 0)
        {
            m_Positions = new Vector3[m_MapsNumber];
            print("Created positions file");
        }
        else
        {
            m_Positions = allDollPositions.GetDollPositions(dollID);
            print("Position fetch~");
        }
        m_Rotation = Quaternion.identity;


        StartCoroutine(AndSavePos());


        if (m_DollPositions == null)
        {
            m_DollPositions = new DollPositions();
            allDollPositions.AddDollPos(m_DollPositions);
        }
    }

    

    // Update is called once per frame
    protected override void Run()
    {
        if (m_Positions == null) return;

        m_Positions[m_Location] = transform.position;


        m_DollPositions.dollID = m_Doll.DollID;

        m_DollPositions.Scene = Location;
        m_DollPositions.Positions = m_Positions;

        m_Rotation = transform.rotation;
        m_DollPositions.Rotation = m_Rotation;
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

        m_DollPositions.Positions = new Vector3[numberOfLocations];
        for (int i = 0; i < numberOfLocations; i++)
        {
            m_DollPositions.Positions[i] = m_Positions[i];

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

        m_Positions = allDollPositions.GetDollPositions(dollID);

        if (m_Positions[loc] == null)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            transform.position = m_Positions[loc];
        }


        print("What " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);

    }

    private void OnDestroy()
    {

        m_Positions[m_Location] = transform.position;
        m_Rotation = Quaternion.identity;

        m_DollPositions.Positions = m_Positions;
        m_DollPositions.Rotation = m_Rotation;

        SavePos();
    }

   
    public void SavePos()
    {
        m_DollPositions.dollID = dollID;
        m_DollPositions.Positions = m_Positions;
        m_DollPositions.Rotation = m_Rotation;
        m_DollPositions.Scene = m_Location;



        allDollPositions.SetDollPos(m_DollPositions);
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
