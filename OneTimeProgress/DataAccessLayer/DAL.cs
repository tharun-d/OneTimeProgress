using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneTimeProgress.DataAccessLayer
{
    public class DAL
    {
        const string Completed = "Completed";

        public static DataSet ExecuteDataset(string connectionString, CommandType sqlCommandType, string storedPrcedure, SqlParameter[] sqlParams)
        {
            DataSet ds = new DataSet();
            SqlConnection sqlConnection = new SqlConnection();

            try
            {
                sqlConnection = new SqlConnection(connectionString);

                ds = ExecuteDataset(sqlConnection, sqlCommandType, storedPrcedure, sqlParams);
            }
            catch (Exception exp)
            {
                //Logging log = new Logging();
                //if (Convert.ToBoolean(ConfigurationManager.AppSettings["WriteToErrorLog"]))
                //{
                //    log.WriteError("Error while executing the stored procedure: " + storedPrcedure, exp);
                //}
                //throw;
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
            }

            return ds;
        }

        private static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            using (SqlCommand cmd = new SqlCommand())
            {
                return PerformExecuteDataSetOperation(connection, commandType, commandText, commandParameters, cmd, 3);
            }
        }

        private static DataSet PerformExecuteDataSetOperation(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlCommand cmd, int retryCount)
        {

            if (retryCount > 0)
            {
                try
                {
                    bool mustCloseConnection = false;
                    PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

                    // Create the DataAdapter & DataSet
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();

                        // Fill the DataSet using default values for DataTable names, etc
                        da.Fill(ds);

                        // Detach the SqlParameters from the command object, so they can be used again
                        cmd.Parameters.Clear();

                        if (mustCloseConnection)
                            connection.Close();

                        // Return the dataset
                        return ds;
                    }
                }
                catch (SqlException sqlExp)
                {
                    cmd.Parameters.Clear();
                    //Logging.WriteToLogFile("Error Occured at Execute dataset, retrying ", sqlExp);
                    return PerformExecuteDataSetOperation(connection, commandType, commandText, commandParameters, cmd, --retryCount);
                }
            }
            else
            {
                throw new Exception("DataBase didnt respond even after multiple tries");
            }
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            try
            {
                if (connection.ServerVersion.Length == 0)
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        public static void ExecuteScalor(string connectionString, CommandType sqlCommandType, string storedPrcedure, SqlParameter[] sqlParams)
        {
            SqlConnection sqlConnection = new SqlConnection();
            try
            {
                sqlConnection = new SqlConnection(connectionString);

                ExecuteScalor(sqlConnection, sqlCommandType, storedPrcedure, sqlParams);
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private static void ExecuteScalor(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        { 
            using (SqlCommand cmd = new SqlCommand())
            {
                PerformDBOperation(connection, commandType, commandText, commandParameters, cmd, 3);
            }
        }

        private static void PerformDBOperation(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlCommand cmd, int retryCount)
        {
            if (retryCount > 0)
            {
                try
                {
                    bool mustCloseConnection = false;
                    PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();

                    if (mustCloseConnection)
                        connection.Close();
                }
                catch (SqlException sqlExp)
                {
                    cmd.Parameters.Clear();
                    //Logging.WriteToLogFile("Error Occured at Execute dataset, retrying ", sqlExp);
                    PerformDBOperation(connection, commandType, commandText, commandParameters, cmd, --retryCount);
                }
            }
            else
            {
                throw new Exception("SQL Database didnt repond even after multiple tries");
            }
        }
    }
}