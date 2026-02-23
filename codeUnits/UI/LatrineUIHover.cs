using UnityEngine;

public class LatrineUIHover : MonoBehaviour
{

    [SerializeField] GameObject dashboard;

    private void OnMouseEnter()
    {
        dashboard.SetActive(true);   
    }


    private void OnMouseExit()
    {
        dashboard.SetActive(false);
    }

    public void DashboardVisible()
    {
        dashboard.SetActive(!dashboard.activeSelf);
    }
}
