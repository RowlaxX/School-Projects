using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD.core
{
    public class DatabaseConnection
    {
        private readonly string server;
        private readonly string database;
        private readonly string user;
        private readonly string password;
        private readonly int port;
        private MySqlConnection? connection;


        public DatabaseConnection(string server, int port, string database, string user, string password)
        {
            this.server = server;
            this.database = database;
            this.user = user;
            this.password = password;
            this.port = port;
        }

        private static string ConStr(string server, int port, string database, string user, string password)
        {
            return string.Format("server={0};port={1};user id={2}; password={3}; database={4}", server, port, user, password, database);
        }

        private string ConStr()
        {
            return ConStr(server, port, database, user, password);
        }

        private MySqlConnection Connect()
        {
            if (connection != null)
                return connection;

            connection = new MySqlConnection(ConStr());
            connection.Open();
            Console.WriteLine("Connected to database.");
            return connection;
        }

        public T RunInTransaction<T>(Action<MySqlCommand> setup, Func<MySqlDataReader, T> func)
        {
            MySqlConnection con = Connect();

            using MySqlCommand command = con.CreateCommand();
            setup.Invoke(command);
            Console.WriteLine("SQL : \'" + command.CommandText + "\'");
            Trace.WriteLine("SQL : \'" + command.CommandText + "\'");

            using MySqlDataReader reader = command.ExecuteReader();
            return func.Invoke(reader);
        }

        public T RunInTransaction<T>(string sql, Func<MySqlDataReader, T> func)
        {
            return RunInTransaction(s => s.CommandText = sql, func);
        }

        public void RunInTransaction(Action<MySqlCommand> setup, Action<MySqlDataReader> action)
        {
            RunInTransaction(setup, reader =>
            {
                action.Invoke(reader);
                return 0;
            });
        }

        public void RunInTransaction(string sql, Action<MySqlDataReader> action)
        {
            RunInTransaction(command => command.CommandText = sql, action);
        }

        public int ExecuteScalar(string sql)
        {
            MySqlConnection con = Connect();

            using MySqlCommand command = con.CreateCommand();
            command.CommandText = sql;
            Console.WriteLine("SQL : \'" + command.CommandText + "\'");
            Trace.WriteLine("SQL : \'" + command.CommandText + "\'");

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public void Execute(string sql)
        {
            MySqlConnection con = Connect();

            using MySqlCommand command = con.CreateCommand();
            command.CommandText = sql;
            Console.WriteLine("SQL : \'" + command.CommandText + "\'");
            Trace.WriteLine("SQL : \'" + command.CommandText + "\'");
            command.ExecuteNonQuery();
        }
    }
}
