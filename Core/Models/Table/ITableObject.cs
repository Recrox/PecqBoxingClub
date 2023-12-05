using System;

namespace RamDam.BackEnd.Core.Models.Table
{
    public interface ITableObject<T> where T : IEquatable<T>
    {
        T Id { get; set; }
        //DateTime CreatedOn { get; set; }
        //DateTime? ModifiedOn { get; set; }
        //Guid? CreatedBy { get; set; }
        //Guid? ModifiedBy { get; set; }
    }
}
