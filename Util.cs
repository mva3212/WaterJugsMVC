using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterJugsMVC
{
    public class Util
    {
        // Find the Greatest Common Divisor
        // Performance could be improved by converting to loop.  
        // Current implementation is cleaner and preferred unless performance becomes an issue.
        public static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}