using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetTask.Models
{
    public class Test
    {
        public int TestId { get; set; }

        public int NumberOfParticipants { get; set; }

        public DateTime DateAM { get; set; }

        public string TestType { get; set; }
    }
}
