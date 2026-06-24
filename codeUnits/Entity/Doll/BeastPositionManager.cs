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
    [SerializeField] private AllDollSleeps m_AllDollSleeps;
    [SerializeField] private ActiveDollPosition m_ActiveDollPosition;


    [SerializeField] private int m_Location;
    public int Location => m_Location;

    private Doll m_Doll;

    [SerializeField] private int m_MapsNumber = 3;

    [SerializeField] private int[] m_LocPos;
    private void Awake()
    {
        m_LocPos = new int[4];

        m_LocPos[1] = m_Location;
    }

    public void ResetLocation()
    {
        m_Location = 0;
    }
    public void SetBase(ActiveDollPosition adp, AllDollSleeps ads)
    {
        m_ActiveDollPosition = adp;
        m_AllDollSleeps = ads;
    }

    public void Fill(int[] pos)
    {
        
        m_Location = pos[0];

        transform.position = new Vector3(pos[1], pos[2], pos[3]);

        m_LocPos = pos;
        print($"Abrunho! {m_LocPos[1]} {m_LocPos[2]}");

    }
    public void PutIntoBed()
    {
        if (m_Doll != null)
        {
            transform.position = m_Doll.Asset.m_BedPos;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            m_ActiveDollPosition.SetDoll(m_LocPos);
    }

    private void OnDestroy()
    {
        m_ActiveDollPosition?.SetDoll(m_LocPos);
    }

    public void Save()
    {
        m_LocPos[1] = (int)transform.position.x;
        m_LocPos[2] = (int)Mathf.Ceil(transform.position.y);
        m_LocPos[3] = (int)transform.position.z;
        m_ActiveDollPosition.SetDoll(m_LocPos);
    }
}
