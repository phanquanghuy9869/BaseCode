using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Core.Helper
{
    public class UniqueIdentityHelper
    {
        public static int GetRandomNumber(int min, int max)
        {
            var rd = new Random();
            return rd.Next(min, max);
        }

        public static int GetRandomNumber()
        {
            var min = 0;
            var max = 999999999;
            return GetRandomNumber(min, max);
        }
    }
}
