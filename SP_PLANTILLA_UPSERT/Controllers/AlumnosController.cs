using Microsoft.AspNetCore.Mvc;
using SP_PLANTILLA_UPSERT.Models;
using SP_PLANTILLA_UPSERT.Repositories;

namespace SP_PLANTILLA_UPSERT.Controllers
{
    public class AlumnosController : Controller
    {
        private RepositoryDeportes repo;

        public AlumnosController()
        {
            this.repo = new RepositoryDeportes();
        }

        public IActionResult Index()
        {
            List<AlumnoDetalle> alumnos = this.repo.GetAlumnos();
            return View(alumnos);
        }

        public IActionResult Details(int id)
        {
            AlumnoDetalle alumno = this.repo.FindAlumno(id);
            return View(alumno);
        }

        
    }
}