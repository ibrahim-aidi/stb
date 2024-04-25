using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de téléphone est requis.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Le format du numéro de téléphone n'est pas valide.")]
        public string Telephone { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Role { get; set; } = "User";
        public bool Etat { get; set; } = true;
    }
}
