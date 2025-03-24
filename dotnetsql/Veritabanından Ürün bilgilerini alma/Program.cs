//

using System;
using Microsoft.Data.SqlClient;

namespace ConsoleApp
{
    

    }
    class Program
    {
      public static void Main(string[] args)
      { 
        getAllProducts();
       


      }
      static void getMySqlConnection(){
        
      }

    static void getAllProducts(){
      using(var connection = getdbConnection()){
        try{
          connection.Open(); //bağlantıyı aç
          string sql = "select * from Products"; //bütün ürünleri getir
          SqlCommand cmnd = new SqlCommand(sql,connection); //sql komutu üret
          SqlDataReader reader = cmnd.ExecuteReader(); //komutu yürütme
          while(reader.Read())
          {
            Console.WriteLine($"Ürün Adı: {reader[1]}, Fiyat: {reader[5]}");
          } 
          reader.Close(); //döngü bitince bağlantıyı kapa
        }
        catch (Exception e){
          Console.WriteLine(e.Message);

        }
        finally
        {
          connection.Close();
        }
      }
      
    }
      static SqlConnection getdbConnection(){
        //veritabanı bağlantısı
      string connectionString = @"Data Source = .\SQLEXPRESS; Initial Catalog=Northwind; Integrated Security=SSPI;TrustServerCertificate=true";
        return new SqlConnection(connectionString);
      
      

    }
  }

