using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace WPFUI
{
    public class SqliteDataAccess
    {
        public static List<Contact> LoadContacts()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Contact>("select * from Contact", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveContact(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException();
            }

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Contact (Name, CellphoneNumber, TelephoneNumber) values (@Name, @CellphoneNumber, @TelephoneNumber)", contact);
            }
        }

        public static void SaveContact(List<Contact> contacts)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                foreach (Contact c in contacts)
                {
                    if (c == null)
                    {
                        throw new ArgumentNullException();
                    }
                    
                    cnn.Execute("insert into Contact (Name, CellphoneNumber, TelephoneNumber) values (@Name, @CellphoneNumber, @TelephoneNumber)", c);
                }
            }
        }

        public static void DeleteContact(Contact contact)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("DELETE FROM Contact WHERE Id=" + contact.Id);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}