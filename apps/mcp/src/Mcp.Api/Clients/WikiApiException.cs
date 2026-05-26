namespace Mcp.Api.Clients;

public class WikiApiException : Exception
{
	public WikiApiException(string message, Exception? innerException = null)
		: base(message, innerException)
	{
	}
}
