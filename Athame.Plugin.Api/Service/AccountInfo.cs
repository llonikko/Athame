namespace Athame.Plugin.Api.Service
{
    /// <summary>
    /// Provides basic information about a signed in user account.
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// The user's unique ID or email address.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The user's name. This could be a screen name or first name/last name. May be null.
        /// </summary>
        public string Name { get; set; }

        public virtual string DisplayName { get; }
    }
}
