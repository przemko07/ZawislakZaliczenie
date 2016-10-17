using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public struct Individual
    : IEquatable<Individual>
    , IEnumerable<uint>
    {
        public static Individual IndividualOfLength(uint length)
        {
            return new Individual(new uint[length]);
        }

        private uint[] nodes;


        public Individual(params uint[] nodes)
        {
            this.nodes = nodes;
        }


        public uint Length
        {
            get { return (uint)nodes.Length; }
        }

        public uint this[uint index]
        {
            get { return nodes[index]; }
            set { nodes[index] = value; }
        }


        #region equality

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return Equals((Individual)obj);
        }

        public override int GetHashCode()
        {
            return nodes.GetHashCode();
        }

        public bool Equals(Individual other)
        {
            if (this.Length != other.Length) return false;
            return nodes.SequenceEqual(other.nodes);
        }

        public static bool operator ==(Individual i1, Individual i2)
        {
            return i1.Equals(i2);
        }

        public static bool operator !=(Individual i1, Individual i2)
        {
            return !i1.Equals(i2);
        }

        #endregion


        #region IEnumerable<uint>

        public IEnumerator<uint> GetEnumerator()
        {
            foreach (var node in nodes)
            {
                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        #endregion


        public override string ToString()
        {
            return string.Join(", ", nodes);
        }
    }
}
