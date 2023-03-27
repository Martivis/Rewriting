using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.RabbitMQ;

public class RabbitMQSettings
{
    public string Uri { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
