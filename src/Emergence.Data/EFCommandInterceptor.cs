using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace Emergence.Data
{
    public class EFCommandInterceptor : System.Data.Entity.Infrastructure.Interception.DbCommandInterceptor
    {
        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuted(command, interceptionContext);
            LogInfo("NonQueryExecuted", interceptionContext.Result.ToString(), command.CommandText);
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(command, interceptionContext);
            LogInfo("NonQueryExecuting", command.CommandType.ToString(), command.CommandText);
        }

        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuted(command, interceptionContext);
            LogInfo("ReaderExecuted", interceptionContext.Result.ToString(), command.CommandText);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(command, interceptionContext);
            LogInfo("ReaderExecuting", command.CommandType.ToString(), command.CommandText);
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuted(command, interceptionContext);
            LogInfo("ScalarExecuted", interceptionContext.Result.ToString(), command.CommandText);
        }

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(command, interceptionContext);
            LogInfo("ScalarExecuting", command.CommandType.ToString(), command.CommandText);
        }

        private void LogInfo(string method, string command, string commandText) =>
            Console.WriteLine("Intercepted on: {0} \n {1} \n {2}", method, command, commandText);

        private void LogInfo(string method, string command, string commandText, string exception) =>
            Console.WriteLine("Intercepted on: {0} \n {1} \n {2} \n {3}", method, command, commandText, exception);
    }
}
