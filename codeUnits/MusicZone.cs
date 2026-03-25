using GentianoseRealDolls;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [SerializeField] private MusicChange m_MusicChangeManager;
    [SerializeField] private int m_MusicID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<Doll>())
        {
            m_MusicChangeManager.SetMusic(m_MusicID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.GetComponent<Doll>())
        {
            m_MusicChangeManager.SetDefaultMusic(m_MusicID);
        }
    }



}
