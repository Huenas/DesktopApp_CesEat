using DesktopApp_WPF.HttpClient;
using DesktopApp_WPF.Models;
using DotNetEnv;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DesktopApp_WPF.ViewModels
{


    public class ContainerViewModel : ViewModelBase
    {


        private float _cpuUsage;

        private float _memoryUsage;
        private string _containerName;
        private List<ContainerModel> _containerAll;

        public List<ContainerModel> ContainerAll
        {
            get
            {
                return _containerAll;
            }

            set
            {
                _containerAll = value;
                OnPropertyChanged(nameof(_containerAll));
            }
        }

        public float MemoryUsage
        {
            get
            {
                return _memoryUsage;
            }

            set
            {
                _memoryUsage = value;
                OnPropertyChanged(nameof(MemoryUsage));
            }
        }

        public float CpuUsage
        {
            get
            {
                return _cpuUsage;
            }

            set
            {
                _cpuUsage = value;
                OnPropertyChanged(nameof(CpuUsage));
            }
        }

        public string ContainerName
        {
            get
            {
                return _containerName;
            }

            set
            {
                _containerName = value;
                OnPropertyChanged(nameof(ContainerName));
            }
        }

        public ICommand getContainer { get; }
        public ContainerViewModel()
        {

            ContainerAll = new List<ContainerModel>();
            getstat();


        }

        private string GetJwt()
        {
            Env.TraversePath().Load();
            string username = Environment.GetEnvironmentVariable("UsernamePortainer");
            string password = Environment.GetEnvironmentVariable("PasswordPortainer");
            string baseUrlAuth = Environment.GetEnvironmentVariable("PortainerApiAuth");

            RestClient rClientAuth = new RestClient(baseUrlAuth);
            string jsonBody = "{\"Username\": \"" + username + "\",\"Password\": \"" + password + "\"}";
            string jsonJwt = rClientAuth.MakePostRequest(baseUrlAuth, jsonBody);
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonJwt);
            return jsonObject.jwt;
        }

        private List<ContainerModel> GetContainers(string jwt)
        {
            string baseUrl = Environment.GetEnvironmentVariable("UrlGetPortainerContainer");
            RestClient rClient = new RestClient(baseUrl);
            string endpoint = "json";
            string jsonContainer = rClient.GetContainer(endpoint, jwt);
            JArray jsonArray = JArray.Parse(jsonContainer);
            List<ContainerModel> listContainer = new List<ContainerModel>();
            foreach (var container in jsonArray)
            {
                var jsonNames = container["Names"];
                string idContainer = container["Id"].ToString();
                string containerName = JArray.Parse(jsonNames.ToString())[0].ToString().Substring(1);

                listContainer.Add(new ContainerModel()
                {
                    Name = containerName,
                    Id = idContainer
                });
            }
            return listContainer;
        }

        private void getstat()
        {

            string jwt = GetJwt();
            List<ContainerModel> listContainer = GetContainers(jwt);



            foreach (var container in listContainer)
            {
                string baseUrl = Environment.GetEnvironmentVariable("UrlGetPortainerContainer");
                string containerId = container.Id;
                RestClient rClient = new RestClient(baseUrl);
                string endpointstat = "/stats?stream=false";
                string jsonContainerStat = rClient.GetContainer(containerId + endpointstat, jwt);

                // data processing

                dynamic jsonObject1 = JsonConvert.DeserializeObject(jsonContainerStat);

                //get cpu & pre cpu value for cpu delta
                var preCpu_TotalUsage = jsonObject1["precpu_stats"]["cpu_usage"]["total_usage"];
                var cpu_TotalUsage = jsonObject1["cpu_stats"]["cpu_usage"]["total_usage"];

                //system cpu delta
                var cpu_systemCpuDelta = jsonObject1["cpu_stats"]["system_cpu_usage"];
                var preCpu_systemCpuDelta = jsonObject1["precpu_stats"]["system_cpu_usage"];

                //number cpus
                var cpu_OnlineCpu = jsonObject1["cpu_stats"]["online_cpus"];

                //cpu usage value
                float system_cpu_delta = cpu_systemCpuDelta - preCpu_systemCpuDelta;
                float number_cpus = cpu_OnlineCpu;
                float cpu_delta = cpu_TotalUsage - preCpu_TotalUsage;

                float cpu_usage = (cpu_delta / system_cpu_delta) * number_cpus * 100f;

                //memory
                float memory_stat_usage = jsonObject1["memory_stats"]["usage"];
                float memory_stat_cache = jsonObject1["memory_stats"]["stats"]["cache"];
                float availablememory = jsonObject1["memory_stats"]["limit"];
                //memory %
                float usedmemory = memory_stat_usage - memory_stat_cache;
                float memory = (usedmemory / availablememory) * 100;
                float memoryfree = (1 - (usedmemory / availablememory)) * 100;

                float cpu_usage_view = ((cpu_delta / system_cpu_delta) * number_cpus * 100f) + 15;
                float memoryfree_view = ((1 - (usedmemory / availablememory)) * 100) - 16;

                string name = container.Name;
                ContainerAll.Add(new ContainerModel()
                {
                    Name = name,
                    CpuUsage = cpu_usage,
                    MemoryUsage = memory_stat_usage,
                    MemoryUsagePercent = memory,
                    MemoryLimit = availablememory,
                    MemoryFree = memoryfree,
                    CpuUsageView = cpu_usage_view,
                    MemoryFreeView = memoryfree_view,

                }); ;

            }
        }


    }
}

