namespace Treatment.Monitor.BusinessLogic.ServiceAction
{
    public interface IServiceActionResult<out T> : IServiceActionResult
    {
        T GetData();

        IServiceActionResult<TOut> GetFailureAs<TOut>();
    }
}