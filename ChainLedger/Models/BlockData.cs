using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Models
{
    internal class BlockData<T>
    {
        public T Value { get; private set; }

        private BlockData(T value)
        {
            Value = value;
        }

        internal string? GetStringValue()
        {
            return Value?.ToString();
        }

        internal class Factory
        {
            internal static BlockData<T> CreateData(T value, int index)
            {
                if (index !=0 && value == null)
                {
                    throw new ArgumentException("Value cannot be null", nameof(value));
                }

                if (typeof(T) != typeof(string) && (typeof(T).IsClass))
                {
                    // Ensure that ToString() is overridden
                    var toStringMethod = typeof(T).GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
                    if (toStringMethod == null || toStringMethod.DeclaringType == typeof(object))
                    {
                        throw new InvalidOperationException("The type must override the ToString() method.");
                    }
                }

                // Create and return the Data instance
                return new BlockData<T>(value);
            }
        }
    }
}
