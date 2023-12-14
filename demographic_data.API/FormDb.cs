using Microsoft.EntityFrameworkCore;

class FormDb : DbContext
{
    public FormDb(DbContextOptions<FormDb> options)
        : base(options) { }

    public DbSet<Form> Forms => Set<Form>();
}