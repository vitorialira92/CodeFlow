using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Data.Common;
using CodeFlowBackend.Model.User;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.Security.Principal;
using System.IO;
using Microsoft.VisualBasic;


namespace CodeFlowBackend.Repositories
{


    internal abstract class DBConnection
    {
        private static readonly string _dbPath = "Data Source=codeflow.db;Version=3;";
        protected static SQLiteConnection _connection = new SQLiteConnection(_dbPath);
        protected static SQLiteCommand _command = new SQLiteCommand(_connection);


        protected static bool Open()
        {
            if (!File.Exists(@"codeflow.db"))
            {
                CreateDB();
                _connection.Open();
                return true;
            }
            else
            {
                try
                {
                    _connection = new SQLiteConnection(_dbPath);
                    _connection.Open();
                    return true;
                }
                catch (SQLiteException e)
                {
                    Console.WriteLine("An error occured when an attempt to open a connection to this database was made: " + e.Message);
                }
            }

            return false;
        }

        private static void CreateDB()
        {
            SQLiteConnection.CreateFile(@"codeflow.db");

            _connection = new SQLiteConnection($"{_dbPath}New=True;");
            _connection.Open();

            _command = new SQLiteCommand(_connection);
            _command.CommandText = @"CREATE TABLE user (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                username TEXT UNIQUE NOT NULL CHECK(length(username) <= 25),
                email TEXT UNIQUE NOT NULL,
                first_name TEXT NOT NULL,
                last_name TEXT NOT NULL,
                date_joined DATETIME NOT NULL,
                role INTEGER NOT NULL CHECK(role IN (1, 2)),
                password TEXT NOT NULL);";
            _command.ExecuteNonQuery(); //user 

            _command.CommandText = @"CREATE TABLE techleader (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                user_id INTEGER NOT NULL UNIQUE,
                specialization INTEGER NOT NULL CHECK(specialization IN (1, 2, 3)),
                FOREIGN KEY (user_id) REFERENCES user(id)
            );";
            _command.ExecuteNonQuery();//tech leader

