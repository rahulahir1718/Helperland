using System;
using System.Collections.Generic;

namespace HelperlandProject.Models.Data
{
    public partial class State
    {
        public State()
        {
            Cities = new HashSet<City>();
        }

        public int Id { get; set; }
        public string StateName { get; set; } = null!;

        public virtual ICollection<City> Cities { get; set; }
    }
}
