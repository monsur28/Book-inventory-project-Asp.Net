using DOT_NET_inventory_project.Models;
using Microsoft.EntityFrameworkCore;
namespace DOT_NET_MVC_INVENTORY.Models
{
    public class AppDBContext
    {
        public string ConnectionString { get; set; }
        public AppDBContext()
        {
            ConnectionString = "Server=MONSUR-PC\\SQLEXPRESS;Database=BookInventory;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true;";
        }

    }
}
