using DesktopApp_WPF.HttpClient;
using DesktopApp_WPF.Models;
using DesktopApp_WPF.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DesktopApp_WPF.ViewModels
{
    public partial class StatNpmPackageViewModel : ViewModelBase
    {

        private PackageModel _package;
        private IUserRepository userRepository;
        public PackageModel Package
        {
            get
            {
                return _package;
            }

            set
            {

                _package = value;
                OnPropertyChanged(nameof(Package));
            }
        }

        private List<PackageModel> _listPackage;

        public List<PackageModel> ListPackage
        {
            get
            {
                return _listPackage;
            }

            set
            {
                _listPackage = value;
                OnPropertyChanged(nameof(ListPackage));
            }
        }

        public StatNpmPackageViewModel()
        {

            userRepository = new UserRepository();

            ListPackage = new List<PackageModel>();


            getpackage();

        }

        private void getpackage()
        {
            string baseUrlService = "https://api.npmjs.org/downloads/range/last-month/";

            List<string> package = new List<string>();
            package.Add("base-controller-cesiweb");
            foreach (var item in package)
            {
                RestClient rClientAuth = new RestClient(baseUrlService);

                string jsonJwt = rClientAuth.getPackageStat(baseUrlService, item);

                dynamic jsonObject = JsonConvert.DeserializeObject(jsonJwt);

                foreach (var dl in jsonObject["downloads"])
                {

                    int downloads = dl["downloads"];
                    DateTime date = dl["day"];

                    ListPackage.Add(new PackageModel()
                    {
                        Date = date,
                        Values = downloads

                    });

                }
            }

        }


    }
}
