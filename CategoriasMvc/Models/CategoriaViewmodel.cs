using System.ComponentModel.DataAnnotations;

namespace CategoriasMvc.Models;

public class CategoriaViewmodel
{
    public int CategoriaIa { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório")]
    public string? Nome { get; set; }

    [Required]
    [Display(Name = "Imagem")]
    public string? ImagemUrl { get; set; }
}
