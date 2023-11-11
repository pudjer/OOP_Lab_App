using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGregator_Domain.Models
{
    public class Subscription : BaseModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool? Unsubscribed { get; set; } = null;

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
