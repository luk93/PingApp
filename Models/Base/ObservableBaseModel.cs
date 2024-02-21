using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Models.Base
{
    public class ObservableBaseModel : ObservableObject
    {
        [Key]
        public int Id { get; set; }
    }
}
