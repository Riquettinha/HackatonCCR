using HackathonCCR.EDM.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HackathonCCR.EDM.Models
{
    public class User : ModelBase
    {
        public User() : base("User", "UserId")
        {
            Mentorings = new List<Schedule>();
            Schedules = new List<Schedule>();
        }

        public Guid UserId { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O '{0}' � obrigat�rio")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "O campo '{0}' deve ter entre {1} e {2} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "E-mail em formato inv�lido.")]
        [Required(ErrorMessage = "O '{0}' � obrigat�rio")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "A '{0}' � obrigat�ria")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "A senha deve conter ao menos uma letra mai�scula, uma letra min�scula e um n�mero")]
        public string Password { get; set; }

        public HackathonCCR.EDM.Enums.User.Type Type { get; set; }

        public virtual ICollection<Schedule> Mentorings { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
