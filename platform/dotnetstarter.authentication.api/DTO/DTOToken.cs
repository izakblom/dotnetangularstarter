using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace dotnetstarter.authentication.api.DTO
{
    [DataContract]
    public class DTOToken
    {
        [DataMember]
        public string token { get; set; }

        [DataMember]
        public DateTimeOffset expiresAt { get; set; }
    }
}
