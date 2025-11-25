using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SneakerShop.Models;
using SneakerShop.ViewModels;

namespace SneakerShop.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["SneakerShopDB"].ConnectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        #region Sneakers CRUD

        public List<SneakerViewModel> GetAllSneakers()
        {
            var sneakers = new List<SneakerViewModel>();
            
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT s.*, b.Name as BrandName, c.Name as CategoryName 
                    FROM Sneakers s 
                    INNER JOIN Brands b ON s.BrandId = b.BrandId 
                    INNER JOIN Categories c ON s.CategoryId = c.CategoryId 
                    WHERE s.IsAvailable = 1
                    ORDER BY s.CreatedDate DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sneakers.Add(new SneakerViewModel
                        {
                            SneakerId = (int)reader["SneakerId"],
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"]?.ToString() ?? string.Empty,
                            Price = (decimal)reader["Price"],
                            BrandName = reader["BrandName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Size = (int)reader["Size"],
                            Color = reader["Color"].ToString(),
                            StockQuantity = (int)reader["StockQuantity"],
                            ImageUrl = reader["ImageUrl"]?.ToString() ?? string.Empty,
                            IsAvailable = (bool)reader["IsAvailable"],
                            CreatedDate = (DateTime)reader["CreatedDate"]
                        });
                    }
                }
            }
            
            return sneakers;
        }

        public Sneaker GetSneakerById(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Sneakers WHERE SneakerId = @SneakerId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SneakerId", id);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Sneaker
                            {
                                SneakerId = (int)reader["SneakerId"],
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"]?.ToString() ?? string.Empty,
                                Price = (decimal)reader["Price"],
                                BrandId = (int)reader["BrandId"],
                                CategoryId = (int)reader["CategoryId"],
                                Size = (int)reader["Size"],
                                Color = reader["Color"].ToString(),
                                StockQuantity = (int)reader["StockQuantity"],
                                ImageUrl = reader["ImageUrl"]?.ToString() ?? string.Empty,
                                IsAvailable = (bool)reader["IsAvailable"],
                                CreatedDate = (DateTime)reader["CreatedDate"]
                            };
                        }
                    }
                }
            }
            
            return null;
        }

        public bool AddSneaker(Sneaker sneaker)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Sneakers (Name, Description, Price, BrandId, CategoryId, Size, Color, StockQuantity, ImageUrl, IsAvailable)
                        VALUES (@Name, @Description, @Price, @BrandId, @CategoryId, @Size, @Color, @StockQuantity, @ImageUrl, @IsAvailable)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", sneaker.Name);
                        command.Parameters.AddWithValue("@Description", sneaker.Description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", sneaker.Price);
                        command.Parameters.AddWithValue("@BrandId", sneaker.BrandId);
                        command.Parameters.AddWithValue("@CategoryId", sneaker.CategoryId);
                        command.Parameters.AddWithValue("@Size", sneaker.Size);
                        command.Parameters.AddWithValue("@Color", sneaker.Color);
                        command.Parameters.AddWithValue("@StockQuantity", sneaker.StockQuantity);
                        command.Parameters.AddWithValue("@ImageUrl", sneaker.ImageUrl ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IsAvailable", sneaker.IsAvailable);
                        
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при добавлении кроссовок: {ex.Message}");
            }
        }

        public bool UpdateSneaker(Sneaker sneaker)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    string query = @"
                        UPDATE Sneakers 
                        SET Name = @Name, Description = @Description, Price = @Price, 
                            BrandId = @BrandId, CategoryId = @CategoryId, Size = @Size, 
                            Color = @Color, StockQuantity = @StockQuantity, 
                            ImageUrl = @ImageUrl, IsAvailable = @IsAvailable
                        WHERE SneakerId = @SneakerId";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SneakerId", sneaker.SneakerId);
                        command.Parameters.AddWithValue("@Name", sneaker.Name);
                        command.Parameters.AddWithValue("@Description", sneaker.Description ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Price", sneaker.Price);
                        command.Parameters.AddWithValue("@BrandId", sneaker.BrandId);
                        command.Parameters.AddWithValue("@CategoryId", sneaker.CategoryId);
                        command.Parameters.AddWithValue("@Size", sneaker.Size);
                        command.Parameters.AddWithValue("@Color", sneaker.Color);
                        command.Parameters.AddWithValue("@StockQuantity", sneaker.StockQuantity);
                        command.Parameters.AddWithValue("@ImageUrl", sneaker.ImageUrl ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IsAvailable", sneaker.IsAvailable);
                        
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении кроссовок: {ex.Message}");
            }
        }

        public bool DeleteSneaker(int sneakerId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE Sneakers SET IsAvailable = 0 WHERE SneakerId = @SneakerId";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SneakerId", sneakerId);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении кроссовок: {ex.Message}");
            }
        }

        #endregion

        #region Brands CRUD

        public List<Brand> GetAllBrands()
        {
            var brands = new List<Brand>();
            
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Brands ORDER BY Name";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        brands.Add(new Brand
                        {
                            BrandId = (int)reader["BrandId"],
                            Name = reader["Name"].ToString(),
                            Country = reader["Country"]?.ToString() ?? string.Empty,
                            Description = reader["Description"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            
            return brands;
        }

        public bool AddBrand(Brand brand)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Brands (Name, Country, Description) VALUES (@Name, @Country, @Description)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", brand.Name);
                    command.Parameters.AddWithValue("@Country", brand.Country ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Description", brand.Description ?? (object)DBNull.Value);
                    
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        #endregion

        #region Categories CRUD

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Categories ORDER BY Name";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryId = (int)reader["CategoryId"],
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            
            return categories;
        }

        public bool AddCategory(Category category)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Categories (Name, Description) VALUES (@Name, @Description)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", category.Name);
                    command.Parameters.AddWithValue("@Description", category.Description ?? (object)DBNull.Value);
                    
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        #endregion

        #region Customers CRUD

        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Customers ORDER BY LastName, FirstName";

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
                            Email = reader["Email"]?.ToString() ?? string.Empty,
                            Phone = reader["Phone"]?.ToString() ?? string.Empty,
                            Address = reader["Address"]?.ToString() ?? string.Empty,
                            RegistrationDate = (DateTime)reader["RegistrationDate"]
                        });
                    }
                }
            }
            
            return customers;
        }

        public bool AddCustomer(Customer customer)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    INSERT INTO Customers (FirstName, LastName, Email, Phone, Address)
                    VALUES (@FirstName, @LastName, @Email, @Phone, @Address)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Email", customer.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                    
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"
                    UPDATE Customers 
                    SET FirstName = @FirstName, LastName = @LastName, Email = @Email, 
                        Phone = @Phone, Address = @Address
                    WHERE CustomerId = @CustomerId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Email", customer.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                    
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Customers WHERE CustomerId = @CustomerId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        #endregion
    }
}