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

    int Count();

    int Create(Product p);
    int Update (Product p);
    int Delete(int productId);
  }
    public class MSSQLProductDal : IProductDal
    {
        public int ProductID { get; private set; }

        private SqlConnection getdbConnection(){
      string connectionString = @"Data Source = .\SQLEXPRESS; Initial Catalog=Northwind; Integrated Security=SSPI;TrustServerCertificate=true";
        return new SqlConnection(connectionString);
    }
        public int Create(Product p)
        {
            int result = 0;
              using(var connection = getdbConnection()){
            try{
            connection.Open();
            string sql = "insert into Products (ProductName) VALUES ( @productname)"; 
            SqlCommand cmnd = new SqlCommand(sql,connection); 
            cmnd.Parameters.AddWithValue("@productname", p.ProductName);

             result = cmnd.ExecuteNonQuery();
            Console.WriteLine($"{result} adet kayıt eklendi");


            }
            catch (Exception e)
            {
            Console.WriteLine(e.Message);
            }
            finally
            {
            connection.Close();
            }
            return result;
        }}

        public int Delete(int productId)
        {
               int result = 0;
              using(var connection = getdbConnection()){
                try{
                connection.Open();
                string sql = "delete from products where ProductID=@productId"; 
                SqlCommand cmnd = new SqlCommand(sql,connection); 
                cmnd.Parameters.AddWithValue("@productId", productId);



                result = cmnd.ExecuteNonQuery();

                }
            catch (Exception e)
                {
                Console.WriteLine(e.Message);
                }
            finally
                {
                connection.Close();
                }
            return result;
        }
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

        public int Update(Product p)
        {
             int result = 0;
              using(var connection = getdbConnection()){
                try{
                connection.Open();
                string sql = "update Products set ProductName = @productname, Price =@price where ProductID=@productid"; 
                SqlCommand cmnd = new SqlCommand(sql,connection); 
                cmnd.Parameters.AddWithValue("@productname", p.ProductName);
                cmnd.Parameters.AddWithValue("@price", p.Price);
                cmnd.Parameters.AddWithValue("@productid", p.ProductID);



                result = cmnd.ExecuteNonQuery();

                }
            catch (Exception e)
                {
                Console.WriteLine(e.Message);
                }
            finally
                {
                connection.Close();
                }
            return result;
        }}

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

        public int Count()
        {
            int  count = 0;
            using (var connection = getdbConnection())
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT count(*) FROM Products"; 
                    SqlCommand cmnd = new SqlCommand(sql, connection); 
                    object result = cmnd.ExecuteScalar();
                    if(result!=null){
                    count = Convert.ToInt32(result);
                    }
                    
            
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
             return count;
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

        public int Count()
        {
            return _productDal.Count();
        }

        public int Create(Product p)
        {
            return _productDal.Create(p);
        }
    

        public int Delete(int productId)
        {
            return _productDal.Delete(productId);
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

        public int Update(Product p)
        {
            return _productDal.Update(p);
        }
    }
}
    class Program
    {
      public static void Main(string[] args)
      { 
        var productDal = new ProductManager(new MSSQLProductDal());
        int result = productDal.Delete(78);
        
      }
      
  }

