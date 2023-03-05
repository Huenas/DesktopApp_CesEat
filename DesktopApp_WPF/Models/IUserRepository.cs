using System.Collections.Generic;
using System.Net;

namespace DesktopApp_WPF.Models
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(UserModel userModel, NetworkCredential password);

        void Edit(UserModel userModel, NetworkCredential credential);
        void Remove(int id);
        UserModel GetById(int id);
        UserModel GetByUsername(string username);
        void UpdateRestaurant(RestaurantModel restaurant);
        IEnumerable<UserModel> GetByAll();

    }
}
