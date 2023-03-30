using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.SmtpSender;

public class SmtpSettings
{
    public bool Enabled { get; set; } = false;
    public string Uri { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
