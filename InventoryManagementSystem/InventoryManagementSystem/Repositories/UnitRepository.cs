using InventoryManagementSystem.Data;
using InventoryManagementSystem.Interfaces;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementSystem.Repositories
{
    public class UnitRepository : IUnit
    {
        private readonly InventoryContext _context; 
        public UnitRepository(InventoryContext context) 
        {
            _context = context;
        }
        public List<Unit> GetItems()
        {
            var units = _context.Units.ToList();
            return units;
        }
        public Unit Create(Unit unit)
        {
            _context.Units.Add(unit);
            _context.SaveChanges();
            return unit;
        }

        public Unit Delete(Unit unit)
        {

            _context.Units.Attach(unit);
            _context.Entry(unit).State = EntityState.Deleted;
            _context.SaveChanges();
            return unit;
        }

        public Unit Edit(Unit unit)
        {
            _context.Units.Attach(unit);
            _context.Entry(unit).State = EntityState.Modified;
            _context.SaveChanges();
            return unit;
        }

        public Unit GetUnit(int id)
        {
            var unit = _context.Units.Where(c => c.Id == id).FirstOrDefault();
            return unit;
        }
      
    }
}
