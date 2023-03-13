using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public class ClientAuthModel
{
    public Guid Uid { get; set; }
    public Guid ClientUid { get; set; }
}
