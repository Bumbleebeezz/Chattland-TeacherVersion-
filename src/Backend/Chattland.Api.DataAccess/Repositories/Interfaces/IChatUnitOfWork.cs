namespace Chattland.Api.DataAccess.Repositories.Interfaces;

public interface IChatUnitOfWork
{
    IDisposable Session { get; }
    void AddOperation(Action operation);
    void ClearOperations();
    Task CommitAsync();
}