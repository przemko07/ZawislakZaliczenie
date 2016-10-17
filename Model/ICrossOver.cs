using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface ICrossOver
    {
        void Cross(Individual i1, Individual i2);

        Individual offspring1 { get; }
        Individual offspring2 { get; }
    }
}
