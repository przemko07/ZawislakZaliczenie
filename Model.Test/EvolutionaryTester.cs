using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Diagnostics;

namespace Model.Test
{
    [TestClass]
    public class EvolutionaryTester
    {
        [TestMethod]
        public void CrossTest1()
        {
            var individual1 = new Individual(0, 1, 2, 3, 4, 5, 6);
            var individual2 = new Individual(4, 3, 5, 6, 1, 2, 0);
            var co = new CrossOverOX();
            co.Cross(individual1, individual2);
            CollectionAssert.AreEqual(new uint[] { 0, 1, 5, 3, 4, 2, 6 }, co.Offspring1.ToArray());
            CollectionAssert.AreEqual(new uint[] { 4, 3, 2, 6, 1, 5, 0 }, co.Offspring2.ToArray());
        }

        [TestMethod]
        public void CrossTest2()
        {
            var individual1 = new Individual(9, 8, 5, 4, 2, 6, 7, 0, 3, 1);
            var individual2 = new Individual(9, 4, 2, 6, 3, 0, 7, 1, 5, 8);
            for (uint i = 0; i < individual1.Length; i++)
            {
                Debug.Write(individual1[i] - 1);
                Debug.Write(", ");
            }

            Debug.WriteLine(string.Empty);
            for (uint i = 0; i < individual2.Length; i++)
            {
                Debug.Write(individual2[i] - 1);
                Debug.Write(", ");
            }
            Debug.WriteLine(string.Empty);

            var co = new CrossOverOX();
            co.Cross(individual1, individual2);
            CollectionAssert.AreEqual(new uint[] { 9, 8, 2, 4, 3, 6, 7, 0, 5, 1 }, co.Offspring1.ToArray());
            CollectionAssert.AreEqual(new uint[] { 9, 4, 5, 6, 2, 0, 7, 1, 3, 8 }, co.Offspring2.ToArray());
        }

        [TestMethod]
        public void CrossTest3()
        {
            var individual1 = new Individual(0, 1, 2, 3, 4, 5, 6);
            var individual2 = new Individual(6, 4, 0, 2, 1, 5, 3);
            var co = new CrossOverOX();
            co.Cross(individual1, individual2);
            CollectionAssert.AreEqual(new uint[] { 0, 4, 2, 3, 1, 5, 6 }, co.Offspring1.ToArray());
            CollectionAssert.AreEqual(new uint[] { 6, 1, 0, 2, 4, 5, 3 }, co.Offspring2.ToArray());
        }
        
        [TestMethod]
        public void CrossTest5()
        {
            var individual1 = new Individual(8, 4, 7, 3, 6, 2, 5, 1, 9, 0);
            var individual2 = new Individual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            var co = new CrossOverOX();
            co.Cross(individual1, individual2);
            CollectionAssert.AreEqual(new uint[] { 8, 1, 2, 3, 4, 5, 6, 7, 9, 0 }, co.Offspring1.ToArray());
            CollectionAssert.AreEqual(new uint[] { 0, 4, 7, 3, 6, 2, 5, 1, 8, 9 }, co.Offspring2.ToArray());
        }
    }
}
