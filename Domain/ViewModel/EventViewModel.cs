using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModel
{
    public class EventViewModel : BaseDomain
    {
        public string Name { get; set; }
        public DateTime DateStart { get; set; }

        public DateTime DateFinish { get; set; }

        public string Location { get; set; }
        public string Dectription { get; set; }

        public string Organizer { get; set; }

        public byte[] MainImage { get; set; }

        public int CreatorId { get; set; }


        public virtual UserViewModel Creator { get; set; }
    }
}
