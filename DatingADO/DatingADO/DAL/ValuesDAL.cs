using DatingADO.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DatingADO.DAL
{
    public class ValuesDAL
    {
        private readonly string url;

        public ValuesDAL(IConfiguration config)
        {
            url = config["ConnectionStrings:DatingSQL"];
        }

        //get all values
        public async Task<List<Values>> GetAll()
        {
            //list for saving data from database
            List<Values> value = new List<Values>();
            //query
            string query = "select * from [Values]";

            //connection
            using (SqlConnection cnn = new SqlConnection(url))
            {
                //pass query to database
                using (var cmd = new SqlCommand(query, cnn))
                {
                    //open connection
                    await cnn.OpenAsync();
                    //read data
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        //create values object
                        Values ele = new Values()
                        {
                            Id = (int)reader["Id"],
                            ValuesName = reader["ValuesName"].ToString()
                        };

                        //add object to list
                        value.Add(ele);
                    }

                }
            }

            return value;
        }

        //get value by id
        public async Task<Values> GetValueById(int id)
        {
            Values value = null;
            //query
            string query = "Select * from [Values]" +
                "where id = @p1";
            //connection
            using (SqlConnection cnn = new SqlConnection(url))
            {
                //pass query to database
                using (var cmd = new SqlCommand(query, cnn))
                {
                    //open connection
                    await cnn.OpenAsync();

                    //add value to parameter
                    cmd.Parameters.AddWithValue("@p1", id);

                    //execute the query
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        value = new Values()
                        {
                            Id = (int)reader["Id"],
                            ValuesName = reader["ValuesName"].ToString()
                        };
                    }

                }
            }

            return value;
        }

        //add a data
        public async Task<int> AddValue(Values value)
        {
            int id=0;
            //query
            string query = "insert into [Values] " +
                "(ValuesName) " +
                "Output INSERTED.ID " +
                "values(@p1);";

            //connection
            using (SqlConnection cnn = new SqlConnection(url))
            {
                //pass query to database 
                using (var cmd = new SqlCommand(query, cnn))
                {
                    //open connection
                    await cnn.OpenAsync();

                    //add value to paramter
                    cmd.Parameters.AddWithValue("@p1", value.ValuesName);
                    //getting inserted id
                    try
                    {
                        id = (int)cmd.ExecuteScalar();
                    }catch(Exception e)
                    {
                        Console.WriteLine("here");
                    }
                   

                }
            }

            return id;
        }

        //add multi-data
        public async Task AddMultiValues(List<Values> value)
        {
            //create data table
            DataTable dt = new DataTable();
            //add column
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("ValuesName", typeof(string)));
            //Add rows
            foreach (var ele in value)
            {
                dt.Rows.Add(0,ele.ValuesName);
            }

            //connection
            using (SqlConnection cnn = new SqlConnection(url))
            {
                //open connection 
                cnn.Open();
                //bulk
                using (var bulk = new SqlBulkCopy(cnn))
                {
                    bulk.DestinationTableName = "[Values]";
                    await bulk.WriteToServerAsync(dt);
                }

            }

        }

        //update a data
        public async Task UpdateValue(Values value)
        {
            //query
            string query = "update [Values] " +
                "set ValuesName = @p1 " +
                "where id = @p2;";

            //connection
            using (SqlConnection cnn = new SqlConnection(url))
            {
                //pass query to database
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    //open connection
                    await cnn.OpenAsync();

                    //add values to parameters
                    cmd.Parameters.AddWithValue("@p1", value.ValuesName);
                    cmd.Parameters.AddWithValue("@p2", value.Id);

                    //execute query
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        //update multi data
        //public async Task UpdateMultiValues(List<Values> value)
        //{
        //}

        //delete data
        public async Task<bool> DeleteData(int id)
        {
            //query
            string query = "delete from [Values] " +
                "where id = @p1";

            //connection
            using (SqlConnection cnn = new SqlConnection(url))
            {
                //pass query to database
                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    //open connection
                    await cnn.OpenAsync();

                    //add value to parameter
                    cmd.Parameters.AddWithValue("@p1", id);

                    //execute query
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }
    }
}
