using System.Data.Common;

namespace ConceptArchitect.Utils
{
    public class DbRepository
    {
        public Func<DbConnection> ConnectionProvider { get; }
        public DbRepository(Func<DbConnection> connectionProvider)
        {
            ConnectionProvider = connectionProvider;
        }

        

        public T ExecuteCommand<T>(Func<DbCommand,T> commandExecutor)
        {
            DbConnection connection = ConnectionProvider();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                //This is where different function will perform different
                //Action on the DbCommand (execute query)

                return commandExecutor(command);    
                //-------------------------------------------------------------------------------------------
                  

            }
            catch (Exception ex)
            {
                //My code may want to handle or translate some exception

                throw;  //throw the same exception you caught
            }
            finally
            {
                connection.Close();
            }
        }
    
    
        public List<T> Query<T>( string query, Func<DbDataReader,T> converter)
        {
            return ExecuteCommand(command =>
               {
                   command.CommandText = query;
                   var reader= command.ExecuteReader();

                   var result = new List<T>();
                   
                   while(reader.Read())
                   {
                       var entity = converter(reader);
                       result.Add(entity);
                   }

                   return result;
               });
        }
    

        public T QueryForOne<T>(string query, Func<DbDataReader,T> converter)
        {
            return ExecuteCommand(command =>
            {
                command.CommandText = query;
                var reader = command.ExecuteReader();

                

                if (reader.Read())
                {
                    var entity = converter(reader);
                    return entity;
                }

                return default(T);
            });
        }

        public T QueryOne<T>(string qry, Func<DbDataReader,T> converter)
        {
            return Query(qry, converter).FirstOrDefault();
        }
    }
}