using BackCriptoDisk2.Models;
using Microsoft.EntityFrameworkCore;

namespace BackCriptoDisk2.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Usuarios> usuarios { get; set; }
    public DbSet<Mensagem> mensagens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuarios>().ToTable("usuarios");
        modelBuilder.Entity<Usuarios>()
            .HasKey(c => c.Id)
            .HasName("Id");
        
        modelBuilder.Entity<Mensagem>().ToTable("mensagens");
        modelBuilder.Entity<Mensagem>()
            .HasKey(c => c.Id)
            .HasName("Id");
        
    }
}