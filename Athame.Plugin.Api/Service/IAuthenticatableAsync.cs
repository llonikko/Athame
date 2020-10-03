using System.Threading.Tasks;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents an authentication method handled entirely by the service (for instance, OAuth and so on).
    /// </summary>
    public interface IAuthenticatableAsync : IAuthenticatable
    {
        /// <summary>
        /// Initiates a custom sign-in process, returning only when the sign-in process is complete.
        /// </summary>
        /// <returns>True if success, false otherwise.</returns>
        Task<bool> AuthenticateAsync();
    }
}
