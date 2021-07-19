using System.Collections.Generic;

namespace Treatment.Monitor.BusinessLogic.ServiceAction
{
    public interface IServiceActionResult
    {
        ServiceActionResultType ResultType { get; }

        IEnumerable<string> ErrorMessages { get; }

        bool IsSuccess { get; }
    }
}