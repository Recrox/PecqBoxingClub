using RamDam.BackEnd.Core.Models.Table;
using Microsoft.AspNetCore.Http;
using System;
using RamDam.BackEnd.Core.Enums;
using System.Linq;

namespace RamDam.BackEnd.Core
{
    public class CurrentContext
    {
        private readonly DateTime _dateTime = DateTime.UtcNow;
        private Guid? _userId;

        public virtual HttpContext HttpContext { get; set; }
        public virtual Guid? UserId
        {
            get => User?.Id ?? _userId;
            set => _userId = value;
        }
        public virtual User User { get; set; }
        public virtual string DeviceIdentifier { get; set; }
        public virtual DateTime DateTime => _dateTime;
       

      
       
    }
}
