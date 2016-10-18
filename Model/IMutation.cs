using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IMutation
    {
        void Mutate(Individual individual);
        bool IsMutated { get; }
        Individual MutatedIndividual { get; }
    }
}
