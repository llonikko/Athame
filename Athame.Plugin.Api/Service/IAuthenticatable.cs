using System.Threading.Tasks;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// The base interface for services that provide authentication. Do not directly implement this interface;
    /// implement either <see cref="IUsernamePasswordAuthenticationAsync"/> or <see cref="IAuthenticatableAsync"/>
    /// depending on your needs.
    /// </summary>
    public interface IAuthenticatable
    {
        /// <summary>
        /// The account associated with this service, or null if the user is signed out.
        /// </summary>
        AccountInfo Account { get; }

        /// <summary>
        /// Whether the service has a signed-in user or not.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Whether the service has a saved session that can be restored or not.
        /// </summary>
        bool HasSavedSession { get; }

        /// <summary>
        /// Restores the session based on the session details stored in settings.
        /// </summary>
        /// <returns>True if the restore succeeded, false otherwise.</returns>
        Task<bool> RestoreAsync();

        /// <summary>
        /// Clears any stored session details.
        /// </summary>
        void Reset();
    }
}
