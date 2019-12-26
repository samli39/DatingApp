using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingADO.Model
{
    public class Values
    {
        public int Id { set; get; }

        [System.ComponentModel.DefaultValue("")]
        public string ValuesName { set; get; }
    }
}
