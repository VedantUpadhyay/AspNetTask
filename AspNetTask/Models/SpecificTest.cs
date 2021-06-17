using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetTask.Models
{
    public class SpecificTest
    {
        public int SpecificTestId { get; set; }

        public string PlayerName { get; set; }

        public string Result { get; set; }

        public int TestId { get; set; }

        public string FitnessRating { get; set; }

        public TestType TestType { get; set; }

    }
}
