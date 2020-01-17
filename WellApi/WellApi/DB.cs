using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text;

namespace WellApi
{
    public class DB
    {
        static SqlConnection sqlConnection = null;

        static void ConnectToDb()
        {
            string connectionString = "Server=tcp:wellhtw.database.windows.net,1433;Initial Catalog=well;Persist Security Info=False;User ID=htw;Password=maph2019!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }
        
        static void DisconnectFromDb()
        {
            if( sqlConnection != null)
            {
                sqlConnection.Close();
                sqlConnection = null;
            }
        }

        static void GetTest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 20 pc.Name as CategoryName, p.name as ProductName ");
            sb.Append("FROM [SalesLT].[ProductCategory] pc ");
            sb.Append("JOIN [SalesLT].[Product] p ");
            sb.Append("ON pc.productcategoryid = p.productcategoryid;");
            String sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                    }
                }
            }
        }
        static void InsertTest()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO well.dbo.Well (Name)");
            sb.Append("VALUES ('Test') ");
            String sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, sqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                    }
                }
            }
            Console.WriteLine("Inserted");
        }
    }
}
