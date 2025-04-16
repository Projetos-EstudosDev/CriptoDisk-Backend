using BackCriptoDisk2.Models;
using Microsoft.EntityFrameworkCore;

namespace BackCriptoDisk2.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Cadastro> usuarios { get; set; }
  
}