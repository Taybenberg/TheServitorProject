using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClanActivitiesService.Containers
{
    public record RegisterUserContainer
    {
        public bool IsSuccessful { get; internal set; }

        public string UserName { get; internal set; }

        public string Platform { get; internal set; }
    }
}
