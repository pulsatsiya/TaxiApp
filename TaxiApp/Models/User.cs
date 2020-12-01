using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiApp.Models
{
    public class User : IEnumerable
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        
        public List<RequestClient> RequestClients { get; set; }


        public IEnumerator GetEnumerator()
        {
            return Login.GetEnumerator();
        }

    }
}
