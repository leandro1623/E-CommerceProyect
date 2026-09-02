using E_CommerceSite.Datos.Order;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace E_CommerceSite.Datos.Customer
{
    public class Customer
    {
        public Guid Id { get; set; }

        //[Required(ErrorMessage = "El nombre es obligatorio")]
        //[StringLength(100)]
        //public string Name { get; set; } = string.Empty;

        //[Required(ErrorMessage = "El correo es obligatorio")]
        //[EmailAddress(ErrorMessage = "El correo no es válido")]
        //[StringLength(150)]
        //public string Email { get; set; } = string.Empty;

        //[Phone(ErrorMessage = "El teléfono no es válido")]
        //[StringLength(20)]
        //public string? Phone { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        //public bool IsActive { get; set; } = true;

        public ICollection<Order.Order> Orders { get; set; }
            = new List<Order.Order>();

        //[Required]
        //public string UserEntityId { get; set; }

        public UserEntity UserEntity { get; set; } = null!; //propiedad de navegacion
    }
}