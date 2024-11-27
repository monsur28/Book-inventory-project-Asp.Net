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

        // Index method to fetch all book items
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
                        Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                        Title = reader["Title"]?.ToString(),
                        Author = reader["Author"]?.ToString(),
                        Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0,
                        Stock = reader["Stock"] != DBNull.Value ? Convert.ToInt32(reader["Stock"]) : 0,
                        Category = reader["Category"]?.ToString()
                    };
                    books.Add(book);
                }
                conn.Close();
            }

            return View(books);
        }

        // Display Add Book form
        [HttpGet]
        public ActionResult AddBook()
        {
            return View();
        }

        // Handle Add Book form submission
        [HttpPost]
        public ActionResult AddBook(BookItem book)
        {
            using (SqlConnection conn = new SqlConnection(dbContext.ConnectionString))
            {
                string query = "INSERT INTO BookItems (Title, Author, Price, Stock, Category) VALUES (@Title, @Author, @Price, @Stock, @Category)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", book.Title ?? string.Empty);
                cmd.Parameters.AddWithValue("@Author", book.Author ?? string.Empty);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);
                cmd.Parameters.AddWithValue("@Category", book.Category ?? string.Empty);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("Index");
        }

        // Display Update Book form
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
                    book.Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0;
                    book.Title = reader["Title"]?.ToString();
                    book.Author = reader["Author"]?.ToString();
                    book.Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0;
                    book.Stock = reader["Stock"] != DBNull.Value ? Convert.ToInt32(reader["Stock"]) : 0;
                    book.Category = reader["Category"]?.ToString();
                }
                conn.Close();
            }

            List<string> categories = new List<string> { "Classic", "Dystopian", "Fantasy", "Thriller", "Biography", "History" };
            ViewBag.Categories = categories;

            return View(book);
        }

        // Handle Update Book form submission
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
                cmd.Parameters.AddWithValue("@Title", book.Title ?? string.Empty);
                cmd.Parameters.AddWithValue("@Author", book.Author ?? string.Empty);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);
                cmd.Parameters.AddWithValue("@Category", book.Category ?? string.Empty);
                cmd.Parameters.AddWithValue("@Id", book.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return RedirectToAction("Index");
        }

        // Handle Delete Book operation
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
