using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Name { get; set; } = string.Empty;


        [Required]
        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        public string Description { get; set; } = string.Empty;

        public List<CategoryViewModel> Categories { get; set; } = new();
    }
}
