using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.SmtpSender;

public class MailModel
{
    public string SourceName { get; set; }
    public string SourceEmail { get; set; }
    public string DestinationEmail { get; set; }
    public string Subject { get; set; }
    public string Text { get; set; }
}
