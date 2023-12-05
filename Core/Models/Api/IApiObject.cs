using RamDam.BackEnd.Core;

namespace RamDam.BackEnd.Core.Models.Api
{
    public interface IApiObject<T, TId>
    {
        TId Id { get; set; }
        void SetId(T obj);
        T ToTableObject(T obj, CurrentContext currentContext);
    }
}
