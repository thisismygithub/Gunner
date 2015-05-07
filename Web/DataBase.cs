using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;

/// <summary>
/// 資料庫相關程序。
/// </summary>
public static class DataBase
{
    /// <summary>
    /// 執行SQL指令(SQL版本)
    /// </summary>
    /// <param name="cnString"></param>
    /// <param name="sqlString"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static bool ExecuteSQLForSQL(string cnString, string sqlString, SqlParameter parameter)
    {
        using (var cn =
            new SqlConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                    new SqlCommand(sqlString, cn))
                {
                    cm.Parameters.Clear();
                    cm.Parameters.Add(parameter);
                    cm.CommandTimeout = 180;
                    cm.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.ToString();
                return false;
            }
        }
    }

    /// <summary>
    /// 執行SQL指令(SQL版本)(App)
    /// </summary>
    /// <param name="cnString"></param>
    /// <param name="sqlString"></param>
    /// <param name="_params"></param>
    /// <returns></returns>
    public static bool ExecuteSQLForSQL(string cnString, string sqlString, SqlParameter[] _params)
    {
        using (var cn =
            new SqlConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                    new SqlCommand(sqlString, cn))
                {
                    cm.Parameters.Clear();
                    foreach (var param in _params)
                    {
                        cm.Parameters.Add(param);
                    }
                    cm.CommandTimeout = 180;
                    cm.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.ToString();
                return false;
            }
        }
    }

    /// <summary>
    /// 執行SQL指令(SQL版本)
    /// </summary>
    /// <param name="cnString">connection string</param>
    /// <param name="sqlString">sql string</param>
    /// <param name="_params">command parameters</param>
    /// <returns></returns>
    public static bool ExecuteSQLForSQL(string cnString, string sqlString, List<SqlParameter> _params)
    {
        using (var cn =
            new SqlConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                    new SqlCommand(sqlString, cn))
                {
                    cm.Parameters.Clear();
                    foreach (var param in _params)
                    {
                        cm.Parameters.Add(param);
                    }
                    cm.CommandTimeout = 180;
                    cm.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                var errorMsg = ex.ToString();
                return false;
            }
        }
    }

    /// <summary>
    /// 執行SQL指令(SQL版本)
    /// </summary>
    /// <param name="cnString">connection string</param>
    /// <param name="sqlString">sql string</param>
    /// <returns></returns>
    public static bool ExecuteSQLForSQL(string cnString, string sqlString)
    {
        using (var cn =
            new SqlConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                    new SqlCommand(sqlString, cn))
                {
                    cm.CommandTimeout = 180;
                    cm.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                var errorMsg = ex.ToString();
                return false;
            }
        }
    }

    /// <summary>
    /// 執行SQL指令(OleDB版本)
    /// </summary>
    /// <param name="cnString">connection string</param>
    /// <param name="sqlString">sql string</param>
    /// <returns></returns>
    public static bool ExecuteSqlForOleDb(string cnString, string sqlString)
    {
        using (var cn =
           new OleDbConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                   new OleDbCommand(sqlString, cn))
                {
                    cm.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
                return false;
            }
            finally
            {
                cn.Dispose();
            }
        }
    }
    
    public static bool ExecuteSqlForOleDb(string cnString, string sqlString, SqlParameter[] _params)
    {
        using (var cn =
           new OleDbConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                   new OleDbCommand(sqlString, cn))
                {
                    foreach (var param in _params)
                    {
                        cm.Parameters.Add(param);
                    }
                    cm.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
                return false;
            }
            finally
            {
                cn.Dispose();
            }
        }
    }

    #region GetSingleValueForSQL

    /// <summary>
    /// 取得單一的值(或第一筆資料的值)
    /// </summary>
    /// <param name="cnString">connection string</param>
    /// <param name="sqlString">sql string</param>
    /// <returns></returns>
    public static string GetSingleValueForSQL(string cnString, string sqlString)
    {
        string result;
        using (var cn =
            new SqlConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                    new SqlCommand(sqlString, cn))
                {
                    cm.CommandTimeout = 180;
                    result = cm.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.ToString();
                result = "";
            }
        }

        return result;
    }

    /// <summary>
    /// 取得單一的值(或第一筆資料的值)
    /// </summary>
    /// <param name="cnString"></param>
    /// <param name="sqlString"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static string GetSingleValueForSQL(string cnString, string sqlString, SqlParameter parameter)
    {
        string result;

        using (var cn =
            new SqlConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                    new SqlCommand(sqlString, cn))
                {
                    cm.Parameters.Clear();
                    cm.Parameters.Add(parameter);
                    cm.CommandTimeout = 180;
                    result = cm.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.ToString();
                result = "";
            }
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cnString"></param>
    /// <param name="sqlString"></param>
    /// <param name="_params"></param>
    /// <returns></returns>
    public static string GetSingleValueForSQL(string cnString, string sqlString, SqlParameter[] _params)
    {
        string result;

        //
        using (var cn =
            new SqlConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                    new SqlCommand(sqlString, cn))
                {
                    cm.Parameters.Clear();
                    foreach (SqlParameter param in _params)
                        cm.Parameters.Add(param);
                    cm.CommandTimeout = 180;
                    result = cm.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                var errorMsg = ex.ToString();
                result = "";
            }
        }

        return result;
    }

    /// <summary>
    /// 取得單一的值(或第一筆資料的值)
    /// </summary>
    /// <param name="cnString"></param>
    /// <param name="sqlString"></param>
    /// <param name="_params"></param>
    /// <returns></returns>
    public static string GetSingleValueForSQL(string cnString, string sqlString, List<SqlParameter> _params)
    {
        string result;

        //
        using (var cn =
            new SqlConnection(cnString))
        {
            try
            {
                cn.Open();
                using (var cm =
                    new SqlCommand(sqlString, cn))
                {
                    cm.Parameters.Clear();
                    foreach (SqlParameter param in _params)
                        cm.Parameters.Add(param);
                    cm.CommandTimeout = 180;
                    result = cm.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.ToString();
                result = "";
            }
        }

        return result;
    }
    #endregion

    #region GetDataTableForSQL

    /// <summary>
    /// 取得DataTable(SQL版本)
    /// </summary>
    /// <param name="cnString">connection string</param>
    /// <param name="sqlString">sql string</param>
    /// <returns></returns>
    public static System.Data.DataTable GetDataTableForSQL(string cnString, string sqlString)
    {
        return GetDataTableForSQL(cnString, sqlString, "Table");
    }

    /// <summary>
    /// 取得DataTable(SQL版本)
    /// </summary>
    /// <param name="cnString">connection string</param>
    /// <param name="sqlString">sql string</param>
    /// <param name="tableName">table name</param>
    /// <returns></returns>
    public static System.Data.DataTable GetDataTableForSQL(string cnString, string sqlString, string tableName)
    {
        var table = new DataTable();

        using (var cn = new SqlConnection(cnString))
        {
            using (var cm = new SqlCommand(sqlString, cn))
            {
                try
                {
                    cn.Open();
                    table.Load(cm.ExecuteReader());
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
        return table;

    }

    /// <summary>
    /// get datatable by sql
    /// </summary>
    /// <param name="cnstring"></param>
    /// <param name="sqlString"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static System.Data.DataTable GetDataTableForSQL(string cnstring, string sqlString, SqlParameter parameter)
    {
        var table = new DataTable();
        using (var cn = new SqlConnection(cnstring))
        {
            using (var cm = new SqlCommand(sqlString, cn))
            {
                try
                {
                    cn.Open();
                    cm.Parameters.Add(parameter);
                    table.Load(cm.ExecuteReader());
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
        return table;
    }

    /// <summary>
    /// get datatable by sql
    /// </summary>
    /// <param name="cnstring"></param>
    /// <param name="sqlString"></param>
    /// <param name="_params"></param>
    /// <returns></returns>
    public static System.Data.DataTable GetDataTableForSQL(string cnstring, string sqlString, SqlParameter[] _params)
    {
        // 
        var table = new DataTable();
        using (var cn = new SqlConnection(cnstring))
        {
            using (var cm = new SqlCommand(sqlString, cn))
            {
                try
                {
                    cn.Open();
                    foreach (var param in _params)
                    {
                        cm.Parameters.Add(param);
                    }
                    table.Load(cm.ExecuteReader());
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
        return table;
    }

    /// <summary>
    /// GetDataTable 
    /// </summary>
    /// <param name="cnstring">cnString</param>
    /// <param name="sqlString">sqlString</param>
    /// <param name="_params">command parameters</param>        
    /// <returns></returns>
    public static System.Data.DataTable GetDataTableForSQL(string cnstring, string sqlString, List<SqlParameter> _params)
    {
        var table = new DataTable();
        using (var cn = new SqlConnection(cnstring))
        {
            using (var cm = new SqlCommand(sqlString, cn))
            {
                try
                {
                    cn.Open();
                    foreach (var param in _params)
                    {
                        cm.Parameters.Add(param);
                    }
                    table.Load(cm.ExecuteReader());

                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
        return table;
    }
    #endregion
}
