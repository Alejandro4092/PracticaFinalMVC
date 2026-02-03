using Microsoft.AspNetCore.Mvc;
using SP_PLANTILLA_UPSERT.Models;
using SP_PLANTILLA_UPSERT.Repositories;

namespace SP_PLANTILLA_UPSERT.Controllers
{
    public class PlantillaController : Controller
    {
        private RepositoryPlantilla repo;

        public PlantillaController()
        {
            this.repo = new RepositoryPlantilla();
        }

        public IActionResult Index()
        {
            List<Plantilla> plantillas = this.repo.GetPlantilla();
            return View(plantillas);
        }

        public IActionResult Details(int id)
        {
            Plantilla plantilla = this.repo.FindPlantilla(id);
            return View(plantilla);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await this.repo.DeletePlantilla(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Plantilla plantilla)
        {
            await this.repo.CreatePlantillaAsync(
                plantilla.IdHospital,
                plantilla.SalaCod,
                plantilla.empleadoNo,
                plantilla.Apellido,
                plantilla.Funcion,
                plantilla.Turno,
                plantilla.Salario
            );
            return RedirectToAction("Index");
        }
        public IActionResult BuscadorPlantilla()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BuscadorPlantilla(string funcion, int salario)
        {

            List<Plantilla> empleados = this.repo.GetPlantillaFuncionSalario(funcion, salario);
            if (empleados == null)
            {
                ViewData["MENSAJE"] = "No esxisten empleados con esta " + funcion + " y salario mayor a " + salario;
                return View();
            }
            else
            {
                return View(empleados);
            }
        }
        public IActionResult DatosPlantilla()
        {
            List<string> funcion = this.repo.GetFuncion();
            ViewData["FUNCION"] = funcion;
            return View();
        }
        [HttpPost]
        public IActionResult DatosPlantilla(string funcion)
        {
            List<string> funciones = this.repo.GetFuncion();
            ViewData["FUNCION"] = funciones;
            ResumenPlantilla model = this.repo.GetEmpleadosFuncion(funcion);
            return View(model);
        }
    }
}
