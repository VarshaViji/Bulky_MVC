using BulkyWeb.Models;

namespace BulkyWeb.Repository.IRepository
{
    public interface IApplicationUserRepository:IRepository<ApplicationUser>
    {
        public void Update(ApplicationUser applicationUser);
    }
}
