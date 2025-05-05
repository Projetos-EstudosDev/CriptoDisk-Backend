using System.ComponentModel.DataAnnotations;
namespace BackCriptoDisk2.Models;

public class Mensagem
{
    [Key]
    public int Id { get; set; }
    public Usuarios? remetente { get; set; }
    public string? Conteudo { get; set; }
    public string? destinatario { get; set; }
    public DateTime Data { get; set; } = DateTime.UtcNow;
}