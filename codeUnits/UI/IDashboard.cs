using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GentianoseRealDolls
{
    public interface IDashboard
    {
        void Show(GameObject ui);
        void Hide(GameObject ui);

        void ShowInteractTip(int tipId);
        void ShowInteractTip(int tipId, string itemName, GiveResource gr);
    }
    public interface IDollSettable
    {
        public void SetDoll(Doll doll);
        public void SetSleepDoll(int index, bool sleep);
    }
}
