using ShiftTool.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace ShiftTool.Shared.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "E-mail er påkrævet")]
        [EmailAddress(ErrorMessage = "Ugyldig e-mail-adresse")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Kodeord er påkrævet")]
        [MinLength(6, ErrorMessage = "Kodeordet skal være mindst 6 tegn langt")]
        public string Password { get; set; } = "";

        [Compare("Password", ErrorMessage = "Kodeordene matcher ikke")]
        public string ConfirmPassword { get; set; } = "";

        [Required(ErrorMessage = "Fuldt navn er påkrævet")]
        public string FullName { get; set; } = "";

        [Phone(ErrorMessage = "Ugyldigt telefonnummer")]
        public string PhoneNumber { get; set; } = "";

        // Du kan tilføje yderligere valideringer til disse felter efter behov
        public string Experience { get; set; } = "";
        public string Skills { get; set; } = "";
        public bool IsCoordinator { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
