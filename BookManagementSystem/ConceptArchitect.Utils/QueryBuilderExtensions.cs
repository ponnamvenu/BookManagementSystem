using System.Reflection;

namespace ConceptArchitect.Utils
{
    public static class QueryBuilderExtensions
    {
        public static ColumnAttribute GetColumnInfo(this PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<ColumnAttribute>();
            if (attribute == null)
                return new ColumnAttribute()
                {
                    Name = property.Name,
                    AllowNull = false,
                    DataType = property.PropertyType == typeof(string) ? "varchar(200)" : property.PropertyType.Name.ToString()
                };

            //attribute exists. let's us put defaults
            if (string.IsNullOrEmpty(attribute.Name))
                attribute.Name = property.Name;
            if (string.IsNullOrEmpty(attribute.DataType))
                attribute.DataType = property.PropertyType == typeof(string) ? "varchar(200)" : property.PropertyType.Name.ToString();

            return attribute;
        }

        public static TableAttribute GetTableInfo(this Type type)
        {


            string tableName = $"{type.Name}Table";
            string primaryKey = "Id";

            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
            {
                tableAttribute = new TableAttribute();    
            }

            if (string.IsNullOrEmpty(tableAttribute.Name))
                tableAttribute.Name = $"{type.Name}Table";
            if (string.IsNullOrEmpty(tableAttribute.PrimaryKey))
                tableAttribute.PrimaryKey = "Id";

            return tableAttribute;
            
        }
    }
}
