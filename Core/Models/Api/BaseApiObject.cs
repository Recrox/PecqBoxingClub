using RamDam.BackEnd.Core;
using RamDam.BackEnd.Core.Models.Table;
using System;

namespace RamDam.BackEnd.Core.Models.Api
{
    public abstract class BaseApiObject<T, TId> : IApiObject<T, TId>
        where T : class, ITableObject<TId>
        where TId : IEquatable<TId>
    {
        public TId Id { get; set; }

        public virtual void SetId(T obj)
        {
            Id = obj.Id;
        }

        public virtual T ToTableObject(T obj, CurrentContext currentContext)
        {
            return obj;
        }

        //public User CreatedBy { get; set; }
        //public DateTime? CreatedOn { get; set; }
        //public User ModifiedBy { get; set; }
        //public DateTime? ModifiedOn { get; set; }
    }
}
