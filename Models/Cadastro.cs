using System.ComponentModel.DataAnnotations;

namespace BackCriptoDisk2.Models;

public class Cadastro
{
    [Key]
    public int id { get; set; }
    public string? nome { get; set; }
    public string? username { get; set; }
    public string? email { get; set; }
    public string? senha { get; set; }
}