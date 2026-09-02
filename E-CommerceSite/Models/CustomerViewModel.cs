using E_CommerceSite.Controllers;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Models
{
    public class CustomerViewModel
    {
        public RegisterViewModel RegisterNewUser { get; set; } = new RegisterViewModel();
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Contraceña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmar la contraceña es obligatorio")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPasswor { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no es válido")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public bool AcceptTerms { get; set; }

        public ICollection<OrderViewModel> Orders { get; set; }
            = new List<OrderViewModel>();

        public List<CustomerViewModel> Customers { get; set; } = new List<CustomerViewModel>();

        public string Mensaje { get; set; } = string.Empty;
    }
}