using AT_Domain.Models;
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
        public bool IsAdmin { get; set; }
        public bool IsSubscribed { get; set; }

        public UserDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
            IsAdmin = user.IsAdmin;
            IsSubscribed = user.IsSubscribed;
        }
    }
}
