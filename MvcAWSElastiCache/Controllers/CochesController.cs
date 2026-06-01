using Microsoft.AspNetCore.Mvc;
using MvcAWSElastiCache.Models;
using MvcAWSElastiCache.Repositories;
using MvcAWSElastiCache.Services;

namespace MvcAWSElastiCache.Controllers
{
    public class CochesController : Controller
    {
        private RepositoryCoches repo;
        private ServiceAWSCache service;

        public CochesController(RepositoryCoches repo, ServiceAWSCache service)
        {
            this.repo = repo;
            this.service = service;
        }

        public async Task<IActionResult> Favoritos()
        {
            List<Coche> coches = await this.service.GetCochesAsync();
            return View(coches);
        }

        public async Task<IActionResult> SeleccionarFavorito(int id)
        {
            Coche favorito = this.repo.FindCoche(id);
            await this.service.AddFavoritoAsync(favorito);
            return RedirectToAction("Favoritos");
        }

        public async Task<IActionResult> DeleteFavorito(int id)
        {
            await this.service.DeleteCocheAsync(id);
            return RedirectToAction("Favoritos");
        }

        public IActionResult Index()
        {
            List<Coche> cars = this.repo.GetCoches();
            return View(cars);
        }

        public IActionResult Details(int id)
        {
            Coche coche = this.repo.FindCoche(id);
            return View(coche);
        }
    }
}
