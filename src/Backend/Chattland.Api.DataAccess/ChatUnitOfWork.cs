using Chattland.Api.DataAccess.Repositories.Interfaces;
using MongoDB.Driver;

namespace Chattland.Api.DataAccess;

public class ChatUnitOfWork : IChatUnitOfWork
{
    private IClientSessionHandle SessionHandle { get; }
    public IDisposable Session => SessionHandle;
    private List<Action> Operations { get; set; }

    public ChatUnitOfWork()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        SessionHandle = client.StartSession();

        Operations = new List<Action>();
    }

    public void AddOperation(Action operation)
    {
        Operations.Add(operation);
    }

    public void ClearOperations()
    {
        Operations.Clear();
    }

    public async Task CommitAsync()
    {
        SessionHandle.StartTransaction();

        Operations.ForEach(o =>
        {
            o.Invoke();
        });

        await SessionHandle.CommitTransactionAsync();

        ClearOperations();
    }
}