
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

public class ProductsModel : PageModel
{
    // This list holds the products retrieved from the database.
    public List<Product> Products { get; set; } = new List<Product>();

    public void OnGet()
    {
        // Connect to the Northwind database in the parent folder.
        string connectionString = "Data Source=../northwind.db;Mode=ReadOnly";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Join products with categories to get each product's category name.
            string sql = @"SELECT p.ProductName, c.CategoryName, p.UnitPrice
                           FROM Products p
                           JOIN Categories c ON p.CategoryID = c.CategoryID";

            using (SqliteCommand command = new SqliteCommand(sql, connection))
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                // Read each database row and add it to the products list.
                while (reader.Read())
                {
                    Products.Add(new Product
                    {
                        ProductName = reader.GetString(0),
                        CategoryName = reader.GetString(1),
                        UnitPrice = (decimal)reader.GetDouble(2)
                    });
                }
            }
        }
    }
}

// This class defines the information displayed for each product.
public class Product
{
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}
