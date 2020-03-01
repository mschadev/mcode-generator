using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mcode_generator.Models;
namespace mcode_generator.ViewModels
{
    public class IndexViewModel
    {
        public GeneratorModel Generator { get; set; }
        public string ConsoleOuput { get; set; }
        public string Output { get; set; }
        public bool Compiled { get; set; }
        
    }
}
