
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace learn_arabic.Classes
{
    public class Database
    {
       public static String ConnectionString = "Server=\".\\SQLEXPRESS\";Initial Catalog=\"learn-arabic\";User ID=\"sa\";Password=\"Usr123456\"";
        //public static String ConnectionString = "Server=\"tornado-soft.com\";Initial Catalog=\"sgaw\";User ID=\"sgaw\";Password=\"Usr@123456\"";

        public static DataTable ReadTable(String tblName, String c, List<SqlParameter> lstParams, out string errMessage)
        {

            SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM " + tblName + " " + c, ConnectionString);
            if (lstParams != null) adp.SelectCommand.Parameters.AddRange(lstParams.ToArray());
            DataTable tbl = new DataTable();
            try
            {
                adp.Fill(tbl);
                errMessage = "";
                return tbl;
            }
            catch (Exception ex) {
                errMessage = ex.Message;
                Tools.SaveError(ex.Message);
                return null;
            }
        }

        public static DataTable ReadTable(String tblName, String c, List<SqlParameter> lstParams, int Rows_Count, out string errMessage)
        {
            SqlDataAdapter adp = new SqlDataAdapter("SELECT TOP (" + Rows_Count + ") * FROM " + tblName + " " + c, ConnectionString);
            if (lstParams != null) adp.SelectCommand.Parameters.AddRange(lstParams.ToArray());
            DataTable tbl = new DataTable();
            try
            {
                adp.Fill(tbl);
                errMessage = "";
                return tbl;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Tools.SaveError(ex.Message);
                return null;
            }
        }

        public async static Task<bool> DeleteRow(string tblName, List<String> cols, List<Object> vals, ER_Ref<string> msg)
        {
            if (vals == null || cols == null) return false;
            if (vals.Count == 0 || cols.Count == 0) return false;
            try
            {
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("", cn);

                string colValStr = "";

                for (int oCounter = 0; oCounter <= cols.Count - 1; oCounter++)
                {
                    colValStr += cols[oCounter] + " = " + "@param" + (oCounter + 1).ToString();

                    if (oCounter != cols.Count - 1)
                    {
                        colValStr += " AND ";
                    }

                    cmd.Parameters.AddWithValue("@param" + (oCounter + 1).ToString(), vals[oCounter]);
                }

                string qStr = "DELETE FROM " + tblName + " WHERE " + colValStr;
                cmd.CommandText = qStr;

                cn.Open();
                int cnt = cmd.ExecuteNonQuery();
                cn.Close();

                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return false;
            }
        }

        public async static Task<bool> DeleteRow(String tblName, Guid ID, ER_Ref<string> msg)
        {
            try
            {
                msg.Error = "";
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("", cn);

                string qStr = "DELETE FROM " + tblName + " WHERE ID = @id";
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.CommandText = qStr;

                cn.Open();
                int cnt = cmd.ExecuteNonQuery();
                cn.Close();

                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                Tools.SaveError(ex.Message);
                return false;
            }
        }

        public static int RowCount(string tblName)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM " + tblName, cn);

            try
            {
                cn.Open();
                int cnt = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                cn.Close();
                return cnt;
            }
            catch (Exception ex)
            {
                Tools.SaveError(ex.Message);
                return 0;
            }
        }


        public static DataTable ReadTable(String tblName, [Optional] out string errrMessage)
        {
            errrMessage = "";
            SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM " + tblName, ConnectionString);

            DataTable tbl = new DataTable();
            try
            {
                adp.Fill(tbl);
                return tbl;
            }
            catch (Exception ex) {
                errrMessage = ex.Message;
                Tools.SaveError(ex.Message);
                return null;
            }
        }
        public async static Task<DataTable> ReadTableByQuery(String query, List<SqlParameter> lstPArams, ER_Ref<string>  errrMessage)
        {
           // errrMessage =  String.Empty;
            SqlDataAdapter adp = new SqlDataAdapter(query, ConnectionString);
            if (lstPArams != null) adp.SelectCommand.Parameters.AddRange(lstPArams.ToArray());

            DataTable tbl = new DataTable();
            try
            {
                adp.Fill(tbl);
                return tbl;
            }
            catch (Exception ex)
            {
                errrMessage.Error = ex.Message;
                Tools.SaveError(ex.Message);
                return null;
            }
        }
        public async static Task<DataSet> ReadDataByQuery(String query, List<SqlParameter> lstPArams, ER_Ref<string> errrMessage)
        {
            // errrMessage =  String.Empty;
            SqlDataAdapter adp = new SqlDataAdapter(query, ConnectionString);
            if (lstPArams != null) adp.SelectCommand.Parameters.AddRange(lstPArams.ToArray());

            DataSet tbl = new DataSet();
            try
            {
                adp.Fill(tbl);
                return tbl;
            }
            catch (Exception ex)
            {
                errrMessage.Error = ex.Message;
                Tools.SaveError(ex.Message);
                return null;
            }
        }
        public async static Task<object> ReadValueByQuery(String query, List<SqlParameter> lstPArams)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(query, cn);
            if (lstPArams != null) cmd.Parameters.AddRange(lstPArams.ToArray());
            try
            {
               await cn.OpenAsync();
                object prop_value = cmd.ExecuteScalarAsync();
                await cn.CloseAsync();
                return prop_value;
            }
            catch
            {
                if (cn.State == ConnectionState.Open) await cn.CloseAsync();
                return null;
            }
        }
        public async static Task<bool> UpdateRow(string tblName, Guid ID, List<String> cols, List<Object> vals,ER_Ref<string> msg)
        {
            msg.Error = string.Empty;
            if (vals == null || cols == null) return false;
            if (vals.Count == 0 || cols.Count == 0) return false;
            try
            {
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("", cn);

                string colValStr = "";

                for (int oCounter = 0; oCounter <= cols.Count - 1; oCounter++)
                {
                    colValStr += cols[oCounter] + " = " + "@param" + (oCounter + 1).ToString();

                    if (oCounter != cols.Count - 1)
                    {
                        colValStr += " , ";
                    }

                    cmd.Parameters.AddWithValue("@param" + (oCounter + 1).ToString(), vals[oCounter]);
                }

                string qStr = "Update " + tblName + " SET " + colValStr + " WHERE ID = @id";
                cmd.Parameters.AddWithValue("@id" , ID);
                cmd.CommandText = qStr;

               await cn.OpenAsync();
                int cnt = await cmd.ExecuteNonQueryAsync();
                await cn.CloseAsync();

                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                Tools.SaveError(ex.Message);
                return false;
            }
        }
        public async static Task<bool> InsertTranslation(string tblName, string language, string value, Guid src_id, ER_Ref<string> msg)
        {
            msg.Error = string.Empty;

            try
            {
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("", cn);



                string qStr = "Insert Into " + tblName + " (id, src_id , language , value , created_at) Values (@id,@src_id ,@language ,@value,@date_ ); ";
                cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@src_id", src_id);
                cmd.Parameters.AddWithValue("@language", language);
                cmd.Parameters.AddWithValue("@date_", DateTime.Now);
                cmd.CommandText = qStr;

                await cn.OpenAsync();
                int cnt = await cmd.ExecuteNonQueryAsync();
                await cn.CloseAsync();

                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                Tools.SaveError(ex.Message);
                return false;
            }
        }

        public async static Task<bool> InsertTranslation(string tblName, string language, string value, string description, Guid src_id,Ref<Guid> TID, ER_Ref<string> msg )
        {
            msg.Error = string.Empty;
            TID.Value = Guid.NewGuid();
            try
            {
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("", cn);



                string qStr = "Insert Into " + tblName + " (id, src_id , language , value ,description , created_at) Values (@id,@src_id ,@language ,@value,@description,@date_ ); ";
                cmd.Parameters.AddWithValue("@id", TID.Value);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@src_id", src_id);
                cmd.Parameters.AddWithValue("@language", language);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@date_", DateTime.Now);
                cmd.CommandText = qStr;

                await cn.OpenAsync();
                int cnt = await cmd.ExecuteNonQueryAsync();
                await cn.CloseAsync();

                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                Tools.SaveError(ex.Message);
                return false;
            }
        }
        public async static Task<bool> UpdateTranslation(string tblName, string language,string value, Guid src_id , ER_Ref<string> msg)
        {
            msg.Error = string.Empty;
            
            try
            {
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("", cn);

                

                string qStr = "Update " + tblName + " SET value = @value, updated_at = @updated_at where src_id = @src_id and language = @language";
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@src_id", src_id);
                cmd.Parameters.AddWithValue("@language", language);
                cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
                cmd.CommandText = qStr;

                await cn.OpenAsync();
                 int cnt = await cmd.ExecuteNonQueryAsync();
                await cn.CloseAsync();

                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                Tools.SaveError(ex.Message);
                return false;
            }
        }

        public async static Task<bool> UpdateTranslation(string tblName, string language, string value, string description, Guid src_id,Ref<Guid> TID, ER_Ref<string> msg)
        {
            msg.Error = string.Empty;
            TID.Value = new Guid();
            try
            {
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("", cn);
                string qStr = "Update " + tblName + " SET value = @value, updated_at = @updated_at, description = @description where  src_id = @src_id and language = @language";
                cmd.Parameters.AddWithValue("@value", value);
                cmd.Parameters.AddWithValue("@src_id", src_id);
                cmd.Parameters.AddWithValue("@language", language);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
                cmd.CommandText = qStr +
                    " ; select id from " + tblName + "where  src_id = @src_id and language = @language ";

                await cn.OpenAsync();
                object prop_value = await cmd.ExecuteScalarAsync();
                await cn.CloseAsync();
                TID.Value = new Guid(prop_value.ToString());
                return TID.Value!= new Guid();

            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                Tools.SaveError(ex.Message);
                return false;
            }
        }

        public async static Task<bool> InsertRow(string tblName, Guid ID, List<String> cols, List<Object> vals, ER_Ref<string> msg)
        {

            if (vals == null || cols == null) return false;
            if (vals.Count == 0 || cols.Count == 0) return false;
            try
            {
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("", cn);

                string colStr = "ID";
                string valStr = "@id";
                cmd.Parameters.AddWithValue("@id", ID);

                for (int oCounter = 0; oCounter <= cols.Count - 1; oCounter++)
                {
                    colStr += ",";
                    valStr += ",";

                    colStr += cols[oCounter];
                    valStr += "@param" + (oCounter + 1).ToString();

                    cmd.Parameters.AddWithValue("@param" + (oCounter + 1).ToString(), vals[oCounter]);
                }

                string qStr = "INSERT INTO " + tblName + " (" + colStr + ") VALUES (" + valStr + ")";
                cmd.CommandText = qStr;

              await  cn.OpenAsync();
                int cnt = await cmd.ExecuteNonQueryAsync();
                await cn.CloseAsync();

                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return false;
            }
        }

        public async static Task<bool> CopyRow(string From_tblName, string To_tblName, Guid ID, ER_Ref<string> msg)
        {
            try
            {
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("", cn);

                string sql = "INSERT INTO "+ To_tblName+
                     " select * from "+From_tblName+" where id =@id ";
                SqlParameter parameter = new SqlParameter("@id", ID);

                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("@id", ID));

              //  string qStr = "INSERT INTO " + tblName + " (" + colStr + ") VALUES (" + valStr + ")";
              //  cmd.CommandText = qStr;

                await cn.OpenAsync();
                int cnt = await cmd.ExecuteNonQueryAsync();
                await cn.CloseAsync();

                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
                return false;
            }
        }

        public async static Task<DataRow> GetRow(String tblName, Guid ID)
        {
            SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM " + tblName + " WHERE ID = @id", ConnectionString);
            adp.SelectCommand.Parameters.Add(new SqlParameter("@id", ID));
            DataTable tbl = new DataTable();
            adp.Fill(tbl);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                return tbl.Rows[0];
            }
            else
            {
                return null;
            }
        }

        public async static Task<DataRow> FindRow(string tblName, string colName, object v)
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM " + tblName + " WHERE " + colName + " LIKE @v", ConnectionString);
                adp.SelectCommand.Parameters.Add(new SqlParameter("@v", v));
                DataTable tbl = new DataTable();
                adp.Fill(tbl);
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    return tbl.Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Tools.SaveError(ex.Message);
                return null;
            }

        }


        public async static Task<object> ReadValue(string tblName, string colName, Guid id)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT "+colName+" FROM "+tblName+" WHERE ID = @id", cn);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                cn.Open();
                object prop_value =await cmd.ExecuteScalarAsync();
                cn.Close();
                return prop_value;
            }
            catch
            {
                if (cn.State == ConnectionState.Open) cn.Close();
                return null;
            }

        }

        public static int Get_Users_Count()
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT  Counter FROM  Counter_TBL",cn);
            try
            {
                cn.Open();

                object prop_value = cmd.ExecuteScalar();
                cn.Close();
                return Convert.ToInt32(prop_value);
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open) cn.Close();
                return 0;
            }

        }

        public static bool Set_Users_Count(int count)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            string command = count == 1 ? "INSERT INTO  Counter_TBL (Counter) VALUES(1)" : "UPDATE Counter_TBL SET Counter = " + count;
            SqlCommand cmd = new SqlCommand(command,cn);
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                return true;
            }
            catch
            {
                if (cn.State == ConnectionState.Open) cn.Close();
                return false;
            }

        }
        public async static Task<String> ReadProp(int id)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT value FROM properties WHERE [index] = @id", cn);
            cmd.Parameters.AddWithValue("@id", id);

            String v = "";

            try
            {
               await cn.OpenAsync();
                SqlDataReader rd = await cmd.ExecuteReaderAsync();
                if (await rd.ReadAsync())
                {
                    v = rd["value"].ToString();
                }
                await rd.CloseAsync();
                await cn.CloseAsync();
                return v;
            }
            catch(Exception ex)
            {
                if (cn.State == ConnectionState.Open) await cn.CloseAsync();
                Tools.SaveError(ex.Message);
                return v;
            }

        }
        public async static Task<bool> WriteProp(int id, String v,ER_Ref<string> errMessage)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("IF NOT EXISTS (SELECT [index] FROM properties WHERE [index] = @id) INSERT INTO properties ([index], value) VALUES (@id, @v) ELSE UPDATE properties SET value = @v WHERE [index] = @id", cn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@v", v);
            try
            {
               await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await cn.CloseAsync();
                errMessage.Error = "";
                return true;
            }
            catch (Exception ex)
            {
                if (cn.State == ConnectionState.Open) await cn .CloseAsync();
                errMessage.Error = ex.Message;
                Tools.SaveError(ex.Message);
                return false;
            }

        }

        public async static Task<DataTable> ConverSQLQueryPage(string sql, List<SqlParameter> lstPArams, string order_col,int page_number,int per_page_number, ER_Ref<string> msg ,Ref<int> count)
        {
            if (string.IsNullOrWhiteSpace(order_col))
                order_col = " (select null) ";
            string Sql_Statment = "DECLARE @page int = " + page_number + " ";
            Sql_Statment += "DECLARE @per_page int = " + per_page_number + " ";
            Sql_Statment += "SELECT *  FROM (   " + sql + " ) AS t " ;

            Sql_Statment += "ORDER BY " + order_col + " ";
            Sql_Statment += " OFFSET @per_page * (@page - 1) ROWS ";
            Sql_Statment += " FETCH NEXT @per_page ROWS ONLY";

            Sql_Statment += " ; SELECT Count(*) FROM (   " + sql + " ) AS t1 ; ";
            DataSet data = await ReadDataByQuery(Sql_Statment, lstPArams, msg);

            try
            {
                int c = Convert.ToInt32( data?.Tables[1].Rows[0][0] ?? 0 );
                count.Value = c;
            }
            catch {        
            }
         
            return data?.Tables[0];

        }

        public async static Task<int> Get_Pages_count (string sql, List<SqlParameter> lstPArams, ER_Ref<string> msg)
        {
            string Sql_Statment = "SELECT Count(*) FROM (   " + sql + " ) AS t ";
            object c =await ReadValueByQuery(Sql_Statment, lstPArams);
            int count = 0;
            msg = "";
            try
            {
                count = Convert.ToInt32(c);
            }
            catch(Exception ex)
            {
                msg.Error = ex.Message;
                count = 0;
            }
            return count;
        }
    }
}