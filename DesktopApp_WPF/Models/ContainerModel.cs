using System.Collections.Generic;

namespace DesktopApp_WPF.Models
{
    public class ContainerModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float MemoryLimit { get; set; }
        public float CpuUsage { get; set; }
        public float MemoryUsagePercent { get; set; }
        public float MemoryUsage { get; set; }
        public float MemoryFree { get; set; }

        public float CpuUsageView { get; set; }
        public float MemoryFreeView { get; set; }


        public List<string> listContainer = new List<string>();

    }
}
