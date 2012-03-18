using System;
using TinyORM.Connection;

namespace TinyORM.ConnectionSetup
{
    public interface IConnectionSetup
    {
        ITinyConnection GetConnection();
        bool HasEnoughInformation();
        DbResultInfo TestConnection();
        TimeSpan ConnectionTimemout { get; set; }
    }
}