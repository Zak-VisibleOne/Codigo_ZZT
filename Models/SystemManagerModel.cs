using POSData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using static POSSystem.Models.ViewModel;

namespace POSSystem.Models
{
    public class SystemManagerModel
    {
        string connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        #region User Form
        public List<User> UsersList()
        {
            List<User> lst = new List<User>();
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].SelectUser", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new User
                    {
                        Id = Convert.ToInt32(rdr["ID"]),
                        UserCode = rdr["UserCode"].ToString(),
                        UserName = rdr["UserName"].ToString(),
                        UserGroupCode = rdr["UserGroupCode"].ToString(),
                        RoleID = Convert.ToInt32(rdr["RoleID"]),
                        Email = rdr["Email"].ToString(),
                        ImageUrl = rdr["ImageUrl"].ToString(),
                        IsLoginLimit = Convert.ToInt32(rdr["IsLoginLimit"]),
                        DeletedStatus = Convert.ToBoolean(rdr["DeletedStatus"])
                    });
                }
                return lst;
            }
        }
        public int EncryptPassword(string Password)
        {
            int newPassword = 0;
            int pLen = Password.Length;

            for (int i = 0; i < pLen; i++)
            {
                newPassword = newPassword + ((Encoding.ASCII.GetBytes(Password.ToString().Substring(i, 1))[0] - pLen + i + 1) * ((i + 1) * (i + 1) * (i + 1)));
            }

            return newPassword;
        }
        public int SaveNewUser(ViewModelUserInfo user)
        {
            int i;
            var password = 0;
            password = EncryptPassword(user.Password ?? "");            
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].InsertUpdateUser", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", user.Id);
                com.Parameters.AddWithValue("@UserCode", user.UserCode);
                com.Parameters.AddWithValue("@UserName", user.UserName);
                com.Parameters.AddWithValue("@UserGroup", user.UserGroupCode);
                com.Parameters.AddWithValue("@Email", user.Email);
                com.Parameters.AddWithValue("@ImageUrl", user.ImageUrl ?? "");
                com.Parameters.AddWithValue("@DefaultCompany", "vs");
                com.Parameters.AddWithValue("@Password", password);
                com.Parameters.AddWithValue("@IsLoginLimit", user.IsLoginLimit ?? 20);
                com.Parameters.AddWithValue("@DeletedStatus", user.DeletedStatus ?? false);
                com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        public int UpdateUser(User user)
        {
            int i;
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].InsertUpdateUser", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", user.Id);
                com.Parameters.AddWithValue("@UserCode", user.UserCode ?? "");
                com.Parameters.AddWithValue("@UserName", user.UserName ?? "");
                com.Parameters.AddWithValue("@UserGroup", user.UserGroupCode ?? "");
                com.Parameters.AddWithValue("@Email", user.Email ?? "");
                com.Parameters.AddWithValue("@ImageUrl", user.ImageUrl ?? "");
                com.Parameters.AddWithValue("@DefaultCompany", "vs");
                com.Parameters.AddWithValue("@Password", 0);
                com.Parameters.AddWithValue("@IsLoginLimit", user.IsLoginLimit ?? 20);
                com.Parameters.AddWithValue("@DeletedStatus", user.DeletedStatus ?? false);
                com.Parameters.AddWithValue("@Action", "Update");
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        public int Delete(int ID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].DeleteUser", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", ID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        #endregion
        #region UserGroup Form
        public List<UserGroup> UserGroupList()
        {
            List<UserGroup> lst = new List<UserGroup>();
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].SelectUserGroup", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new UserGroup
                    {
                        ID = Convert.ToInt32(rdr["ID"]),
                        GroupName = rdr["GroupName"].ToString(),
                        GroupCode = rdr["GroupCode"].ToString(),
                    });
                }
                return lst;
            }
        }
        public int AddUserGroup(UserGroup usergroup)
        {
            int i;
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].InsertUpdateUserGroup", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", usergroup.ID);
                com.Parameters.AddWithValue("@GroupCode", usergroup.GroupCode);
                com.Parameters.AddWithValue("@GroupName", usergroup.GroupName);
                com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        public int UpdateUserGroup(UserGroup usergroup)
        {
            int i;
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].InsertUpdateUserGroup", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", usergroup.ID);
                com.Parameters.AddWithValue("@GroupCode", usergroup.GroupCode);
                com.Parameters.AddWithValue("@GroupName", usergroup.GroupName);
                com.Parameters.AddWithValue("@Action", "Update");
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        public int DeleteUserGroup(int groupID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].DeleteUserGroup", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", groupID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        #endregion
        #region SiteInfo
        public class SiteInfoModel
        {
            public string Name { get; set; }
            public string Data { get; set; }
        }
        public class unReadMessage
        {
            public string Name { get; set; }
            public int Total { get; set; }
        }
        public List<SiteInfoModel> SiteInfo()
        {
            List<SiteInfoModel> lst = new List<SiteInfoModel>();
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[dbo].SiteInfo", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new SiteInfoModel
                    {
                        Name = rdr["Name"].ToString(),
                        Data = rdr["Data"].ToString()
                    });
                }
                return lst;
            }
        }
        
        #endregion
    }
}