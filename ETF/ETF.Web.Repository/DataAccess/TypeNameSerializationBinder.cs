using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace ETF.Web.Repository.DataAccess
{
    public class TypeNameSerializationBinder : SerializationBinder
    {
        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            if (assembly == null)
            {
                return Type.GetType(typeName, true);
            }
            return assembly.GetType(typeName, true);
        }
    }
}
