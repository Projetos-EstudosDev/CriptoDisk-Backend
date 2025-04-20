using System.ComponentModel.DataAnnotations;

namespace BackCriptoDisk2.Models;

public class Cadastro
{
    
    public string? nome { get; set; }
    
    [Key]
    public string? username { get; set; }
    public string? email { get; set; }
    public string? senha { get; set; }
}