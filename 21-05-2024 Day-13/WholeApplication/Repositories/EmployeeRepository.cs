using System.Collections.Generic;
using WholeApplication.Models;

namespace WholeApplication.Repositories
{
    public class EmployeeRepository : Repository<int, Employee>
    {
        private int _counter = 100; // starting value for employee IDs

        protected override int GenerateID()
        {
            return ++_counter;
        }

        public override ICollection<Employee> GetAll()
        {
            return _items;
        }

        public override Employee GetById(int id)
        {
            foreach (var item in _items)
            {
                var property = item.GetType().GetProperty("Id");
                if (property != null && (int)property.GetValue(item)! == id)
                {
                    return item;
                }
            }
            throw new KeyNotFoundException("Employee not found with ID: " + id);
        }
    }
}
