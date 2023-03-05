using DesktopApp_WPF.HttpClient;
using DesktopApp_WPF.Models;
using DotNetEnv;
using HandyControl.Tools.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace DesktopApp_WPF.ViewModels
{
    public class LivreurViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }


        private readonly DispatcherTimer _timer;

        private LivreurModel _selectedLivreur;

        public LivreurModel SelectedLivreur
        {
            get
            {
                return _selectedLivreur;
            }

            set
            {

                _selectedLivreur = value;
                OnPropertyChanged(nameof(SelectedLivreur));
            }
        }

        private List<LivreurModel> _livreur;

        public List<LivreurModel> Livreur
        {
            get
            {
                return _livreur;
            }

            set
            {
                _livreur = value;
                OnPropertyChanged(nameof(Livreur));
            }
        }
        public ICommand EditLivreurCommand { get; private set; }
        public ICommand DeleteLivreurCommand { get; private set; }

        public LivreurViewModel()
        {
            IsLoading = true;

            EditLivreurCommand = new RelayCommand(item => EditLivreur(item));
            DeleteLivreurCommand = new RelayCommand(item => DeleteLivreur(item));

            Livreur = new List<LivreurModel>();
            GetLivreur();



        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void GetLivreur()
        {
            string jwtresponse = GetJwt();

            string baseUrlService = "http://localhost:3000/user/filter?type=3";
            RestClient rClientAuth = new RestClient(baseUrlService);
            string jsonJwt = rClientAuth.getClient(baseUrlService, jwtresponse);
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonJwt);
            List<LivreurModel> listLivreur = new List<LivreurModel>();
            foreach (var livreur in jsonObject)
            {

                int type = livreur["type"];
                int id = livreur["id"];
                string _id = livreur["_id"];
                string address = livreur["address"];
                string password = livreur["password"];
                string phone = livreur["phone"];
                string name = livreur["name"];
                string email = livreur["email"];
                bool isActive = livreur["active"];

                Livreur.Add(new LivreurModel()
                {
                    Type = type,
                    Id = id,
                    IdString = _id,
                    Address = address,
                    Password = password,
                    Phone = phone,
                    Name = name,
                    Email = email,
                    IsActive = isActive
                });

            }

        }


        private void EditLivreur(object parameter)
        {
            string jwtresponse = GetJwt();
            int id = SelectedLivreur.Id;
            bool active = SelectedLivreur.IsActive;
            string address = SelectedLivreur.Address;
            string email = SelectedLivreur.Email;
            string name = SelectedLivreur.Name;
            string password = SelectedLivreur.Password;
            string phone = SelectedLivreur.Phone;
            string baseUrlService = "http://localhost:3000/user/modify/" + id;
            string jsonBody = "{\"active\": \"" + active + "\",\"address\": \"" + address + "\",\"email\": \"" + email + "\",\"name\": \"" + name
                + "\",\"password\": \"" + password + "\",\"phone\": \"" + phone + "\"}";
            RestClient rClientAuth = new RestClient(baseUrlService);
            rClientAuth.EditClient(baseUrlService, jsonBody, jwtresponse);
        }

        private void DeleteLivreur(object parameter)
        {

            string jwtresponse = GetJwt();
            int id = SelectedLivreur.Id;
            string baseUrlService = "http://localhost:3000/user/delete/" + id;
            RestClient rClientAuth = new RestClient(baseUrlService);
            rClientAuth.DeleteClient(baseUrlService, jwtresponse);


        }
    }
}
