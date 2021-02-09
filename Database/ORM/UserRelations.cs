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
    public record UserRelations
    {
        [Key]
        public int RelationID { get; set; }

        public long User1ID { get; set; }
        public User User1 { get; set; }

        public long? User2ID { get; set; }
        public User User2 { get; set; }

        public int Count { get; set; }
    }
}
