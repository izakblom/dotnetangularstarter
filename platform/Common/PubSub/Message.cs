using System;
using System.Collections.Generic;
using System.Text;

namespace Common.PubSub
{
    /// <summary>
    /// Every message we send or retrieve over pub sub is of type Message. This is a base class that can be extended if needed to track more properties via inheritance
    /// </summary>
    public class Message
    {
        public Guid Id { get; }
        public DateTime CreationDate { get; }

        public Message()
        {

            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}
