using DOT_NET_inventory_project.Models;
using DOT_NET_MVC_INVENTORY.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DOT_NET_inventory_project.Controllers
{
    public class BookItemController : Controller
    {
        public AppDBContext dbContext { get; set; }

        public BookItemController()
        {
            dbContext = new AppDBContext();
        }

        public IActionResult Index()
        {
            List<BookItem> books = new List<BookItem>();

            using (SqlConnection conn = new SqlConnection(dbContext.ConnectionString))
            {
                string query = "SELECT * FROM BookItems";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BookItem book = new BookItem
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Stock = Convert.ToInt32(reader["Stock"]),
                        Category = reader["Category"].ToString()
                    };
                    books.Add(book);
                }
                conn.Close();
            }

            return View(books);
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            // Removed the category retrieval code

            return View();
        }

        [HttpPost]
        public ActionResult AddBook(BookItem book)
        {
            using (SqlConnection conn = new SqlConnection(dbContext.ConnectionString))
            {
                // Ensure the correct SQL command for inserting a new book
                string query = "INSERT INTO BookItems (Title, Author, Price, Stock, Category) VALUES (@Title, @Author, @Price, @Stock, @Category)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);
                cmd.Parameters.AddWithValue("@Category", book.Category);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateBook(int id)
        {
            BookItem book = new BookItem();
            using (SqlConnection conn = new SqlConnection(dbContext.ConnectionString))
            {
                string query = "SELECT * FROM BookItems WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    book.Id = Convert.ToInt32(reader["Id"]);
                    book.Title = reader["Title"].ToString();
                    book.Author = reader["Author"].ToString();
                    book.Price = Convert.ToDecimal(reader["Price"]);
                    book.Stock = Convert.ToInt32(reader["Stock"]);
                    book.Category = reader["Category"].ToString();
                }
                conn.Close();
            }

            List<string> categories = new List<string> { "Classic", "Dystopian", "Fantasy", "Thriller", "Biography", "History" };
            ViewBag.Categories = categories;

            return View(book);
        }

        [HttpPost]
        public ActionResult UpdateBook(BookItem book)
        {
            using (SqlConnection conn = new SqlConnection(dbContext.ConnectionString))
            {
                string query = @"UPDATE BookItems SET 
                                    Title=@Title,
                                    Author=@Author,
                                    Price=@Price,
                                    Stock=@Stock,
                                    Category=@Category
                                WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);
                cmd.Parameters.AddWithValue("@Category", book.Category);
                cmd.Parameters.AddWithValue("@Id", book.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("Index");
        }

        // Delete BookItem by Id
        [HttpPost]
        public ActionResult DeleteBook(int id)
        {
            using (SqlConnection conn = new SqlConnection(dbContext.ConnectionString))
            {
                string query = "DELETE FROM BookItems WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("Index");
        }
    }
}
