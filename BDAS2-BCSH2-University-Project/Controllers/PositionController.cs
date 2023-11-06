using BDAS2_BCSH2_University_Project.Interfaces;
using BDAS2_BCSH2_University_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDAS2_BCSH2_University_Project.Controllers
{
   
        public class PositionController : Controller, IMainController<Position>
        {
            private readonly IMainRepository<Position> _positionRepository;

            public PositionController(IMainRepository<Position> positionRepository)
            {
                _positionRepository = positionRepository;
            }

            [HttpGet]
            public IActionResult Index()
            {
                List<Position> positions = _positionRepository.GetAll();
                return View(positions);
            }

            [HttpGet]
            public IActionResult Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

            Position position = _positionRepository.GetById(id.GetValueOrDefault());

                if (position == null)
                {
                    return NotFound();
                }

                return View(position);
            }

            [HttpGet]
            public IActionResult Edit(int? id)
            {
                return Details(id);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(int id, Position model)
            {
                if (id != model.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _positionRepository.Edit(model);
                }
                return View(model);
            }


            [HttpGet]
            public IActionResult Create()
            {
                throw new NotImplementedException();
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Position model)
            {
                throw new NotImplementedException();
            }


            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Delete(int id)
            {
                throw new NotImplementedException();
            }

        }
    }

