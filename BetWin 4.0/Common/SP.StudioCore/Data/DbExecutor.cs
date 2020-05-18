using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Dapper.SqlMapper;
using MySql.Data.MySqlClient;

namespace SP.StudioCore.Data
{
    /// <summary>
    /// 数据库操作对象
    /// </summary>
    public class DbExecutor : IDisposable
    {
        private readonly IDbConnection conn;

        private readonly IDbTransaction tran;

        private List<Action> callback;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public readonly DatabaseType DBType;

        /// <summary>
        /// commit之後要執行的回調方法
        /// </summary>
        /// <param name="action"></param>
        public void AddCallback(Action action)
        {
            if (this.callback == null) this.callback = new List<Action>();
            this.callback.Add(action);
        }

        /// <summary>
        /// 创建一个数据库链接对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private IDbConnection CreateDbConnection(string connectionString)
        {
            IDbConnection conn = null;
            switch (this.DBType)
            {
                case DatabaseType.SqlServer:
                    conn = new SqlConnection(connectionString);
                    break;
                case DatabaseType.MySql:
                    conn = new MySqlConnection(connectionString);
                    break;
                case DatabaseType.SQLite:

                    break;
            }
            return conn;
        }

        /// <summary>
        /// 构造函数 赋值
        /// </summary>
        /// <param name="connectionString">链接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="isTran">是否启用事务</param>
        public DbExecutor(string connectionString, DatabaseType dbType = DatabaseType.SqlServer, DataConnectionMode connMode = DataConnectionMode.None, IsolationLevel tranLevel = IsolationLevel.Unspecified)
        {
            this.DBType = dbType;
            conn = this.CreateDbConnection(connectionString);

            if (tranLevel != IsolationLevel.Unspecified)
            {
                conn.Open();
                tran = conn.BeginTransaction(tranLevel);
            }
        }

        public int ExecuteNonQuery(string commandText)
        {
            return this.ExecuteNonQuery(CommandType.Text, commandText);
        }

        public int ExecuteNonQuery(string procedureName, object parameters = null)
        {
            return this.ExecuteNonQuery(CommandType.StoredProcedure, procedureName, parameters);
        }

        public int ExecuteNonQuery(CommandType cmdType, string cmdText, object parameters = null)
        {
            try
            {
                return this.conn.Execute(cmdText, parameters, this.tran, null, cmdType);
            }
            catch (Exception ex)
            {
                throw new Exception(this.ThrowException(ex, cmdText, parameters));
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(CommandType.Text, commandText);
        }

        public object ExecuteScalar(string procedureName, object parameters = null)
        {
            return this.ExecuteScalar(CommandType.StoredProcedure, procedureName, parameters);
        }

        public object ExecuteScalar(CommandType cmdType, string cmdText, object parameters = null)
        {
            try
            {
                return this.conn.ExecuteScalar(cmdText, parameters, this.tran, null, cmdType);
            }
            catch (Exception ex)
            {
                throw new Exception(this.ThrowException(ex, cmdText, parameters));
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public DataSet GetDataSet(string commandText)
        {
            return this.GetDataSet(CommandType.Text, commandText);
        }

        public DataSet GetDataSet(string procedureName, params DbParameter[] parameter)
        {
            return this.GetDataSet(CommandType.StoredProcedure, procedureName, parameter);
        }

        public DataSet GetDataSet(CommandType cmdType, string cmdText, params DbParameter[] parameter)
        {
            IDbCommand comm = this.conn.CreateCommand();
            comm.CommandType = cmdType;
            comm.CommandText = cmdText;
            foreach (DbParameter param in parameter)
            {
                comm.Parameters.Add(param);
            }
            DbDataAdapter adapter = new SqlDataAdapter();

            if (tran != null) comm.Transaction = tran;
            try
            {
                adapter.SelectCommand = (SqlCommand)comm;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(this.ThrowException(ex, cmdText, parameter));
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataReader ReadData(CommandType cmdType, string cmdText, object parameters = null)
        {
            try
            {
                return this.conn.ExecuteReader(cmdText, parameters, this.tran, null, cmdType);
            }
            catch (Exception ex)
            {
                throw new Exception(this.ThrowException(ex, cmdText, parameters));
            }
            finally
            {
                this.CloseConnection();
            }
        }


        public void Commit()
        {
            if (tran == null)
                throw new Exception("未开启事务");
            //if (this.connectionMode == DataConnectionMode.None)
            //    throw new Exception("如果要使用事务则必须使用数据的长连接");
            tran.Commit();

            if (callback != null)
            {
                foreach (Action action in this.callback)
                {
                    action.Invoke();
                }
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (tran == null)
                throw new Exception("未开启事务");
            //if (this.connectionMode == DataConnectionMode.None)
            //    throw new Exception("如果要使用事务则必须使用数据的长连接");
            tran.Rollback();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private void CloseConnection()
        {

        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        private string ThrowException(Exception ex, string cmdText, object param)
        {
            StringBuilder sb = new StringBuilder(ex.Message);
            sb.AppendLine(cmdText);
            if (param != null)
            {
                if (param.GetType() == typeof(DynamicParameters))
                {
                    DynamicParameters dynamicParameters = (DynamicParameters)param;

                    foreach (string paramName in dynamicParameters.ParameterNames)
                    {
                        sb.AppendFormat("{0} : {1} \n", paramName, dynamicParameters.Get<Object>(paramName));
                    }
                }
                else
                {
                    foreach (PropertyInfo property in param.GetType().GetProperties())
                    {
                        sb.AppendFormat("{0} : {1} \n", property.Name, property.GetValue(param));
                    }
                }
            }
            return sb.ToString();
        }

        private string ThrowException(Exception ex, string cmdText, params DbParameter[] parameters)
        {
            StringBuilder sb = new StringBuilder(ex.Message);
            sb.AppendLine(cmdText);
            foreach (DbParameter param in parameters)
            {
                sb.AppendFormat("{0} : {1} \n", param.ParameterName, param.Value);
            }
            return sb.ToString();
        }

        public DbParameter NewParam(string parameterName, object value)
        {
            DbType dbType;
            switch (value.GetType().Name)
            {
                case "DateTime":
                    dbType = DbType.DateTime;
                    break;
                case "Boolean":
                    dbType = DbType.Boolean;
                    break;
                case "Int32":
                    dbType = DbType.Int32;
                    break;
                case "Int16":
                    dbType = DbType.Int16;
                    break;
                case "Decimal":
                    dbType = DbType.Decimal;
                    break;
                case "Byte":
                    dbType = DbType.Byte;
                    break;
                case "Guid":
                    dbType = DbType.Guid;
                    break;
                default:
                    dbType = DbType.String;
                    break;
            }
            return this.NewParam(parameterName, value, dbType, 0, ParameterDirection.Input);
        }

        public DbParameter NewParam(string parameterName, object value, DbType dbType, int size, ParameterDirection direction)
        {
            DbParameter param = new SqlParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            param.DbType = dbType;
            param.Size = size;
            param.Direction = direction;
            return param;
        }

        public void Dispose()
        {
            if (this.tran != null) tran.Dispose();

            if (this.conn.State == ConnectionState.Open)
            {
                this.conn.Close();
            }
        }
    }
}
