using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.TextComparer
{
    internal interface IHashCounter
    {
        Hash Hash(byte[] data);
        Hash Hash(string data);
    }
}