            _command.CommandText = @"CREATE TABLE developer (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                user_id INTEGER NOT NULL UNIQUE,
                experience_level INTEGER NOT NULL CHECK(experience_level IN (1, 2, 3, 4)),
                FOREIGN KEY (user_id) REFERENCES user(id)
            );";
            _command.ExecuteNonQuery();//devs

            _command.CommandText = @"CREATE TABLE project (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                techleader_id INTEGER NOT NULL,
                name TEXT NOT NULL CHECK(length(name) <= 30),
                description TEXT NOT NULL CHECK(length(description) <= 150),
                due_date DATETIME NOT NULL,
                status INTEGER NOT NULL CHECK(status IN (1, 2, 3, 4, 5)),
                FOREIGN KEY (techleader_id) REFERENCES techleader(id)
            );";
            _command.ExecuteNonQuery(); //project

            _command.CommandText = @"CREATE TABLE project_members (
                project_id INTEGER NOT NULL,
                member_id INTEGER NOT NULL,
                enter_date DATETIME NOT NULL,
                left_date DATETIME,
                PRIMARY KEY (project_id, member_id),
                FOREIGN KEY (project_id) REFERENCES project(id),
                FOREIGN KEY (member_id) REFERENCES user(id)
            );";
            _command.ExecuteNonQuery(); //project members

            _command.CommandText = @"CREATE TABLE tag (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                project_id INTEGER NOT NULL,
                name TEXT NOT NULL,
                color TEXT NOT NULL CHECK(length(color) = 7),
                FOREIGN KEY (project_id) REFERENCES project(id)
            );";
            _command.ExecuteNonQuery();//TAG

            _command.CommandText = @"CREATE TABLE assignment (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                project_id INTEGER NOT NULL,
                name TEXT NOT NULL CHECK(length(name) <= 30),
                description TEXT NOT NULL CHECK(length(description) <= 150),
                due_date DATETIME,
                assignee INTEGER,
                status INTEGER NOT NULL CHECK(status IN (1, 2, 3, 4, 5)),
                tag_id INTEGER,
                FOREIGN KEY (project_id) REFERENCES project(id),
                FOREIGN KEY (assignee) REFERENCES user(id),
                FOREIGN KEY (tag_id) REFERENCES tag(id)
            );";//assignment
            _command.ExecuteNonQuery() ;

            

            _command.CommandText = @"CREATE TABLE checklist (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                assignment_id INTEGER NOT NULL,
                name TEXT CHECK(length(name) <= 40),
                isDone INTEGER NOT NULL CHECK(isDone IN (0, 1)),
                FOREIGN KEY (assignment_id) REFERENCES assignment(id)
            );";
            _command.ExecuteNonQuery();//checklist

            _command.CommandText = @"CREATE TABLE user_salt (
                user_id INTEGER NOT NULL,
                salt TEXT NOT NULL,
                PRIMARY KEY (user_id),
                FOREIGN KEY (user_id) REFERENCES user (id)
            );";
            _command.ExecuteNonQuery();

            _command.CommandText = @"CREATE TABLE project_invite (
                user_id INTEGER NOT NULL,
                project_id INTEGER NOT NULL,
                entered INTEGER NOT NULL CHECK(entered IN (0,1)),
                PRIMARY KEY (user_id, project_id),
                FOREIGN KEY (project_id) REFERENCES project (id),
                FOREIGN KEY (user_id) REFERENCES user (id)
            );";
            _command.ExecuteNonQuery();

            _command.CommandText = @"CREATE TABLE project_entercode (
                project_id INTEGER NOT NULL,
                entercode TEXT NOT NULL CHECK(length(entercode) = 6),
                PRIMARY KEY (project_id),
                FOREIGN KEY (project_id) REFERENCES project (id)
            );";
            _command.ExecuteNonQuery();

            InsertInitialData();
        }

        private static void InsertInitialData()
        {
            TechLeader user = new TechLeader(Model.User.Enum.Specializations.Backend, "vitorialira", "Vitória", "Tenorio",
                "vitoriatenorio@estudante.ufscar.br", "vitoria1234");

            string query = @"INSERT INTO user (username, email, first_name, " +
                "last_name, date_joined, role, password) VALUES (@username, @email, @firstName, " +
                "@lastName, @dateJoined, @role, @password); insert into techleader(user_id, specialization)" +
                @" values (1, 2);";

            _command = new SQLiteCommand(query, _connection);
            (string,string) a = HashUtil.GetHashedAndSalt(user._password);
            _command.Parameters.AddWithValue("@username", user.Username);
            _command.Parameters.AddWithValue("@email", user.Email);
            _command.Parameters.AddWithValue("@firstName", user.FirstName);
            _command.Parameters.AddWithValue("@lastName", user.LastName);
            _command.Parameters.AddWithValue("@dateJoined", user.DateJoined);
            _command.Parameters.AddWithValue("@role", (int)user.Role);
            _command.Parameters.AddWithValue("@password", a.Item1);

            _command.ExecuteNonQuery();

            query = @"INSERT INTO user_salt (user_id, salt) VALUES (@user_id, @salt);";

            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@user_id", 1);
            _command.Parameters.AddWithValue("@salt", a.Item2);

            _command.ExecuteNonQuery();

            query = @"insert into project (techleader_id, name, description, due_date, status)" +
                @"values (@techleader, @name, @description, @duedate, @status);" +
                @"insert into project_members (project_id, member_id, enter_date) values (1,1, @enterdate)";
            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@techleader", 1);
            _command.Parameters.AddWithValue("@name", "Code Flow 2");
            _command.Parameters.AddWithValue("@description", "descrição do projeto 2");
            _command.Parameters.AddWithValue("@duedate", DateTime.Today);
            _command.Parameters.AddWithValue("@status", 1);
            _command.Parameters.AddWithValue("@enterdate", DateTime.Today);

            _command.ExecuteNonQuery();
            

            query = @"insert into project (techleader_id, name, description, due_date, status)" +
                @"values (@techleader, @name, @description, @duedate, @status);" +
                @"insert into project_members (project_id, member_id, enter_date) values (2,1, @enterdate)";

            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@techleader", 1);
            _command.Parameters.AddWithValue("@name", "Code Flow");
            _command.Parameters.AddWithValue("@description", "descrição do projeto");
            _command.Parameters.AddWithValue("@duedate", DateTime.Today);
            _command.Parameters.AddWithValue("@status", 3);
            _command.Parameters.AddWithValue("@enterdate", DateTime.Today);

            _command.ExecuteNonQuery();

            query = @"insert into project (techleader_id, name, description, due_date, status)" +
                @"values (@techleader, @name, @description, @duedate, @status);" +
                @"insert into project_members (project_id, member_id, enter_date) values (3,1, @enterdate)";

            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@techleader", 1);
            _command.Parameters.AddWithValue("@name", "Code Flow 3");
            _command.Parameters.AddWithValue("@description", "descrição do projeto 3");
            _command.Parameters.AddWithValue("@duedate", DateTime.Today);
            _command.Parameters.AddWithValue("@status", 5);
            _command.Parameters.AddWithValue("@enterdate", DateTime.Today);

            _command.ExecuteNonQuery();

            query = @"insert into project (techleader_id, name, description, due_date, status)" +
               @"values (@techleader, @name, @description, @duedate, @status);" +
               @"insert into project_members (project_id, member_id, enter_date) values (4,1, @enterdate)";

            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@techleader", 1);
            _command.Parameters.AddWithValue("@name", "Code Flow 4");
            _command.Parameters.AddWithValue("@description", "descrição do projeto 4");
            _command.Parameters.AddWithValue("@duedate", DateTime.Today);
            _command.Parameters.AddWithValue("@status", 5);
            _command.Parameters.AddWithValue("@enterdate", DateTime.Today);

            _command.ExecuteNonQuery();

            query = @"insert into project (techleader_id, name, description, due_date, status)" +
               @"values (@techleader, @name, @description, @duedate, @status);" +
               @"insert into project_members (project_id, member_id, enter_date) values (5,1, @enterdate)";

            _command = new SQLiteCommand(query, _connection);
            _command.Parameters.AddWithValue("@techleader", 1);
            _command.Parameters.AddWithValue("@name", "Code Flow 5");
            _command.Parameters.AddWithValue("@description", "descrição do projeto 5");
            _command.Parameters.AddWithValue("@duedate", DateTime.Today);
            _command.Parameters.AddWithValue("@status", 5);
            _command.Parameters.AddWithValue("@enterdate", DateTime.Today);

            _command.ExecuteNonQuery();
            

            Close();
            //seting permissions
            using (var connection = new SQLiteConnection("Data Source=codeflow.db;Version=3;"))
            {
                connection.Open();
                using (var command = new SQLiteCommand("PRAGMA journal_mode=WAL;", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

        }

        protected static bool Close()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (SQLiteException e)
            {
                Console.WriteLine("An error occured when an attempt to close a connection to this database was made: " + e.Message);
            }
            return false;
        }

    }

}
