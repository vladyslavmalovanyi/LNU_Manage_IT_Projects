﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL
{
    public  class BaseEntity
    {

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public int Id { get; set; }
            public DateTime Created { get; set; }
            public DateTime Modified { get; set; }

            public byte[] RowVersion { get; set; }
        }
  
}
