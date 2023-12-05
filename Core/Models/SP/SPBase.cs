using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.SP
{
    public class SPBase
    {
        protected string _SPName;
        public SPBase(string SPName)
        {
            _SPName = SPName;
        }
        public string getSPName()
        {
            return _SPName;
        }

    }
}
