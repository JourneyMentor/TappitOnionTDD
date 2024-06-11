using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.DTOs
{
    public class GetUserDto
    {
        public string Id { get; set; }
        public bool EmailConfirmed { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
    }
}
