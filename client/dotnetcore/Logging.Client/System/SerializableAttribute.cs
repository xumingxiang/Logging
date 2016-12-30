using System.Reflection;

namespace System
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false)]
    [System.Runtime.InteropServices.ComVisible(true)]
    public class SerializableAttribute : Attribute
    {
        internal static Attribute GetCustomAttribute(TypeInfo type)
        {
            return (type.Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable ? new SerializableAttribute() : null;
        }

        internal static bool IsDefined(TypeInfo type)
        {
            return type.IsSerializable;
        }

        public SerializableAttribute()
        {
        }
    }
}