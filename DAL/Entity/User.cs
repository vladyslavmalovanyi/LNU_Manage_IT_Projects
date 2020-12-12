using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Moldels
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        public string Roles { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }//stored encrypted
        [NotMapped]
        public string DecryptedPassword
        {
            get { return Decrypt(Password); }
            set { Password = Encrypt(value); }
        }
        public virtual ICollection<Event> Events { get; set; }
        
        private string Decrypt(string cipherText)
        {
            return EntityHelper.Decrypt(cipherText);
        }
        private string Encrypt(string clearText)
        {
            return EntityHelper.Encrypt(clearText);
        }
    }
}
