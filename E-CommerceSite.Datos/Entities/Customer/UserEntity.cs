using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Datos.Customer
{
    public class UserEntity : IdentityUser
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public required string Fullname { get; set; }

        public Guid CustomerId { get; set; }
    }
}
