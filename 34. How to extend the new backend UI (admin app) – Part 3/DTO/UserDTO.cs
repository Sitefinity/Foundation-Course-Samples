using System.Runtime.Serialization;
using Telerik.Sitefinity.Security.Model;

namespace SitefinityWebApp.Custom.DTO
{
    [DataContract]
    public class UserDTO
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        public UserDTO() { }

        public UserDTO( SitefinityProfile profile )
        {
            Id = profile.User.Id.ToString();
            Name = $"{profile.FirstName} {profile.LastName}";
            Email = profile.User.Email;
        }
    }
}
