using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationParkApp.Domain.Models
{
    public class House
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public bool IsActive { get; set; }
        public int Capacity { get; set; }
    }
}
