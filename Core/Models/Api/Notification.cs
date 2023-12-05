using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Models.Api
{

    public class Notification : BaseApiObject<Table.Notification, Guid>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime SendDate { get; set; }
        public int IdJob { get; set; }
        public string Topic { get; set; } = "news";

    }
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, Table.Notification>();
        }
    }

}
