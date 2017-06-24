using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine
{
    public static class StringExtensions 
    {
        public static bool IsEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }
    }
}
