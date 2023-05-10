﻿using InventoryManagementSystem.Data;
using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InventoryManagementSystem.Controllers
{
    public class UnitController : Controller
    {
        private readonly IUnit _unitRepo;

        public UnitController(IUnit unitRepo)
        {
            _unitRepo = unitRepo;
        }

        public IActionResult Index()
        {
            var units = _unitRepo.GetItems();
            return View(units);
        }
        public IActionResult Create()
        {
            var unit= new Unit();
            return View(unit);
        }
        [HttpPost]
        public IActionResult Create(Unit unit)
        {
            _unitRepo.Create(unit);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        [HttpPost]
        public IActionResult Edit(Unit unit)
        {
            _unitRepo.Edit(unit);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
        [HttpPost]
        public IActionResult Delete(Unit unit)
        {
            _unitRepo.Delete(unit);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
        {
            var unit = _unitRepo.GetUnit(id);
            return View(unit);
        }
       
    }
}
