using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Test.Models;

namespace Test.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Command")]
    public class CommandController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly MySqlConnection connection = new MySqlConnection("server=localhost;Port=3306;user id=root;Password=usbw;persistsecurityinfo=True;database=testDB;CharSet=utf8;SslMode=none");

        public CommandController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(Commands newCommand)
        {
            string query = @"insert into commands (Description, KeyCommand) values (@Description, @KeyCommand);";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("DBConnect");

            MySqlDataReader reader;

            using (MySqlConnection connect = new MySqlConnection(sqlDataSource))
            {
                connect.Open();
                using (MySqlCommand command = new MySqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@Description", newCommand.Description);
                    command.Parameters.AddWithValue("@KeyCommand", newCommand.KeyCommand);
                    reader = command.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    connect.Close();
                }
            }
            return new JsonResult("Added successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from commands where Id = @Id;";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("DBConnect");

            MySqlDataReader reader;

            using (MySqlConnection connect = new MySqlConnection(sqlDataSource))
            {
                connect.Open();
                using (MySqlCommand command = new MySqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    reader = command.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    connect.Close();
                }
            }
            return new JsonResult("Deleted successfully");
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select Id, Description, KeyCommand from commands";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("DBConnect");

            MySqlDataReader reader;

            using(MySqlConnection connect = new MySqlConnection(sqlDataSource))
            {
                connect.Open();
                using(MySqlCommand command = new MySqlCommand(query, connect))
                {
                    reader = command.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    connect.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"select Id, Description, KeyCommand from commands where Id = @id";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("DBConnect");

            MySqlDataReader reader;

            using (MySqlConnection connect = new MySqlConnection(sqlDataSource))
            {
                connect.Open();
                using (MySqlCommand command = new MySqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@id", id);
                    reader = command.ExecuteReader();
                    table.Load(reader);

                    reader.Close();
                    connect.Close();
                }
            }
            return new JsonResult(table);

        }
    }
}
