using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiApp.Models
{
    public class RegisterModel
    {
       
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }


        [Required(ErrorMessage = "Не указана роль")]
        public Role Role { get; set; }
        


        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Введен неправильный пароль")]
        public string ConfirmPassword { get; set; }

    }
}
