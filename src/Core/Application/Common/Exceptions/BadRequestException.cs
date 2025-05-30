using System.Net;

namespace Microsoft.Teams.Assist.Application.Common.Exceptions;
public class BadRequestException : CustomException
{
    public BadRequestException(string message, List<string>? errors = default)
        : base(message, errors, HttpStatusCode.BadGateway)
    {
    }
}
