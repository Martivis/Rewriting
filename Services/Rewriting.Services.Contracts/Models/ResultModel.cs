using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Contracts;

public class ResultModel
{
    public string Text { get; set; }
    public DateTime PublishDate { get; set; }
    public int? Uniqueness { get; set; }
}
