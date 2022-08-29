using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.Utils
{
    public static class ObjectMapper
    {
        public static T To<T>(this DbDataReader reader)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));

            foreach (var property in typeof(T).GetProperties())
            {
                try
                {
                    if (property.CanWrite)
                    {
                        //Find  a key in reader which matches property name
                        var readerValue = reader[property.Name];
                        //Covert value we got from reader to match property type
                        var value = Convert.ChangeType(readerValue, property.PropertyType);
                        //assign the value we got from reader to my object
                        property.SetValue(obj, value, null);
                    }

                }
                catch (Exception ex)
                {
                    //ignore properties that can't be converted.
                }
            }

            return obj;
        }


        public static Func<DbDataReader, T> To<T>(Action<T, DbDataReader> extraInitlization)
        {
            Func<DbDataReader, T> mapper = (r) =>
            {
                T obj = r.To<T>();
                extraInitlization(obj, r);

                return obj;
            };

            return mapper;

        }


        public static Target To<Src, Target>(this Src obj, Action<Target, Src> additionalIntialization = null)
        {
            var target = (Target)Activator.CreateInstance(typeof(Target));
            target.Copy<Src, Target>(obj, additionalIntialization);
            return target;
        }

        public static T Copy<S, T>(this T target, S source, Action<T, S> extraInitialization = null)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            foreach (var property in targetType.GetProperties())
            {
                try
                {
                    var srcValue = sourceType.GetProperty(property.Name).GetValue(source);
                    property.SetValue(target, srcValue);
                }
                catch
                {
                    //ignore the errors
                }
            }

            if (extraInitialization != null)
            {
                extraInitialization(target, source);
            }

            return target;
        }



    }
}