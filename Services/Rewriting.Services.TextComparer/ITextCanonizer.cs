using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.TextComparer
{
    internal interface ITextCanonizer
    {
        List<string> Canonize(string text);
    }
}
