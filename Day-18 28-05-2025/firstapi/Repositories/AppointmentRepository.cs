using FirstApi.Models;
using FirstApi.Interfaces;
using FirstApi.Contexts;
using Microsoft.EntityFrameworkCore;


namespace FirstApi.Repositories
{
    public  class AppointmnetRepository : Repository<string, Appointment>
    {
        protected AppointmnetRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public override async Task<Appointment> Get(string key)
        {
            var appointment = await _clinicContext.Appointmnets.SingleOrDefaultAsync(p => p.AppointmnetNumber == key);

            return appointment??throw new Exception("No Appointment with the given ID");
        }

        public override async Task<IEnumerable<Appointment>> GetAll()
        {
            var appointments = _clinicContext.Appointmnets;
            if (appointments.Count() == 0)
                throw new Exception("No Appointment in the database");
            return (await appointments.ToListAsync());
        }
    }
}