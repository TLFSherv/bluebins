public class ExternalLoginProviderException(string provider, string message) :
Exception($"External login provider: {provider} experienced error: {message}");