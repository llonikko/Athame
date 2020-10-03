using System.Collections.Generic;

namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Represents an authentication method where the user provides a username and password.
    /// </summary>
    public interface IUsernamePasswordAuthenticationAsync : IAuthenticatableAsync
    {
        /// <summary>
        /// Set credentials to use for authentication.
        /// </summary>
        /// <param name="username">The username to use.</param>
        /// <param name="password">The password to use.</param>
        /// <param name="rememberUser">Whether the user's session is saved in settings.</param>
        /// <returns></returns>
        IUsernamePasswordAuthenticationAsync SetCredentials(string username, string password, bool rememberUser);

        /// <summary>
        /// Helpful text to show to the user while entering their credentials.
        /// </summary>
        string SignInHelpText { get; }

        /// <summary>
        /// A collection of links to show to the user that may assist them with signing in.
        /// </summary>
        IReadOnlyCollection<SignInLink> SignInLinks { get; }
    }
}
