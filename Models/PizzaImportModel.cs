using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Drawing;

namespace VonnPizzaBackEndService.Models
{
    public class PizzaImportModel
    {
        public required string pizza_id { get; set; }
        public required string pizza_type_id { get; set; }
        public required string size { get; set; }
        public required float price { get; set; }
    }
}
