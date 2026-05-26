using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentianoseRealDolls
{
    public interface IParty
    {
        public void InitDolls(int mapID, AllDollCharacters adc, 
            AllDollPositions adp, AllDollSleeps ads, long time);
    }
}
