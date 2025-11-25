using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["ShopDBConnection"].ConnectionString;
    }

    // Общие методы для работы с базой данных
    private SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    // Методы для работы с продуктами
    public List<ProductViewModel> GetProducts()
    {
        var products = new List<ProductViewModel>();
        
        using (var connection = GetConnection())
        {
            connection.Open();
            string query = @"
                SELECT p.*, c.Name as CategoryName 
                FROM Products p 
                LEFT JOIN Categories c ON p.CategoryId = c.CategoryId";

            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(new ProductViewModel
                    {
                        ProductId = (int)reader["ProductId"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = (decimal)reader["Price"],
                        StockQuantity = (int)reader["StockQuantity"],
                        CategoryName = reader["CategoryName"].ToString(),
                        CreatedDate = (DateTime)reader["CreatedDate"]
                    });
                }
            }
        }
        
        return products;
    }

    public void AddProduct(Product product)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            string query = @"
                INSERT INTO Products (Name, Description, Price, StockQuantity, CategoryId) 
                VALUES (@Name, @Description, @Price, @StockQuantity, @CategoryId)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                
                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateProduct(Product product)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            string query = @"
                UPDATE Products 
                SET Name = @Name, Description = @Description, Price = @Price, 
                    StockQuantity = @StockQuantity, CategoryId = @CategoryId 
                WHERE ProductId = @ProductId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductId", product.ProductId);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteProduct(int productId)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            string query = "DELETE FROM Products WHERE ProductId = @ProductId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductId", productId);
                command.ExecuteNonQuery();
            }
        }
    }

    // Методы для работы с категориями
    public List<Category> GetCategories()
    {
        var categories = new List<Category>();
        
        using (var connection = GetConnection())
        {
            connection.Open();
            string query = "SELECT * FROM Categories";

            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryId = (int)reader["CategoryId"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString()
                    });
                }
            }
        }
        
        return categories;
    }

    // Методы для работы с клиентами
    public List<Customer> GetCustomers()
    {
        var customers = new List<Customer>();
        
        using (var connection = GetConnection())
        {
            connection.Open();
            string query = "SELECT * FROM Customers";

            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        CustomerId = (int)reader["CustomerId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString()
                    });
                }
            }
        }
        
        return customers;
    }
}