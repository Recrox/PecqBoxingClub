using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.RamDamApi
{
    


        public class RamDamGuest
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public DateTime Date_gmt { get; set; }
            public Guid Guid { get; set; }
            public DateTime Modified { get; set; }
            public DateTime Modified_gmt { get; set; }
            public string Slug { get; set; }
            public string Status { get; set; }
            public string Type { get; set; }
            public string Link { get; set; }
            public Title Title { get; set; }
            public int Featured_media { get; set; }
            public string Template { get; set; }
            public object[] Acf { get; set; }
            public _Links _links { get; set; }
        }

       


    }

