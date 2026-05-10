using GentianoseRealDolls;
using NTC.MonoCache;
using UnityEngine;

public class ExitHabitat : MonoCache
{
    [SerializeField] private Animator m_DoorAnimator;


    [SerializeField] private Door door;
    bool near;

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
            near = true;
        }
    }

    protected override void Run()
    {
        base.Run();

        if (near)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                door.Activate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.GetComponent<Doll>() != null)
        {
            Dashboard.Instance.HideInteractTip();
            near = false;
        }
    }
}
