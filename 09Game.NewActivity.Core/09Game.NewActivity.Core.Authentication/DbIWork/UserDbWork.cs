using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace _09Game.NewActivity.Core.Authentication.DbIWork
{
    public class UserDbWork
    {
        public UserDbWork(string configuration)
        {
            Connection = new MySqlConnection(configuration);
        }

        /// <summary>
        /// 连接对象
        /// </summary>
        public IDbConnection Connection { get; }

        /// <summary>
        /// 事务
        /// </summary>
        public IDbTransaction Transaction { get; private set; }

        public void BeginTransaction()
        {
            Connection.Open();
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction?.Rollback();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection.Dispose();
        }
    }
}
