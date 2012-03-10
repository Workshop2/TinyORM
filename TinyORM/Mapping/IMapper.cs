using System.Data;

namespace TinyORM.Mapping
{
    public interface IMapper
    {
        TT Map<TT>(object dbValue);
    }
}