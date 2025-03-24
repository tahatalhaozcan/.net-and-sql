using System;
using System.Collections.Generic;
using System.Data;
using ConsoleApp;
using Microsoft.Data.SqlClient;

namespace ConsoleApp
{
 public interface IProductDal{
    List<Product> GetAllProducts();
    Product GetProductById(int d);
    List <Product> FindProduct(string name);

    void Create(Product p);
    void Update (Product p);
    void Delete(int productId);
  }
    public class MSSQLProductDal : IProductDal
    {
        public int ProductID { get; private set; }

        private SqlConnection getdbConnection(){
      string connectionString = @"Data Source = .\SQLEXPRESS; Initial Catalog=Northwind; Integrated Security=SSPI;TrustServerCertificate=true";
        return new SqlConnection(connectionString);
    }
        public void Create(Product p)
        {
            throw new NotImplementedException();
        }

        public void Delete(int productId)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = null;

      using(var connection = getdbConnection()){
        try{
          connection.Open();
          string sql = "select * from Products WHERE Price>10"; 
          SqlCommand cmnd = new SqlCommand(sql,connection); 
          SqlDataReader reader = cmnd.ExecuteReader(); 
          products = new List<Product>();
          while(reader.Read())
          {
            products.Add(
              new Product{
                ProductID = int.Parse(reader["ProductID"].ToString()),
                ProductName = reader["ProductName"].ToString(),
                Price = int.Parse(reader["Price"]?.ToString())
                }
              );
          } 
          reader.Close(); 
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }
        finally
        {
          connection.Close();
        }
      }
      return products;
        }
  public Product GetProductById(int d)
{
    Product product = null;
    using (var connection = getdbConnection())
    {
        try
        {
            connection.Open();
            string sql = "SELECT * FROM Products WHERE ProductID = @ProductID"; 
            SqlCommand cmnd = new SqlCommand(sql, connection); 
            cmnd.Parameters.Add("@ProductID", SqlDbType.Int).Value = d;

            SqlDataReader reader = cmnd.ExecuteReader();
            
            if (reader.HasRows) // Eğer veri varsa
            {
                reader.Read();
                product = new Product()
                {
                    ProductID = Convert.ToInt32(reader["ProductID"]),
                    ProductName = reader["ProductName"].ToString(),
                    Price = Convert.ToInt32(reader["Price"]) // `int` yerine `decimal` olabilir
                };
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
    return product;
}

        public void Update(Product p)
        {
            throw new NotImplementedException();
        }

        public List<Product> FindProduct(string name)
        {
            List<Product> products = null;
            using (var connection = getdbConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT * FROM Products WHERE ProductName LIKE @ProductName"; 
                    SqlCommand cmnd = new SqlCommand(sql, connection); 
                    cmnd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = "%" + name + "%";

                    SqlDataReader reader = cmnd.ExecuteReader();

                    products = new List<Product>();
                    while(reader.Read())
                    {
                      products.Add(
                        new Product{
                          ProductID = int.Parse(reader["ProductID"].ToString()),
                          ProductName = reader["ProductName"].ToString(),
                          Price = int.Parse(reader["Price"]?.ToString())
                          }
                        );
                    } 
                    
                    reader.Close();
                }
        catch (Exception e)
        {
            Console.WriteLine($"Hata: {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
    return products;
        }
    }

    public class ProductManager : IProductDal
    {
      IProductDal _productDal;
      public ProductManager(IProductDal productDal)
      {
        _productDal = productDal;

      }

        public int ProductID { get; private set; }

        public void Create(Product p)
        {
            throw new NotImplementedException();
        }

        public void Delete(int productId)
        {
            throw new NotImplementedException();
        }

        public List<Product> FindProduct(string name)
        {
            return _productDal.FindProduct(name);
        }

        public List<Product> GetAllProducts()
        {
            return _productDal.GetAllProducts();
        }

        public Product GetProductById(int d)
        {
            return _productDal.GetProductById(d);

        }

        public void Update(Product p)
        {
            throw new NotImplementedException();
        }
    }
}
    class Program
    {
      public static void Main(string[] args)
      { 
        var productDal = new ProductManager(new MSSQLProductDal());
        var products = productDal.FindProduct("Sir");
        foreach (var product in products)
        {
          Console.WriteLine($"{product.ProductName} - {product.Price}");
          
        }
        
      }
      static void getMySqlConnection(){
        
      }
      
  }

