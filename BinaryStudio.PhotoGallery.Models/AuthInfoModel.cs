
namespace BinaryStudio.PhotoGallery.Models
{
    /// <summary>
    /// The class that represents types of user's authentications.
    /// </summary>
    public class AuthInfoModel
    {
        public enum ProviderType
        {
            Local,
            Google,
            Facebook
        }

        public AuthInfoModel()
        {
        }

        /// <summary>
        /// Main constructor with important fields
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="authProvider">[local][google][facebook]</param>
        public AuthInfoModel(int userId, string authProvider)
        {
            UserId = userId;
            AuthProvider = authProvider;
        }

        

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the authentication service.
        /// </summary>
        public string AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets token for work with social webs
        /// </summary>
        public string AuthProviderToken { get; set; }

        public int UserId { get; set; }
    }
}