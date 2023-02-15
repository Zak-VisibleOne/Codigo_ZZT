using POSData;
using POSSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSSystem.Models
{
    public class LoginModel
    {
        public List<User> auth(string username, int password, string passOri, ref string errorMessage)
        {
            sqlDescriptor sd = new sqlDescriptor();
            try
            {
                var result = sd.GetDataByQuery<User>("login/auth", new object[] {
                0, username, password, passOri}).ToList();
                return result;
            }
            catch (Exception e)
            {
                string[] error = e.Message.Split('\'');
                errorMessage = error[3] + " column does not exit in " + error[1].Split('.')[1] + " table.";
                return null;
            }
        }
        public void UpdateState(string username, int state)
        {
            sqlDescriptor sd = new sqlDescriptor();
            var result = sd.GetDataByQuery<ResultMessage>("login/UpdateState", new object[] {
                state, username
            }).ToList();
        }
        public int CurrentLogin(string username)
        {
            sqlDescriptor sd = new sqlDescriptor();
            var result = sd.GetDataByQuery<ResultMessage>("login/CheckCurrentLogin", new object[] {
                username
            }).FirstOrDefault().cnt;
            return result;
        }
        public ResultMessage isLoggedIn(string username)
        {
            sqlDescriptor sd = new sqlDescriptor();
            var result = sd.GetDataByQuery<ResultMessage>("login/isLoggedIn", new object[] {
                username
            }).FirstOrDefault();
            return result;
        }
        public void updateLog(string ip, string sesion_id, string user)
        {
            sqlDescriptor sd = new sqlDescriptor();
            var result = sd.GetDataByQuery<ResultMessage>("login/UpdateLog", new object[] {
                sesion_id, ip,user
            }).ToList();
        }
    }
}