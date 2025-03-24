//

using System;
using System.Collections.Generic;
using ConsoleApp;
using Microsoft.Data.SqlClient;

namespace ConsoleApp
{
    

    }
    class Program
    {
      public static void Main(string[] args)
      { 
         var products = getAllProducts();
         foreach(var pr in products)
         {
          Console.WriteLine($"Ürün Adı: {pr.ProductName}, fiyat: {pr.Price}");
         }
       


      }
      static void getMySqlConnection(){
        
      }

    static List<Product> getAllProducts(){
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
        catch (Exception e){
          
          Console.WriteLine(e.Message);

        }
        finally
        {
          connection.Close();
        }
      }
      return products;
    }
      static SqlConnection getdbConnection(){
      string connectionString = @"Data Source = .\SQLEXPRESS; Initial Catalog=Northwind; Integrated Security=SSPI;TrustServerCertificate=true";
        return new SqlConnection(connectionString);
      
      

    }
  }

