namespace ChatbotModule.Services
{
	// Rate-limiting placeholder. The default implementation allows everything.
	// Replace the registration with your own to enforce real limits (per user/IP/etc.).
	public interface IChatRateLimiter
	{
		// Return true to allow the request, false to reject it (controller returns 429).
		Task<bool> AllowAsync(string userId, CancellationToken cancellationToken = default);
	}

	public sealed class NoOpChatRateLimiter : IChatRateLimiter
	{
		public Task<bool> AllowAsync(string userId, CancellationToken cancellationToken = default) =>
			Task.FromResult(true);
	}
}