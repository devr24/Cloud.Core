namespace Cloud.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contract for a Named Instance implementation.  Enables support for the Named Instance Factory.
    /// </summary>
    public interface INamedInstance
    {
        string Name { get; set; }
    }

    /// <summary>
    /// Factory for getting instances of type T (where T is constrained to INamedInstance type).
    /// </summary>
    /// <typeparam name="T">Type T of a class that implemented INamedInstance.</typeparam>
    public class NamedInstanceFactory<T>
        where T : INamedInstance
    {
        public T this[string name]
        {
            get
            {
                if (Clients.TryGetValue(name, out var client))
                    return client;

                // If the client was not found, then throw exception, as this is not expected behaviour.
                throw new ArgumentException(nameof(name));
            }
        }

        public bool TryGetValue(string name, out T value)
        {
            if (Clients.TryGetValue(name, out var returnValue))
            {
                value = returnValue;
                return true;
            }

            value = default;
            return false;
        }

        internal readonly IDictionary<string, T> Clients = new Dictionary<string, T>();

        public NamedInstanceFactory(IEnumerable<T> clients)
        {
            foreach (var client in clients)
            {
                var name = client.Name;

                // Default the name if it was not set.
                if (name.IsNullOrEmpty())
                {
                    name = client.GetType().Name;
                }

                // Autogenerate new name (rather than erroring at the moment - might change later) if it already exists.
                if (Clients.ContainsKey(name))
                {
                    var generatedName = GenerateName(name);
                    Clients.Add(generatedName, client);
                }
                else
                    Clients.Add(name, client);
            }
        }

        private string GenerateName(string suggestedName, int increment = 0)
        {
            increment++;
            var returnName = suggestedName + increment;

            if (Clients.ContainsKey(returnName))
                return GenerateName(suggestedName, increment);

            return returnName;
        }
    }
}
