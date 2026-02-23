using GentianoseRealDolls;
using UnityEngine;

public class ExitHabitat : MonoBehaviour
{
    [SerializeField] private Animator m_DoorAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private int tipID = 2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<Doll>() != null)
        {
            Dashboard.Instance.ShowInteractTip(tipID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.GetComponent<Doll>() != null)
        {
            Dashboard.Instance.HideInteractTip();
        }
    }
}
