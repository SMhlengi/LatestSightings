using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatestSightingsLibrary
{
    [Serializable]
    public class Person
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public string CellNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string OtherContact { get; set; }
        public string Address { get; set; }
        public string Banking { get; set; }
        public string Paypal { get; set; }
        public string Facebook { get; set; }
        public string Skype { get; set; }
        public string Twitter { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public bool Active { get; set; }

        private const string SQL_INSERT_PERSON = "INSERT INTO latestsightings.dbo.people (id, firstname, lastname, email, password, active, role, cellNumber, telNumber, otherContact, twitter, facebook, skype, address, banking, paypal, accountType, accountNumber, branchName, branchCode) VALUES (@id, @firstname, @lastname, @email, @password, @active, @role, @cellNumber, @telNumber, @otherContact, @twitter, @facebook, @skype, @address, @banking, @paypal, @accountType, @accountNumber, @branchName, @branchCode);";
        private const string SQL_UPDATE_PERSON = "UPDATE latestsightings.dbo.people SET firstname = @firstname, lastname = @lastname, email = @email, password = @password, active = @active, role = @role, cellNumber = @cellNumber, telNumber = @telNumber, otherContact = @otherContact, twitter = @twitter, facebook = @facebook, skype = @skype, address = @address, banking = @banking, paypal = @paypal, modified = @modified, accountType = @accountType, accountNumber = @accountNumber, branchName = @branchName, branchCode = @branchCode WHERE (id = @id);";

        public static Person GetPerson(string id)
        {
            Person person = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand("SELECT * FROM latestsightings.dbo.people WHERE (id = @id)", conn);
                sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = id;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    person = new Person();
                    while (rdr.Read())
                    {
                        person.Id = rdr["id"].ToString();
                        person.FirstName = rdr["firstname"].ToString();
                        person.Email = rdr["email"].ToString();
                        person.LastName = rdr["lastname"].ToString();
                        person.Password = rdr["password"].ToString();
                        person.Role = Convert.ToInt32(rdr["role"]);
                        person.Active = Convert.ToBoolean(rdr["active"]);
                        person.Address = rdr["address"].ToString();
                        person.Banking = rdr["banking"].ToString();
                        person.Paypal = rdr["paypal"].ToString();
                        person.CellNumber = rdr["cellNumber"].ToString();
                        person.Facebook = rdr["facebook"].ToString();
                        person.Id = rdr["id"].ToString();
                        person.OtherContact = rdr["otherContact"].ToString();
                        person.Skype = rdr["skype"].ToString();
                        person.TelephoneNumber = rdr["telNumber"].ToString();
                        person.BranchCode = rdr["branchCode"].ToString();
                        person.BranchName = rdr["branchName"].ToString();
                        person.AccountNumber = rdr["accountNumber"].ToString();
                        person.AccountType = rdr["accountType"].ToString();
                        person.Twitter = rdr["twitter"].ToString();
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return person;
        }

        public static List<Person> GetPeople()
        {
            List<Person> people = new List<Person>();

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand("SELECT * FROM latestsightings.dbo.people ORDER BY firstname, lastname;", conn);
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Person person = new Person();
                        person.Id = rdr["id"].ToString();
                        person.FirstName = rdr["firstname"].ToString();
                        person.LastName = rdr["lastname"].ToString();
                        person.Email = rdr["email"].ToString();
                        people.Add(person);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }
            
            return people;
        }

        public static Person GetPerson(string email, string password)
        {
            Person person = null;
            string userId = string.Empty;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand("SELECT Id FROM latestsightings.dbo.people WHERE (email = @email AND password = @password AND active = 1)", conn);
                sqlQuery.Parameters.Add("email", System.Data.SqlDbType.VarChar).Value = email.Trim();
                sqlQuery.Parameters.Add("password", System.Data.SqlDbType.VarChar).Value = password.Trim();
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        userId = rdr["id"].ToString();
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            if (!String.IsNullOrEmpty(userId))
                person = GetPerson(userId);

            return person;
        }

        public static bool SavePerson(Person person)
        {
            bool saved = false;
            bool update = true;

            if (string.IsNullOrEmpty(person.Id))
            {
                person.Id = Guid.NewGuid().ToString();
                update = false;
            }

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = update == true ? SQL_UPDATE_PERSON : SQL_INSERT_PERSON;
                sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = person.Id == null ? string.Empty : person.Id;
                sqlQuery.Parameters.Add("firstname", System.Data.SqlDbType.VarChar).Value = person.FirstName == null ? string.Empty : person.FirstName;
                sqlQuery.Parameters.Add("lastname", System.Data.SqlDbType.VarChar).Value = person.LastName == null ? string.Empty : person.LastName;
                sqlQuery.Parameters.Add("email", System.Data.SqlDbType.VarChar).Value = person.Email == null ? string.Empty : person.Email;
                sqlQuery.Parameters.Add("password", System.Data.SqlDbType.VarChar).Value = person.Password == null ? string.Empty : person.Password;
                sqlQuery.Parameters.Add("active", System.Data.SqlDbType.Bit).Value = person.Active == null ? true : person.Active;
                sqlQuery.Parameters.Add("role", System.Data.SqlDbType.Int).Value = person.Role == null ? 3 : person.Role;
                sqlQuery.Parameters.Add("cellNumber", System.Data.SqlDbType.VarChar).Value = person.CellNumber == null ? string.Empty : person.CellNumber;
                sqlQuery.Parameters.Add("telNumber", System.Data.SqlDbType.VarChar).Value = person.TelephoneNumber == null ? string.Empty : person.TelephoneNumber;
                sqlQuery.Parameters.Add("otherContact", System.Data.SqlDbType.VarChar).Value = person.OtherContact == null ? string.Empty : person.OtherContact;
                sqlQuery.Parameters.Add("twitter", System.Data.SqlDbType.VarChar).Value = person.Twitter == null ? string.Empty : person.Twitter;
                sqlQuery.Parameters.Add("facebook", System.Data.SqlDbType.VarChar).Value = person.Facebook == null ? string.Empty : person.Facebook;
                sqlQuery.Parameters.Add("skype", System.Data.SqlDbType.VarChar).Value = person.Skype == null ? string.Empty : person.Skype;
                sqlQuery.Parameters.Add("address", System.Data.SqlDbType.VarChar).Value = person.Address == null ? string.Empty : person.Address;
                sqlQuery.Parameters.Add("banking", System.Data.SqlDbType.VarChar).Value = person.Banking == null ? string.Empty : person.Banking;
                sqlQuery.Parameters.Add("paypal", System.Data.SqlDbType.VarChar).Value = person.Paypal == null ? string.Empty : person.Paypal;
                sqlQuery.Parameters.Add("accountType", System.Data.SqlDbType.VarChar).Value = person.AccountType == null ? string.Empty : person.AccountType;
                sqlQuery.Parameters.Add("accountNumber", System.Data.SqlDbType.VarChar).Value = person.AccountNumber == null ? string.Empty : person.AccountNumber;
                sqlQuery.Parameters.Add("branchName", System.Data.SqlDbType.VarChar).Value = person.BranchName == null ? string.Empty : person.BranchName;
                sqlQuery.Parameters.Add("branchCode", System.Data.SqlDbType.VarChar).Value = person.BranchCode == null ? string.Empty : person.BranchCode;
                sqlQuery.Parameters.Add("modified", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                sqlQuery.ExecuteNonQuery();
                conn.Close();

                saved = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return saved;
        }

        public static string RoleName(int roleId)
        {
            string roleName = string.Empty;

            switch (roleId)
            {
                case 1:
                    roleName = "Administrator";
                    break;
                case 2:
                    roleName = "Finance";
                    break;
                case 3:
                    roleName = "Contributor";
                    break;
                default:
                    break;
            }

            return roleName;
        }
    }
}