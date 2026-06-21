using GentianoseRealDolls;
using SpaceShooter;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class HygieneDisplay : MonoBehaviour
{


    [SerializeField] private Doll m_ActiveDoll;
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

        gameObject.SetActive(false);
    }


    private void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    public void UpdateUI(Doll activeDoll)
    {
        m_ActiveDoll = activeDoll;

        if (activeDoll.ToiletStats != null)
        {
            m_PooText.text = $"Кишечник: {activeDoll.ToiletStats[0]}/10";
            m_SprayText.text = $"Параанальные железы: {activeDoll.GetSprayCarePoints()}/10";
            m_PeeText.text = $"Моча: {activeDoll.ToiletStats[2]}/10";
            m_BathText.text = $"Ванная: {activeDoll.ToiletStats[3]}/40";
            m_BrushTeethText.text = $"Чистка зубов: {activeDoll.ToiletStats[4]}/30";
        }



    }
}
