using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoctisDev.IdentifierSystem
{
    [Serializable]
    public struct Identifier : IComparable<Identifier>, IEquatable<Identifier>
    {
        [SerializeField] private int _value;

        public Identifier(int value)
        {
            _value = value;
        }

        public int Value
        {
            get => _value;
            set => _value = value;
        }

        public bool Equals(Identifier other) => 
            other._value == _value;

        public override bool Equals(object obj)
        {
            if (obj is Identifier other)
                return _value == other._value;
            return false;
        }

        public override int GetHashCode() => 
            _value.GetHashCode();

        public int CompareTo(Identifier other) => 
            _value.CompareTo(other._value);

        public override string ToString() => _value.ToString();

        public static bool operator ==(Identifier left, Identifier right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Identifier left, Identifier right)
        {
            return !left.Equals(right);
        }
    }
}