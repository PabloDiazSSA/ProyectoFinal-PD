
using ProyectoFinal.Models.DBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.DBO.DTO
{
    public class UserDto
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8)]
        [MaxLength(20)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])([A-Za-z\d$@$!%*?&]|[^ ]){8,20}$", ErrorMessage = "Password must meet requirements (Al menos ocho cáracteres con: Mayuscula(s), minuscula(s), numero(s), al menos un cáracter especial)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}