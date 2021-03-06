﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace DAL
{
    public class SQLHelper
    {

        private static readonly string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();


        #region database connection without parameter
        public static int Update(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql,conn);

            try
            {
                conn.Open();

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        public static object GetSingleResult(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static SqlDataReader GetReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        #endregion

        #region data base connection with parameter
        public static int Update(string sql, SqlParameter[] param){

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql,conn);

            if (param != null)
            {

                cmd.Parameters.AddRange(param);
            }

              try 
	            {	   
                  conn.Open();
                  return cmd.ExecuteNonQuery();
		
	            }
	            catch (Exception ex)
	            {
		
		            throw ex;
                }
              finally
              {
                  conn.Close();
              }
        }

        public static object GetSingleResult(string sql, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }

            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                conn.Close();
            }


        }

        public static SqlDataReader GetReader(string sql, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }

            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }



        #endregion

        /// <summary>
        /// get server time
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerTime()
        {
            return Convert.ToDateTime(SQLHelper.GetSingleResult("select getdate()"));
        }

        public static bool UpdateByTransaction(List<string> sqlList)
        {
            SqlConnection conn = new SqlConnection(connString);

            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;

            try
            {
                conn.Open();

                cmd.Transaction = conn.BeginTransaction();

              


                foreach (var itemsql in sqlList)
                {
                    cmd.CommandText = itemsql;
                    cmd.ExecuteNonQuery();
                }

                cmd.Transaction.Commit();

                return true;

            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();

                }
                throw new Exception("update by transaction"+ex.Message);
            }
            finally
            {

                if (cmd.Transaction != null)
                {
                    cmd.Transaction = null;
                }

                conn.Close();
            }
        }
    }
}
