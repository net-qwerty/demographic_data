using Microsoft.EntityFrameworkCore;

// // In memmory db context
// class FormDb : DbContext
// {
//     public FormDb(DbContextOptions<FormDb> options)
//         : base(options) { }

//     public DbSet<Form> Forms => Set<Form>();
// }

public class FormDb : DbContext
{
    // public DbSet<Form> Forms { get; set; }
    public FormDb(DbContextOptions<FormDb> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Form> Forms { get; set; } = null!;
}
