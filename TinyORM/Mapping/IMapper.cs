using System.Data;

namespace TinyORM.Mapping
{
    /// <summary>
    /// General interface for mapping objects from a DB call to a valid return object
    /// </summary>
    public interface IMapper
    {
        TT Map<TT>(object dbValue);
    }
}