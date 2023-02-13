using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context;

public class AppDbContextFactory
{
    private readonly DbContextOptions<AppDbContext> _options;

	public AppDbContextFactory(DbContextOptions<AppDbContext> options)
	{
		_options = options;
	}

	public AppDbContext Create() => new(_options);
}
