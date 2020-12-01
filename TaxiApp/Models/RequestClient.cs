using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiApp.Models
     {public enum Status
    {   [Display(Name = "Выполняется")]
        Start,
        [Display(Name = "Выполнена")]
        End,
        [Display(Name = "Отмена")]
        Cancel,
        [Display(Name = "Выполняется водителем")]
        Edit
    }
   
    public class RequestClient
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Некорректные данные")]
        public string StartPoint { get; set; }
        [Required(ErrorMessage = "Некорректные данные")]
        public string EndPoint { get; set; }
        [Required(ErrorMessage = "Некорректные данные")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Некорректные данные")]
        public string Phone { get; set; }
        public Status Status { get; set; } = Status.Start;
        public string Comment { get; set; } = "Без комментариев";

        public int? UserId { get; set; }
        public User User { get; set; }


    }
}
