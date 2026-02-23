using SpaceShooter;
using System.Collections;
using UnityEngine;

namespace GentianoseRealDolls
{
    public class DollBath : DollComponent
    {
       
        public void Wash()
        {
            float bath = m_Doll.TakeToiletStat(3);
            if (bath < 34f)
            {
                m_Doll.CareToiletStat(ToiletStat.Bath, 8.5f);

                Inventory.Instance.AddKuklons(37);
                InventoryController.Instance.InitAllItems();
            }
        }

        public void BrushTeeth()
        {
            float bt = m_Doll.TakeToiletStat(4);
            
            if (bt < Doll.MaxBrushTeeth)
            {
                m_Animator.SetInteger("Autom", 17);

                FindFirstObjectByType<FollowCamera>().Turn(-1);


                int count = 0;
                IEnumerator BrushTeethTime()
                {
                    yield return new WaitForSeconds(2);
                    m_Doll.CareToiletStat(ToiletStat.BrushTeeth, 11f);
                    count++;

                    if (count < 3 || m_Doll.TakeToiletStat(4) < Doll.MaxBrushTeeth)
                    {
                        StartCoroutine(BrushTeethTime());
                    }
                    else
                    {
                        m_Animator.SetInteger("Autom", 0);
                        FindFirstObjectByType<FollowCamera>().Turn(1);
                    }
                }

                StartCoroutine(BrushTeethTime());


                Inventory.Instance.AddKuklons(108);
                InventoryController.Instance.InitAllItems();
            }
        }
    }
}

