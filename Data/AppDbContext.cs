using BackCriptoDisk2.Models;
using Microsoft.EntityFrameworkCore;

namespace BackCriptoDisk2.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Cadastro> usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cadastro>().ToTable("usuarios");
        modelBuilder.Entity<Cadastro>()
            .HasKey(c => c.username)
            .HasName("username");


    }
}