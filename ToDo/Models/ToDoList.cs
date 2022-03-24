using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class ToDoList
    {
        public string Maintext {get; set; }
        public string Description {get; set; }
        public ToDoList(string maintext, string description)
        {
            
            Maintext = maintext;
            Description = description;

        }
    }
}
