using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using W3SavegameEditor.Core.Savegame.Attributes;
using W3SavegameEditor.Core.Savegame.Variables;

namespace W3SavegameEditor.Core.Savegame
{
    public class VariableValueParser
    {
        private class VariablePropertyInfo
        {
            public string CName { get; set; }
            public string CType { get; set; }

            public bool IsArray { get; set; }
            public string ArrayCountName { get; set; }
            public Type ArrayElementType { get; set; }
            public string ArrayElementCType { get; set; }

            public PropertyInfo Info { get; set; }
        }

        private readonly Dictionary<string, Func<Stack<Variable>, object>> _factories;

        public VariableValueParser()
        {
            _factories = new Dictionary<string, Func<Stack<Variable>, object>>();

            foreach (var serializableType in GetSerializableTypes())
            {
                var type = serializableType;
                Func<Stack<Variable>, object> deserializationFunction = (v) => Deserialize(type.Key, type.Value, v);
                _factories.Add(serializableType.Key, deserializationFunction);
            }
        }

        private object Deserialize(string typeName, Type type, Stack<Variable> v)
        {
            var instance = Activator.CreateInstance(type);

            if (v.Peek().Name == typeName)
            {
                v.Pop();
            }

            foreach (var serializableProperty in GetNamedOrArrayProperties(type))
            {
                object propertyValue = null;
                if (serializableProperty.CType != null)
                {
                    propertyValue = _factories[serializableProperty.CType](v);
                }
                else
                {
                    var typedVariable = v.Peek() as TypedVariable;
                    if(typedVariable == null)
                    {
                        continue;
                    }

                    typedVariable = (TypedVariable)v.Pop();

                    if (serializableProperty.IsArray && typedVariable.Name == serializableProperty.ArrayCountName)
                    {
                        uint count = (uint) typedVariable.Value.Object;
                        var array = Array.CreateInstance(serializableProperty.ArrayElementType, count);
                        for (int i = 0; i < count; i++)
                        {
                            var element = Deserialize(serializableProperty.ArrayElementCType, serializableProperty.ArrayElementType, v);
                            array.SetValue(element, i);
                        }
                        propertyValue = array;
                    }
                    else if (typedVariable.Name == serializableProperty.CName)
                    {
                        propertyValue = typedVariable.Value.Object;
                    }
                }


                serializableProperty.Info.SetValue(instance, propertyValue);
            }
            return instance;
        }
        
        public object Parse(string type, Stack<Variable> variables)
        {
            return _factories[type](variables);
        }

        private static IEnumerable<VariablePropertyInfo> GetNamedOrArrayProperties(Type serializableType)
        {
            // TODO: Add an order attribute
            foreach (var property in serializableType.GetProperties())
            {
                var cNameAttribute = property.GetCustomAttributes(typeof(CNameAttribute), true).SingleOrDefault() as CNameAttribute;
                var cArrayAttribute = property.GetCustomAttributes(typeof(CArrayAttribute), true).SingleOrDefault() as CArrayAttribute;
                var cArrayElementType = cArrayAttribute != null ? GetCSerializableAttribute(property.PropertyType.GetElementType()) : null;
                var cSerializableAttribute = GetCSerializableAttribute(property.PropertyType);
                if (cNameAttribute != null || cArrayAttribute != null)
                {
                    yield return new VariablePropertyInfo
                    {
                        CName = cNameAttribute != null ? cNameAttribute.Name : null,
                        CType = cSerializableAttribute != null ? cSerializableAttribute.Type : null,
                        IsArray = cArrayAttribute != null,
                        ArrayCountName = cArrayAttribute != null ? cArrayAttribute.CountName : null,
                        ArrayElementType = cArrayAttribute != null ? property.PropertyType.GetElementType() : null,
                        ArrayElementCType = cArrayElementType != null ? cArrayElementType.Type : null,
                        Info = property
                    };
                }
            }
        }

        private static IEnumerable<KeyValuePair<string, Type>> GetSerializableTypes()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var cSerializableAttribute = GetCSerializableAttribute(type);
                if (cSerializableAttribute != null)
                {
                    yield return new KeyValuePair<string, Type>(cSerializableAttribute.Type, type);
                }
            }
        }

        private static CSerializableAttribute GetCSerializableAttribute(Type type)
        {
            return type.GetCustomAttributes(typeof(CSerializableAttribute), true).SingleOrDefault() as CSerializableAttribute;
        }
    }
}
