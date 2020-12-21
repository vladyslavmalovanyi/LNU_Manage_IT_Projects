using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModel
{
    public class EventViewModel : BaseDomain
    {
        private DateTime dateStart;
        private DateTime dateFinish;

        public string Name { get; set; }
        public DateTime DateStart {
            get {
                return new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, dateStart.Hour, dateStart.Minute, dateStart.Second);
            }
            set { dateStart = value;
            } 
        }

        public DateTime DateFinish {
            get
            {
                return new DateTime(dateFinish.Year, dateFinish.Month, dateFinish.Day, dateFinish.Hour, dateFinish.Minute, dateFinish.Second);
            }
            set
            {
                dateFinish = value;
            }
        }

        public string Location { get; set; }
        public string Decription { get; set; }

        public string Organizer { get; set; }

        public string MainImage { get; set; }

        public int CreatorId { get; set; }


        public virtual UserViewModel Creator { get; set; }
    }
}
