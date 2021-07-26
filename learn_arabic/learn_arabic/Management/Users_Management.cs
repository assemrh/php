using learn_arabic.Classes;
using learn_arabic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Management
{
    public class Users_Management
    {
        //b0313575-b11a-4254-b8c3-d39b8a8773ec
        public async static Task<bool> Add(AddUserModel user, ER_Ref<string> msg, Ref<TokenModel> token)
        {
            //// add user info 
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "email", "full_name", "mobile", "username", "dob", "gender", "living_country", "is_admin", "token", "password", "created_at" };
            cols.AddRange(colsinput);
            string Token = await HelperClass.RandomString(50);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("users", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("users", ID);
            }
            string user_name = user.FullName.Split()[0] + "_" + ID.ToString();
            object[] valsinput = { user.Email, user.FullName, user.Phone, user_name, user.DOB, user.Gender, user.Country, 0, Token, await Ciphering.GetMD5HashDataAsync(user.Password), DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            token.Value = new TokenModel()
            {
                ID = ID.ToString(),
                Token = Token,
                User_Name = user_name
            };
            return await Database.InsertRow("users", ID, cols, vals, msg);

        }

        public async static Task<bool> Login(LoginModel user, ER_Ref<string> msg, Ref<TokenModel> token)
        {
            DataRow dtr = await Database.FindRow("users", "email", user.Email);
            if (dtr != null)
            {
                token.Value = new TokenModel()
                {
                    ID = dtr["id"].ToString(),
                    Token = dtr["token"].ToString(),
                    User_Name = dtr["username"].ToString()
                };
                return dtr["password"].ToString() == await Ciphering.GetMD5HashDataAsync(user.Password);
            }
            else
            {
                msg.Error = "Invalied Email !";
                return false;
            }

        }

        public async static Task<bool> Admin_Login(LoginModel user, ER_Ref<string> msg, Ref<TokenModel> token)
        {
            DataRow dtr = await Database.GetRow("users", new Guid("D06DACE1-63E9-4CFA-B55A-7178E31D0034"));
            if (dtr != null)
            {
                if (dtr["email"].ToString() == user.Email)
                {
                    if (dtr["password"].ToString() == await Ciphering.GetMD5HashDataAsync(user.Password))
                    {
                        token.Value = new TokenModel()
                        {
                            ID = dtr["id"].ToString(),
                            Token = dtr["token"].ToString(),
                            User_Name = dtr["username"].ToString()
                        };
                        return true;
                    }
                    else msg.Error = "password is not correct";
                }
                else msg.Error = "email is not correct";
            }
            else msg.Error = "there are no admins ";
            return false;
        }

        public async static Task<bool> GetCurrentUser(string token, Ref<DataRow> user)
        {
            user.Value = await Database.FindRow("users", "token", token);
            return user.Value != null;
        }

        public async static Task<VistorSuggestionModel> GetUserInfoForSuggestion(DataRow user, Ref<string> userID)
        {
            userID.Value = user["id"].ToString();
            return new VistorSuggestionModel()
            {
                Name = user["full_name"].ToString(),
                Email = user["email"].ToString(),
                Mobile = user["mobile"].ToString()
            };
        }

        public async static Task<bool> ChangePassword(ChangePasswordModel usermodel, ER_Ref<string> msg)
        {
            DataRow user = await Database.GetRow("users", usermodel.ID);
            if (user != null && user["password"].ToString() == await Ciphering.GetMD5HashDataAsync(usermodel.Password))
            {
                List<string> cols = new List<string>();
                List<Object> vals = new List<object>();

                string[] colsinput = { "password", "updated_at" };
                cols.AddRange(colsinput);

                object[] valsinput = { await Ciphering.GetMD5HashDataAsync(usermodel.New_Password), DateTime.Now.ToShortDateString() };
                vals.AddRange(valsinput);
                return await Database.UpdateRow("users", usermodel.ID, cols, vals, msg);
            }
            msg.Error = "password is not correct!";
            return false;
        }

        public async static Task<bool> ForgetPassword(string email, ER_Ref<string> msg)
        {
            DataRow user = await Database.FindRow("users", "email", email);
            if (user != null)
            {

                string verfication_code = await HelperClass.RandomString(6);
                string body = "<h2> " + "your verification code is " + " : " + verfication_code;
                if (await EmailManager.SendEmail("Forget Password", body, email, msg))
                {
                    Guid ID = new Guid(user["id"].ToString());
                    List<string> cols = new List<string>();
                    List<Object> vals = new List<object>();


                    string[] colsinput = { "otp_code", "otp_exp_date", "updated_at" };
                    cols.AddRange(colsinput);

                    object[] valsinput = { verfication_code, DateTime.Now.AddDays(1), DateTime.Now };
                    vals.AddRange(valsinput);

                    return await Database.UpdateRow("users", ID, cols, vals, msg);
                    
                }
                msg.Error = "sending email is failed  " + msg.Error;
                return false;
            }
            msg.Error = "email is not found!";
            return false;
        }

        public async static Task<bool> CheckCode(VerficationCodeModel model, ER_Ref<string> msg)
        {
            DataRow user = await Database.FindRow("users", "email", model.Email);
            if (user != null)
            {
                if(user["otp_code"] != null && user["otp_code"].ToString() == model.Code)
                {
                    DateTime exp_date = Convert.ToDateTime(user["otp_exp_date"].ToString());
                    if (DateTime.Now < exp_date)
                        return true;
                    msg.Error = "date is expired!!";
                    return false;
                }
                msg.Error = "Code is not correct!!";
                return false;
            }
            msg.Error = "email is not found!";
            return false;
        }

        public async static Task<bool> SetPassword(ResetPasswordModel model, ER_Ref<string> msg)
        {
            DataRow user = await Database.FindRow("users", "email", model.Email);
            if (user != null)
            {
                if (user["otp_code"] != null && user["otp_code"].ToString() == model.Code)
                {
                    DateTime exp_date = Convert.ToDateTime(user["otp_exp_date"].ToString());
                    if (DateTime.Now < exp_date)
                    {
                        List<string> cols = new List<string>();
                        List<Object> vals = new List<object>();

                        string[] colsinput = { "password", "updated_at" };
                        cols.AddRange(colsinput);

                        object[] valsinput = { await Ciphering.GetMD5HashDataAsync(model.Password), DateTime.Now };
                        vals.AddRange(valsinput);
                        Guid ID = new Guid(user["id"].ToString());
                        return await Database.UpdateRow("users", ID, cols, vals, msg);
                    }
                    msg.Error = "date is expired!!";
                    return false;
                }
                msg.Error = "Code is not correct!!";
                return false;
            }
            msg.Error = "email is not found!";
            return false;
        }

        public async static Task<UserModel> showProfile(string id, string lang, ER_Ref<string> msg)
        {
            string sql = @"select u.full_name, u.email, u.mobile, u.dob, c.value, u.gender,u.username   
               from users as u  inner join country_translations  as c  
                on u.living_country=c.src_id and c.language = @lang     where u.id =@id   ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            li.Add(new SqlParameter("@id", new Guid(id)));
            DataTable users = await Database.ReadTableByQuery(sql, li, msg);
            if (users != null && users.Rows.Count > 0)
            {
                DataRow row = users.Rows[0];
                return new UserModel()
                {
                    Country = row["value"].ToString(),
                    DOB = row["dob"].ToString(),
                    FullName = row["full_name"].ToString(),
                    Gender = row["gender"].ToString(),
                    Phone = row["mobile"].ToString(),
                    Username = row["username"].ToString(),
                    Email = row["email"].ToString()
                };
            }
            return null;
        }

        public async static Task<bool> edit(Guid ID, editUserModel user, ER_Ref<string> msg)
        {
            //// add user info 
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "email", "full_name", "mobile", "dob", "gender", "living_country", "is_admin", "updated_at" };
            cols.AddRange(colsinput);

            object[] valsinput = { user.Email, user.FullName, user.Phone, user.DOB, user.Gender, user.Country, 0, DateTime.Now };
            vals.AddRange(valsinput);

            return await Database.UpdateRow("users", ID, cols, vals, msg);

        }

        public async static Task<bool> delete(Guid ID, ER_Ref<string> msg)
        {
          if(await Database.CopyRow("users", "user_archived",ID,msg))
            {
                return await Database.DeleteRow("users", ID, msg);
            }
            return false;
        }

        public async static Task<List<UserModel>> GetAllUsers(string lang, ER_Ref<string> msg)
        {
            List<UserModel> dtList = new List<UserModel>();
            string sql = @"select u.id, u.full_name, u.email, u.mobile, u.dob, c.value, u.gender,u.username   
               from users as u  inner join country_translations  as c  
                on u.living_country=c.src_id and c.language like @lang   ";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable users = await Database.ReadTableByQuery(sql, li, msg);
            if (users != null && users.Rows.Count > 0)
            {
                dtList = users.AsEnumerable()
                       .Select(row => new UserModel
                       {
                           ID=row["id"].ToString(),
                           Country = row["value"].ToString(),
                           DOB = row["dob"].ToString(),
                           FullName = row["full_name"].ToString(),
                           Gender = row["gender"].ToString(),
                           Phone = row["mobile"].ToString(),
                           Username = row["username"].ToString(),
                           Email = row["email"].ToString()
                       }).ToList();
                return dtList;
            }
            return null;
        }
    }
}
