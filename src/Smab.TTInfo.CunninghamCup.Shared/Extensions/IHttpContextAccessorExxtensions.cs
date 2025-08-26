using Microsoft.AspNetCore.Http;

namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;
public static class IHttpContextAccessorExxtensions
{
	extension (IHttpContextAccessor httpContextAccessor)
	{
		public string? GetUserId()
		{
			return httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
		}

		public bool IsEditor()
		{
			return httpContextAccessor.HttpContext?.Request.Host.ToString().StartsWith("smab") ?? false;
		}
	}
}
