using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Entities
{
    public class UserCustomer : IdentityUser
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public required string Fullname { get; set; }
    }
}
