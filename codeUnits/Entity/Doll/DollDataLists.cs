using UnityEngine;


/// <summary>
/// Атлас кукол
/// </summary>
[CreateAssetMenu]
public class DollDataLists : ScriptableObject
{
    [SerializeField] private int[] m_AnalGlandVolumeArray;
    public int[] AnalGlandVolumeArray => m_AnalGlandVolumeArray; 
    
    [SerializeField] private int[] m_SizeArray;
    public int[] SizeArray => m_SizeArray;
}
