using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Moldels
{
    public class Event : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public DateTime DateStart { get; set; }

        public DateTime DateFinish { get; set; }

        public string Location { get; set; }
        public string Dectription { get; set; }

        public string Organizer { get; set; }

        public byte[] MainImage { get; set; }

        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }

    }
}
