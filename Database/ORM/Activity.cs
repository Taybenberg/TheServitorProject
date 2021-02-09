using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public record Activity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ActivityID { get; set; }

        public DateTime Period { get; set; }

        public BungieNetApi.ActivityType ActivityType { get; set; }

        public ICollection<ActivityUserStats> ActivityUserStats { get; set; }

        public int? SuspicionIndex { get; set; }
    }
}
