using DOT_NET_inventory_project.Models;

namespace DOT_NET_inventory_project.Controllers
{
    internal class Book2 : BookItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public double Ratings { get; set; }
        public double Price { get; set; }
    }
}