using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{
    public class GroupedPersonalGridItem
    {
        public DateTime Date { get; set; }
        public List<PersonalGridItem> Items { get; set; }
    }
}
