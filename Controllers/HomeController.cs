using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mcode_generator.Models;
using mcode_generator.ViewModels;
using System.Runtime.InteropServices;
namespace mcode_generator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();
            viewModel.Generator = new GeneratorModel();
            
            return View(viewModel);
        }
        public IActionResult Docs()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("Generator")]IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            MCodeGeneratorOptions options = new MCodeGeneratorOptions();
            options._IsX64 = (model.Generator.SelectedPlatform.Equals("IsX64") ? true : false);
            options._IsX86 = !options._IsX64;
            MCodeGenerator mcodeGenerator =new MCodeGenerator(model.Generator.Code,options);
            mcodeGenerator.Generate();
            model.ConsoleOuput = mcodeGenerator._ConsoleOutput;
            model.Output = mcodeGenerator._Output;
            model.Compiled = true;
            mcodeGenerator.Dispose();
            return View(model);
        }

      
    }
}
