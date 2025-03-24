using System;
using System.Collections.Generic;
using ConsoleApp;
using Microsoft.Data.SqlClient;

namespace ConsoleApp
{
 public interface IProductDal{
    List<Product> GetAllProducts();
    Product GetProductById(int d);
    void Create(Product p);
    void Update (Product p);
    void Delete(int productId);
  }
    public class MSSQLProductDal : IProductDal
    {
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
            throw new NotImplementedException();
        }

        public void Update(Product p)
        {
            throw new NotImplementedException();
        }
        
    }

    public class ProductManager : IProductDal
    {
      IProductDal _productDal;
      public ProductManager(IProductDal productDal)
      {
        _productDal = productDal;

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
            return _productDal.GetAllProducts();
        }

        public Product GetProductById(int d)
        {
            throw new NotImplementedException();
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
        var products = productDal.GetAllProducts();
        foreach (var item in products)
        {
          Console.WriteLine($"{item.ProductName}");
          
        }
      }
      static void getMySqlConnection(){
        
      }
      
  }

