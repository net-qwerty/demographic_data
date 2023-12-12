using Microsoft.EntityFrameworkCore;

namespace RESTfull.Infrastructure;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

}
