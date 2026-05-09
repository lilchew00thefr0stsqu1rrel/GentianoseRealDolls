using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

namespace GentianoseRealDolls
{
    public class GaitDisplay : MonoBehaviour
    {
        const string oneCross = "+";
        const string twoCrosses = "++";
        const string threeCrosses = "+++";

        [SerializeField] private Text[] m_GaitTexts;
        [SerializeField] private List<DollGaitManager> m_Party;

        private string[] signs = new string[3];

        [SerializeField] private GaitInputController m_InputController;

        

        private void Awake()
        {


            signs[0] = oneCross;
            signs[1] = twoCrosses;
            signs[2] = threeCrosses;
        }
        
       
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_InputController.OnGaitChanged += UpdateGaitDisplay;
        }
        private void OnDestroy()
        {
            m_InputController.OnGaitChanged -= UpdateGaitDisplay;
        }
        public void UpdateGaitDisplay(int[] gaitMap)
        {
            if (m_GaitTexts.Length != 0)
            {
                for (int i = 0; i <  gaitMap.Length; i++) 
                {
                    m_GaitTexts[i].text = signs[gaitMap[i]-1];
                }

            }


            //!!!!!!!!!!!!!
            // print("Huh1!");
        }

        public void UpdateGaitDisplay(int indexInParty, int gaitState)
        {
            if (m_GaitTexts.Length != 0)
            {

                indexInParty = Mathf.Clamp(indexInParty, 0, 2);
                gaitState = Mathf.Clamp(gaitState, 1, 3);

                m_GaitTexts[indexInParty].text = signs[gaitState - 1];
            }


            //!!!!!!!!!!!!!
            // print("Huh1!");
        }

        [SerializeField] private int dollInParty;



        [SerializeField] private Doll m_ActiveDoll;
        public void SetActiveDoll(Doll doll)
        {
            m_ActiveDoll = doll;
        }

    }


    
}

