using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context.Entities;

public class BaseEntity
{
    [Key]
    public Guid Uid { get; set; } = Guid.NewGuid();
}
