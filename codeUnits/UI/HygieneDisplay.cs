using GentianoseRealDolls;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

public class HygieneDisplay : MonoBehaviour
{
    [SerializeField] private Text m_PooText;
    [SerializeField] private Text m_SprayText;
    [SerializeField] private Text m_PeeText;
    [SerializeField] private Text m_BathText;
    [SerializeField] private Text m_BrushTeethText;



    Doll currentDoll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDoll = Party.Instance.ActiveDoll;
    }

    private void OnEnable()
    {
        currentDoll = Party.Instance.ActiveDoll;
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentDoll) return;


        m_PooText.text = $"Кишечник: {Mathf.Round(currentDoll.ToiletStats[0] * 10) / 10}/11.0";
        m_SprayText.text = $"Параанальные железы: {Mathf.Round(currentDoll.AnalGlandHealth * 10) / 10}/11.0";
        m_PeeText.text = $"Моча: {Mathf.Round(currentDoll.ToiletStats[2] * 10) / 10}/11.0";
        m_BathText.text = $"Ванная: {Mathf.Round(currentDoll.ToiletStats[3] * 10) / 10}/34.0";
        m_BrushTeethText.text = $"Чистка зубов: {Mathf.Round(currentDoll.ToiletStats[4] * 10) / 10}/33.0";
    }
}
