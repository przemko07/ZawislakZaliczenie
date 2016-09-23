using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Program
    {
        static void Main(string[] args)
        {
            Permutacja perm = new Permutacja(30, 8);
            Console.WriteLine(perm);
        }
    }
}
