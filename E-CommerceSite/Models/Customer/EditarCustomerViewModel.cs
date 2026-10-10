using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Models
{
    public class EditarCustomerViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;


        [Phone(ErrorMessage = "El teléfono no es válido")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        public bool IsActive { get; set; }

    }
}
