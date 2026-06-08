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
                m_Doll.CareToiletStat(ToiletStat.Bath, 10);

                m_Inventory.AddKuklons(37);
                InventoryController.Instance.InitAllItems();
            }
        }

        public void BrushTeeth()
        {
            float bt = m_Doll.TakeToiletStat(4);
            
            if (bt < Doll.MaxBrushTeeth)
            {
                m_AnimatorGuard.SetAnimation(19);

                FindAnyObjectByType<CameraAroundDoll>().Turn(-1);


                int count = 0;
                IEnumerator BrushTeethTime()
                {
                    yield return new WaitForSeconds(2);
                    m_Doll.CareToiletStat(ToiletStat.BrushTeeth, 10);
                    count++;

                    if (count < 3 || m_Doll.TakeToiletStat(4) < Doll.MaxBrushTeeth)
                    {
                        StartCoroutine(BrushTeethTime());
                    }
                    else
                    {
                        m_AnimatorGuard.SetAnimation(0);
                        FindAnyObjectByType<CameraAroundDoll>().Turn(1);
                    }
                }

                StartCoroutine(BrushTeethTime());


                m_Inventory.AddKuklons(108);
                InventoryController.Instance.InitAllItems();
            }
        }
    }
}

