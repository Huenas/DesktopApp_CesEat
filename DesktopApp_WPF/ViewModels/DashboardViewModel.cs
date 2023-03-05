using DesktopApp_WPF.HttpClient;
using DesktopApp_WPF.Models;
using DotNetEnv;
using HandyControl.Tools.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Input;


namespace DesktopApp_WPF.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private ClientModel _selectedClient;

        public ClientModel SelectedClient
        {
            get
            {
                return _selectedClient;
            }

            set
            {

                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }


        private List<ClientModel> _client;

        public List<ClientModel> Client
        {
            get
            {
                return _client;
            }

            set
            {
                _client = value;
                OnPropertyChanged(nameof(Client));
            }
        }

        public ICommand EditClientCommand { get; private set; }
        public ICommand DeleteClientCommand { get; private set; }

        public DashboardViewModel()
        {
            EditClientCommand = new RelayCommand(item => EditClient(item));
            DeleteClientCommand = new RelayCommand(item => DeleteClient(item));

            Client = new List<ClientModel>();
            GetClient();



        }
        private string GetJwt()
        {
            Env.TraversePath().Load();
            string userEmail = Environment.GetEnvironmentVariable("MongoDbUserEmail");
            string userPassword = Environment.GetEnvironmentVariable("MongoDbUserPassword");
            string UrlUserAuth = Environment.GetEnvironmentVariable("MongoDbUrlUserAuth");

            RestClient rClientAuth = new RestClient(UrlUserAuth);
            string jsonBody = "{\"email\": \"" + userEmail + "\",\"password\":  \"" + userPassword + "\"}";
            string jsonJwt = rClientAuth.MakePostRequest(UrlUserAuth, jsonBody);
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonJwt);
            return jsonObject["token"];

        }

        private void GetClient()
        {
            string jwtresponse = GetJwt();

            string baseUrlService = "http://localhost:3000/user/filter?type=1";
            RestClient rClientAuth = new RestClient(baseUrlService);

            string jsonJwt = rClientAuth.getClient(baseUrlService, jwtresponse);
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonJwt);
            List<ClientModel> listClient = new List<ClientModel>();
            foreach (var client in jsonObject)
            {

                int type = client["type"];
                int id = client["id"];
                string _id = client["_id"];
                string address = client["address"];
                string password = client["password"];
                string phone = client["phone"];
                string name = client["name"];
                string email = client["email"];
                bool isActive = client["active"];

                Client.Add(new ClientModel()
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

        private void EditClient(object parameter)
        {

            string jwtresponse = GetJwt();

            int id = SelectedClient.Id;
            bool active = SelectedClient.IsActive;
            string address = SelectedClient.Address;
            string email = SelectedClient.Email;
            string name = SelectedClient.Name;
            string password = SelectedClient.Password;
            string phone = SelectedClient.Phone;

            string baseUrlService = "http://localhost:3000/user/modify/" + id;
            string jsonBody = "{\"active\": \"" + active + "\",\"address\": \"" + address + "\",\"email\": \"" + email + "\",\"name\": \"" + name
                + "\",\"password\": \"" + password + "\",\"phone\": \"" + phone + "\"}";

            RestClient rClientAuth = new RestClient(baseUrlService);
            rClientAuth.EditClient(baseUrlService, jsonBody, jwtresponse);


        }

        private void DeleteClient(object parameter)
        {

            string jwtresponse = GetJwt();
            int id = SelectedClient.Id;
            string baseUrlService = "http://localhost:3000/user/delete/" + id;
            RestClient rClientAuth = new RestClient(baseUrlService);
            rClientAuth.DeleteClient(baseUrlService, jwtresponse);


        }

    }
}
