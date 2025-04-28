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

public class Username
{
    public string? username { get; set; }
}

public class Login
{
    public string? username { get; set; }
    public string? senha { get; set; }
}