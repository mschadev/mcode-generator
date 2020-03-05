 using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mcode_generator.Models
{
    public class GeneratorModel
    {
        [Required]
        public string Code { get; set; }
        public string SelectedPlatform { get; set; }

        public string SelectedOptimizationLevel { get; set; }

    }
}
