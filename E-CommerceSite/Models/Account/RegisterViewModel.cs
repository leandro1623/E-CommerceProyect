using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Datos.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Fullname { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [StringLength(
        100,
        MinimumLength = 8,
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",
        ErrorMessage = "Debe incluir una minúscula, una mayúscula, un número y un carácter especial")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe aceptar los terminos y condiciones para poder registrarse en nuestro sistema")]
        public bool AcceptTerms { get; set; }
    }
}
