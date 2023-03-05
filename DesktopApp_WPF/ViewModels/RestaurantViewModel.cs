using DesktopApp_WPF.HttpClient;
using DesktopApp_WPF.Models;
using DesktopApp_WPF.Repositories;
using DotNetEnv;
using HandyControl.Tools.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DesktopApp_WPF.ViewModels
{
    public class RestaurantViewModel : ViewModelBase
    {
        private IUserRepository userRepository;
        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand(EditRestaurant);
                }
                return _editCommand;
            }
        }



        private RestaurantModel _selectedRestaurant;


        private List<RestaurantModel> _restaurant;

        public RestaurantModel SelectedRestaurant
        {
            get
            {
                return _selectedRestaurant;
            }

            set
            {

                _selectedRestaurant = value;
                OnPropertyChanged(nameof(SelectedRestaurant));
            }
        }

        public List<RestaurantModel> Restaurant
        {
            get
            {
                return _restaurant;
            }

            set
            {
                _restaurant = value;
                OnPropertyChanged(nameof(Restaurant));
            }
        }
        public ICommand EditRestaurantCommand { get; private set; }
        public ICommand DeleteRestaurantCommand { get; private set; }
        public RestaurantViewModel()
        {

            userRepository = new UserRepository();
            EditRestaurantCommand = new RelayCommand(item => EditRestaurant(item));
            DeleteRestaurantCommand = new RelayCommand(item => DeleteRestaurant(item));
            Restaurant = new List<RestaurantModel>();
            GetRestaurant();


        }
        private string GetJwt()
        {
            Env.TraversePath().Load();
            string userEmail = Environment.GetEnvironmentVariable("MongoDbUserEmail");
            string userPassword = Environment.GetEnvironmentVariable("MongoDbUserPassword");
            string UrlUserAuth = Environment.GetEnvironmentVariable("MongoDbUrlUserAuth");
            RestClient rClientAuth = new RestClient(UrlUserAuth);
            string jsonBody = "{\"email\": \"" + userEmail + "\",\"password\": \"" + userPassword + "\"}";
            string jsonJwt = rClientAuth.MakePostRequest(UrlUserAuth, jsonBody);
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonJwt);
            return jsonObject["token"];

        }

        private void GetRestaurant()
        {

            string jwtresponse = GetJwt();

            string baseUrlService = "http://localhost:3000/user/filter?type=2";
            RestClient rClientAuth = new RestClient(baseUrlService);

            string jsonJwt = rClientAuth.getClient(baseUrlService, jwtresponse);
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonJwt);
            List<RestaurantModel> listRestaurant = new List<RestaurantModel>();
            foreach (var restaurant in jsonObject)
            {

                int type = restaurant["type"];
                int id = restaurant["id"];
                string _id = restaurant["_id"];
                string address = restaurant["address"];
                string password = restaurant["password"];
                string phone = restaurant["phone"];
                string name = restaurant["name"];
                string email = restaurant["email"];
                bool isActive = restaurant["active"];

                Restaurant.Add(new RestaurantModel()
                {
                    Type = type,
                    Id = id,
                    Idstring = _id,
                    Address = address,
                    Password = password,
                    Phone = phone,
                    Name = name,
                    Email = email,
                    IsActive = isActive
                });
            }

        }
        void Delete(object item)
        {
        }
        private void EditRestaurant(object parameter)
        {

            string jwtresponse = GetJwt();

            int id = SelectedRestaurant.Id;
            bool active = SelectedRestaurant.IsActive;
            string address = SelectedRestaurant.Address;
            string email = SelectedRestaurant.Email;
            string name = SelectedRestaurant.Name;
            string password = SelectedRestaurant.Password;
            string phone = SelectedRestaurant.Phone;

            string baseUrlService = "http://localhost:3000/user/modify/" + id;
            string jsonBody = "{\"active\": \"" + active + "\",\"address\": \"" + address + "\",\"email\": \"" + email + "\",\"name\": \"" + name
                + "\",\"password\": \"" + password + "\",\"phone\": \"" + phone + "\"}";

            RestClient rClientAuth = new RestClient(baseUrlService);
            rClientAuth.EditClient(baseUrlService, jsonBody, jwtresponse);


        }

        private void DeleteRestaurant(object parameter)
        {
            string jwtresponse = GetJwt();
            int id = SelectedRestaurant.Id;
            string baseUrlService = "http://localhost:3000/user/delete/" + id;
            RestClient rClientAuth = new RestClient(baseUrlService);
            rClientAuth.DeleteClient(baseUrlService, jwtresponse);


        }
    }
}
