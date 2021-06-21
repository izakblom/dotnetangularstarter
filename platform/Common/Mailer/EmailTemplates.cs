using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Mailer
{
    public class EmailTemplates
    {

        private readonly static EmailTemplates CONTRACT_COMMENT_NOTIFICATION = new EmailTemplates("d-0701e39573274ceb8996368ebc9030a9", "Contract Comment Notification");
        private readonly static EmailTemplates CONTRACT_WORKFLOW_NOTIFICATION = new EmailTemplates("d-6cfe0a7203d74069b00d71f9f312166f", "Contract Workflow Notification");
        private readonly static EmailTemplates FILE_PROCESS_UPDATE = new EmailTemplates("d-bb92f6d6fc3649d199b5854158180cc6", "File Process Update");
        private readonly static EmailTemplates EMMPLOYEE_SIGNUP_INVITE = new EmailTemplates("d-f3b8b4b4b0e4477ab6edbc853f669120", "Signup Invitation");
        public string _id { get; private set; }
        public string _name { get; private set; }

        public EmailTemplates()
        {
        }

        public EmailTemplates(string id, string name)
        {
            _id = id;
            _name = name;
        }

        public static IEnumerable<EmailTemplates> List()
        {
            return new[]
            {
                CONTRACT_COMMENT_NOTIFICATION, CONTRACT_WORKFLOW_NOTIFICATION, FILE_PROCESS_UPDATE, EMMPLOYEE_SIGNUP_INVITE
            };
        }

        public static EmailTemplates FromName(string name)
        {
            var template = List().SingleOrDefault(s => String.Equals(s._name, name, StringComparison.CurrentCultureIgnoreCase));

            if (template == null)
                throw new Exception($"Possible values for Email Template: {String.Join(",", List().Select(s => s._name))}");

            return template;
        }

        public static EmailTemplates FromId(string id)
        {
            var template = List().SingleOrDefault(s => String.Equals(s._id, id, StringComparison.CurrentCultureIgnoreCase));

            if (template == null)
                throw new Exception($"Possible values for Email Template: {String.Join(",", List().Select(s => s._name))}");

            return template;
        }
    }
}