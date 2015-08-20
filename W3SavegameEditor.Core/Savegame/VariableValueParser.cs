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
            public string Name { get; set; }
            public string CName { get; set; }
            public string CType { get; set; }
            public bool IsArray { get; set; }
            public Type ArrayElementType { get; set; }
            public PropertyInfo Info { get; set; }
        }

        private readonly Dictionary<string, Func<Stack<Variable>, object>> _factories;

        public VariableValueParser()
        {
            _factories = new Dictionary<string, Func<Stack<Variable>, object>>();

            foreach (var serializableType in GetSerializableTypes())
            {
                var type = serializableType;
                var deserializationFunction = CreateDeserializationFunction(type.Key, type.Value);
                _factories.Add(serializableType.Key, deserializationFunction);
            }
        }

        private Func<Stack<Variable>, object> CreateDeserializationFunction(string typeName, Type type)
        {
            return (v) =>
            {
                var instance1 = Activator.CreateInstance(type);

                if (v.Peek().Name == typeName)
                {
                    v.Pop();
                }

                foreach (var serializableProperty1 in GetNamedProperties(type))
                {
                    object propertyValue1 = null;
                    if (serializableProperty1.CType != null)
                    {
                        propertyValue1 = _factories[serializableProperty1.CType](v);
                    }
                    else
                    {
                        var variable1 = v.Pop();
                        var typedVariable1 = (TypedVariable) variable1;
                        if (serializableProperty1.IsArray && variable1.Name == "count")
                        {
                            uint count1 = (uint) typedVariable1.Value.Object;
                            var array1 = Array.CreateInstance(serializableProperty1.ArrayElementType, count1);
                            for (int i1 = 0; i1 < count1; i1++)
                            {
                                // HACK: Refactor this so that no new function has to be generated.
                                var element1 = CreateDeserializationFunction("", serializableProperty1.ArrayElementType)(v);
                                array1.SetValue(element1, i1);
                            }
                            propertyValue1 = array1;

                        }
                        else if (serializableProperty1.CName == variable1.Name)
                        {
                            if (typedVariable1 != null)
                            {
                                propertyValue1 = typedVariable1.Value.Object;
                            }
                        }
                    }


                    serializableProperty1.Info.SetValue(instance1, propertyValue1);
                }

                return instance1;
            };
        }

        public object Parse(string type, Stack<Variable> variables)
        {
            return _factories[type](variables);
        }

        private static IEnumerable<VariablePropertyInfo> GetNamedProperties(Type serializableType)
        {
            // TODO: Order
            foreach (var property in serializableType.GetProperties())
            {
                var cNameAttribute = property.GetCustomAttributes(typeof(CNameAttribute), true).SingleOrDefault() as CNameAttribute;
                var cArrayAttribute = property.GetCustomAttributes(typeof(CArrayAttribute), true).SingleOrDefault() as CArrayAttribute;
                var cSerializableAttribute = GetCSerializableAttribute(property.PropertyType);
                if (cNameAttribute != null || cArrayAttribute != null)
                {
                    yield return new VariablePropertyInfo
                    {
                        Name = property.Name,
                        CName = cNameAttribute != null ? cNameAttribute.Name : null,
                        CType = cSerializableAttribute != null ? cSerializableAttribute.Type : null,
                        IsArray = cArrayAttribute != null,
                        ArrayElementType = cArrayAttribute != null ? property.PropertyType.GetElementType() : null,
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
            CSerializableAttribute cSerializableAttribute =
                type.GetCustomAttributes(typeof(CSerializableAttribute), true).SingleOrDefault() as CSerializableAttribute;
            return cSerializableAttribute;
        }
    }
}
