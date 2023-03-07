using Microsoft.EntityFrameworkCore;
using Rewriting.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.Orders.Tests;

internal class DbContextHelper
{
    public AppDbContext Context { get; set; }

	public DbContextHelper()
	{
		var builder = new DbContextOptionsBuilder<AppDbContext>();
		builder.UseInMemoryDatabase("RewritingDb");
		builder.UseLazyLoadingProxies();

		var options = builder.Options;
		Context = new AppDbContext(options);

		Context.Database.EnsureDeleted();
		Context.Database.EnsureCreated();
	}
}
