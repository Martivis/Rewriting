using Microsoft.EntityFrameworkCore;
using Rewriting.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders.UnitTests;

internal class DbContextHelper
{
    public AppDbContext Context { get; set; }

	public DbContextHelper()
	{
		var builder = new DbContextOptionsBuilder<AppDbContext>();
		builder.UseInMemoryDatabase("RewritingDb");
		
		var options = builder.Options;
		Context = new AppDbContext(options);

		Context.AddRange(TestDataProvider.Orders);

		Context.SaveChanges();
	}
}
