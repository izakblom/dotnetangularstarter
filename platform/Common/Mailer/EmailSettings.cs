using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Mailer
{
    public class EmailSettings
    {
        public string Email_SendGridKey { get; set; }
        public string Email_From { get; set; }
        public string Email_FromName { get; set; }
        public string Template_Id { get; set; }

    }
}
