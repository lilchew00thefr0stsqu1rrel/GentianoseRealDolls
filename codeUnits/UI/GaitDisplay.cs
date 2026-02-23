using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace GentianoseRealDolls
{
    public class GaitDisplay : SingletonBase<GaitDisplay>
    {
        const string oneCross = "+";
        const string twoCrosses = "++";
        const string threeCrosses = "+++";

        [SerializeField] private Text m_GaitText1;
        [SerializeField] private Text m_GaitText2;
        [SerializeField] private Text m_GaitText3;
        [SerializeField] private List<DollGaitManager> m_Party;

        private static Text[] texts = new Text[3];
        private new void Awake()
        {
            base.Awake();
            m_Party = new List<DollGaitManager>();
            texts[0] = m_GaitText1;
            texts[1] = m_GaitText2;
            texts[2] = m_GaitText3;
        }
        public void SetDolls(Doll[] dolls)
        {
            texts[0] = m_GaitText1;
            texts[1] = m_GaitText2;
            texts[2] = m_GaitText3;

            m_Party.Clear();
            for (int i = 0; i < dolls.Length; i++)
            {
                m_Party.Add(dolls[i].DollController.GaitManager);
                
            }
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }



        // Update is called once per frame
        public static Action<int, int> UpdateText()
        {
            return (dollPartyID, gaitState) =>
            {
                //  print($" {dollPartyID} {gaitState} Blip ");

                //   print("Texts0"+texts[0]);
                if (texts[0] && texts[1] && texts[2])
                {
                    switch (dollPartyID)
                    {
                        case 0:
                            switch (gaitState)
                            {
                                case 1:
                                    texts[0].text = oneCross; break;
                                case 2:
                                    texts[0].text = twoCrosses; break;
                                case 3:
                                    texts[0].text = threeCrosses; break;
                            }
                            break;
                        case 1:
                            switch (gaitState)
                            {
                                case 1:
                                    texts[1].text = oneCross; break;
                                case 2:
                                    texts[1].text = twoCrosses; break;
                                case 3:
                                    texts[1].text = threeCrosses; break;
                            }
                            break;
                        case 2:
                            switch (gaitState)
                            {
                                case 1:
                                    texts[2].text = oneCross; break;
                                case 2:
                                    texts[2].text = twoCrosses; break;
                                case 3:
                                    texts[2].text = threeCrosses; break;
                            }
                            break;

                    }
                }
                

            };
        }

        public void TextChange(int dollPartyID, int gaitState)
        {
            

            if (gaitState == 0) m_GaitText1.text = oneCross;

            if (gaitState == 1) m_GaitText2.text = twoCrosses;

            if (gaitState == 2) m_GaitText3.text = threeCrosses;
        }

        [SerializeField] private Doll m_ActiveDoll;
        public void SetActiveDoll(Doll doll)
        {
            m_ActiveDoll = doll;
        }
    }


    
}

