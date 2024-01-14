using Microsoft.EntityFrameworkCore;

// // In memmory db context
// class FormDb : DbContext
// {
//     public FormDb(DbContextOptions<FormDb> options)
//         : base(options) { }

//     public DbSet<Form> Forms => Set<Form>();
// }

public class Context : DbContext
{
    // public DbSet<Form> Forms { get; set; }
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<FormItem> Forms { get; set; } = null!;
}
