using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Common.DataObjects.Authentication
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
