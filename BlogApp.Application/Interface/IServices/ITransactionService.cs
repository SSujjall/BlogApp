namespace BlogApp.Application.Interface.IServices
{
    public interface ITransactionService
    {
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
    }
}
