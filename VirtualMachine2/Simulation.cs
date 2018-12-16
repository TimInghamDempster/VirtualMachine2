using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine2
{
    /// <summary>
    /// Holds the entire simulation, not in Program.cs in case we want to re-use
    /// in other programs
    /// </summary>
    class Simulation
    {
        /// <summary>
        /// Everything in the system is a component
        /// </summary>
        List<Primitive> components = new List<Primitive>();

        /// <summary>
        /// Updates the output of every component based on previous state, then updates
        /// input based on the new outputs
        /// </summary>
        public void Run()
        {
            // Using parallel so that we can't cheat and rely on things executing in
            // sequence.  This way we have no gauruntee on what order the various
            // components will update.  The performance benefit is nice too!
            Parallel.For(
                0,
                components.Count,
                (index) => components[index].UpdateInternalStateByOneGateDelay());
            Parallel.For(
                0,
                components.Count,
                (index) => components[index].GetInputsForNextTimestep());
        }
    }
}
