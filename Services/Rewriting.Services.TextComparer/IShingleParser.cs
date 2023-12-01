using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.TextComparer
{
    internal interface IShingleParser
    {
        List<Hash> ParseToShingles(IList<string> words, int shingleLength);
    }
}
