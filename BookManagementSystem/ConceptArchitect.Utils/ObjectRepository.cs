using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.Utils
{
    public class ObjectRepository<T>
    {
        public void CreateTable()
        {
            var type = typeof(T);
            //var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            var tableAttribute = typeof(T).GetTableInfo();

            string tableName = tableAttribute.Name;
            string primaryKey = tableAttribute.PrimaryKey;

            var query = new StringBuilder();

            query.Append($"CREATE TABLE {tableName} (");

            foreach(var property in type.GetProperties())
            {
                var column = property.GetColumnInfo();
                if (column.Exclude)
                    continue; //ingore and move to next
                string nullType = column.AllowNull ? "" : "NOT NULL";
                query.Append($"{column.Name} {column.DataType} {nullType}, ");
            }

            query.Append($"PRIMARY KEY({primaryKey})");

            query.Append(");");

            var qry=query.ToString();

            Console.WriteLine(qry);

        }


        public void Add(T obj)
        {

        }

        public List<T> GetAll()
        {
            return new List<T>();
        }

        public T GetById(object id)
        {
            var tableAttribute = typeof(T).GetTableInfo();

            var qry = $"select * from {tableAttribute.Name} where {tableAttribute.PrimaryKey}={id}";

            return default(T);
        }

        public void Remove(object id)
        {

        }

        public void Update(object id, T obj)
        {
            
        }

        public List<T> Search( string whereClause)
        {
            return new List<T>();
        }

    }
}
