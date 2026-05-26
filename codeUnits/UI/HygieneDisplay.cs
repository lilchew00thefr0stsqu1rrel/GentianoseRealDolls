using GentianoseRealDolls;
using SpaceShooter;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class HygieneDisplay : MonoBehaviour
{


    [SerializeField] private Party m_Party;
    ////[Inject]
    //public void Construct(Party obj)
    //{
    //    m_Party = obj;
    //}

    [SerializeField] private Text m_PooText;
    [SerializeField] private Text m_SprayText;
    [SerializeField] private Text m_PeeText;
    [SerializeField] private Text m_BathText;
    [SerializeField] private Text m_BrushTeethText;



    Doll currentDoll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDoll = m_Party.ActiveDoll;

        gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        if (m_Party) 
            currentDoll = m_Party.ActiveDoll;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Party) return;
        if (!currentDoll) return;

       

        m_PooText.text = $"Кишечник: {Mathf.Round(currentDoll.ToiletStats[0] * 10) / 10}/11.0";
        m_SprayText.text = $"Параанальные железы: {Mathf.Round(currentDoll.AnalGlandHealth * 10) / 10}/11.0";
        m_PeeText.text = $"Моча: {Mathf.Round(currentDoll.ToiletStats[2] * 10) / 10}/11.0";
        m_BathText.text = $"Ванная: {Mathf.Round(currentDoll.ToiletStats[3] * 10) / 10}/34.0";
        m_BrushTeethText.text = $"Чистка зубов: {Mathf.Round(currentDoll.ToiletStats[4] * 10) / 10}/33.0";
        
    }
}
