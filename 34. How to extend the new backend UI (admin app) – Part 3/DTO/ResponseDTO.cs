using System.Runtime.Serialization;

namespace SitefinityWebApp.Custom.DTO
{
    [DataContract]
    public class ResponseDTO
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Data { get; set; }

    }
}
