using RamDam.BackEnd.Core.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Exceptions
{
    public class TimeConflictException: Exception
    {
        public IEnumerable<Favorites> ConflictedFavorites;
        public TimeConflictException() { }

        public TimeConflictException(string message) : base(message) { }

        public TimeConflictException(string message, Exception ex) : base(message, ex) { }

        public TimeConflictException(IEnumerable<Favorites> conflictedFavorites)
        {
            this.ConflictedFavorites = conflictedFavorites;
        }
    }
}
