using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DbMapper.Mapping
{
    public static class PropertyMapHelper
    {

        public static void Map(Type type, DataRow row, PropertyInfo prop, object entity)
        {
            List<string> columnNames = AttributeHelper.GetDataNames(type, prop.Name);

            foreach (var columnName in columnNames)
            {
                if (!String.IsNullOrWhiteSpace(columnName) && row.Table.Columns.Contains(columnName))
                {
                    var propertyValue = row[columnName];
                    if (propertyValue != DBNull.Value)
                    {
                        ParsePrimitive(prop, entity, row[columnName]);
                        break;
                    }
                }
            }
        }

        //public static void Map(Type type, SqlCommand cmd, PropertyInfo prop, object entity)
        //{
        //    List<string> parameterNames = AttributeHelper.GetDataNames(type, prop.Name);

        //    foreach (var parameterName in parameterNames)
        //    {
        //        if (!String.IsNullOrWhiteSpace(parameterName) && cmd.Parameters.Contains(parameterName))
        //        {
        //            var propertyValue = cmd.Parameters[parameterName].Value;
        //            if (propertyValue != DBNull.Value)
        //            {
        //                ParsePrimitive(prop, entity, propertyValue);
        //                break;
        //            }
        //        }
        //    }
        //}

        private static void ParsePrimitive(PropertyInfo prop, object entity, object value)
        {
            //string
            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(entity, Regex.Replace(value.ToString().Trim(), "(<|>)", ""), null);
            }

            //bool || bool?
            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, ParseBoolean(value.ToString()), null);
                }
            }

            //long || long?
            else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, long.Parse(value.ToString()), null);
                }
            }

            //int || int?
            else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, int.Parse(value.ToString()), null);
                }
            }

            //decimal || decimal?
            else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
            {
                decimal number;
                bool isValid = decimal.TryParse(value.ToString(), out number);
                if (isValid)
                {
                    prop.SetValue(entity, decimal.Parse(value.ToString()), null);
                }

                prop.SetValue(entity, decimal.Parse(value.ToString()), null);
            }

            //double || double?
            else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
            {
                double number;
                bool isValid = double.TryParse(value.ToString(), out number);
                if (isValid)
                {
                    prop.SetValue(entity, double.Parse(value.ToString()), null);
                }
            }

            //DateTime || DateTime?
            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            {
                DateTime date;
                bool isValid = DateTime.TryParse(value.ToString(), out date);
                if (isValid)
                {
                    prop.SetValue(entity, date, null);
                }
                else
                {
                    isValid = DateTime.TryParseExact(value.ToString(), "MMddyyyy", new CultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out date);
                    if (isValid)
                    {
                        prop.SetValue(entity, date, null);
                    }
                }
            }

            //GUID
            else if (prop.PropertyType == typeof(Guid))
            {
                Guid guid;
                bool isValid = Guid.TryParse(value.ToString(), out guid);
                if (isValid)
                {
                    prop.SetValue(entity, guid, null);
                }
                else
                {
                    isValid = Guid.TryParseExact(value.ToString(), "B", out guid);
                    if (isValid)
                    {
                        prop.SetValue(entity, guid, null);
                    }
                }
            }

            //byte[] || byte?[]
            else if (prop.PropertyType == typeof(byte[]) || prop.PropertyType == typeof(byte?[]))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, value, null);
                }
            }
        }

        public static bool ParseBoolean(object value)
        {
            if (value == null || value == DBNull.Value) return false;

            switch (value.ToString().ToLowerInvariant())
            {
                case "1":
                case "y":
                case "yes":
                case "true":
                    return true;

                case "0":
                case "n":
                case "no":
                case "false":
                default:
                    return false;
            }
        }
    }
}
