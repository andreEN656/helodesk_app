using DbMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbMapper.Mapping
{
    public static class AttributeHelper
    {
        public static List<string> GetDataNames(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName).GetCustomAttributes(false).Where(x => x.GetType().Name == "DataNamesAttribute" || x.GetType().Name == "OutputDataNamesAttribute").FirstOrDefault();

            if (property != null && property.GetType() == typeof(DataNamesAttribute))
            {
                return ((DataNamesAttribute)property).ValueNames;
            }
            else if (property != null && property.GetType() == typeof(OutputDataNamesAttribute))
            {
                return ((OutputDataNamesAttribute)property).ValueNames;
            }
            return new List<string>();
        }
    }
}
