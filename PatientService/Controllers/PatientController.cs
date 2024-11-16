using Microsoft.AspNetCore.Mvc;

namespace PatientService.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
