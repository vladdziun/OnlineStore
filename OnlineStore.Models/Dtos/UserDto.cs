using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Models.Dtos
{
    public class UserDto
    {
        public string UserName { get; set; }    
        public string PasswordHash { get; set; }
    }
}
