using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AT_Domain.DTOs.OutDTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string IsAdmin { get; set; }
        public string IsSubscribed { get; set; }
    }
}
