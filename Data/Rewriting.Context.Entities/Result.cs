﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context.Entities;

public class Result : BaseEntity
{
    public string Content { get; set; }
    public ResultStatus Status { get; set; }
    public int? Uniqueness { get; set; }
    public Guid ContractUid { get; set; }
    public virtual Contract Contract { get; set; }
    public DateTime PublishDate { get; set; }
}
