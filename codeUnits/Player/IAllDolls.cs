using System.Collections.Generic;
using UnityEngine;

namespace GentianoseRealDolls
{
    public interface IAllDolls
    {
        [Tooltip("For all dolls")]
        void ReadDolls();
        List<int> GetDolls();

        [Tooltip("For one doll")]
        void WriteDoll(int[] stats);
    }
}
