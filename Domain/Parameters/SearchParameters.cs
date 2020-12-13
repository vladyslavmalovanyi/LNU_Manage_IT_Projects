using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class SearchParameters
    {
        public string Name { get; set; }
        public DateTime DateTimeStart
        {
            get;

            set;

        } = DateTime.MinValue;
        public DateTime DateTimeFinish { get; set; } = DateTime.MaxValue;
      
    }
}
