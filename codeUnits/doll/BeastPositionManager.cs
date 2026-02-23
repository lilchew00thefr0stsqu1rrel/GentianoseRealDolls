using GentianoseRealDolls;
using SpaceShooter;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Doll))]
public class BeastPositionManager : MonoBehaviour
{

    [SerializeField] private DollPositions m_DollPositions;

  //  [SerializeField] private DollCurrentStats m_CurrentStats;

    [SerializeField] private int m_Location;
    public int Location => m_Location;

    private Doll m_Doll;

    private Vector3[] m_Positions;
    private Quaternion m_Rotation;

    private int dollID;

    private Vector3 m_WaypointWarpPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Doll = GetComponent<Doll>();
        dollID = m_Doll.DollID;

        if (AllDollCharacters.Instance.GetDollPositions(dollID).Length == 0)
        {
            m_Positions = new Vector3[2];
        }
        else
        {
            m_Positions = AllDollCharacters.Instance.GetDollPositions(dollID);
        }
        m_Rotation = new Quaternion();

        StartCoroutine(AndSavePos());


        if (m_DollPositions == null)
        {
            m_DollPositions = new DollPositions();
            AllDollCharacters.Instance.AddDollPos(m_DollPositions);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        //print(address[2..10]);
        //  print(address[10..18]);
        // print(address[18..26]);
        float x = float.Parse(address[2..10]);

        float y = float.Parse(address[10..18]);

        float z = float.Parse(address[18..]);
        print($"{x}; {y}; {z}");

        transform.position = new Vector3(x, y, z);

        SavePos();
    }
    IEnumerator AndSavePos()
    {

        //       print("!*!");

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


       
        
        m_Positions = AllDollCharacters.Instance.GetDollPositions(dollID);

        transform.forward = Vector3.forward;

        transform.position = Level.Instance.SpawnPoint.position + Vector3.right * index;


        transform.SetPositionAndRotation(Level.Instance.SpawnPoint.position
            + Vector3.right, m_Rotation);

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



        AllDollCharacters.Instance.SetDollPos(m_DollPositions);
    }

    private bool m_IsSleeping;
    public void TimeActionStats(long timeDifference)
    {
        m_IsSleeping = AllDollSleeps.GetSleepingByID(dollID);

        if (m_IsSleeping)
        {
            WarpDoll(Level.Instance.Beds[dollID]);
            //  GoToBed();
            // Прибавить сон. 1 процент за минуту сна. PreviousTime в минутах от 0001 г.

        }
        SavePos();
    }
}
