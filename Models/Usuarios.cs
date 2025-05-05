using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;

namespace BackCriptoDisk2.Models;

public class Usuarios
{
    [Key]
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Senha { get; set; }

}