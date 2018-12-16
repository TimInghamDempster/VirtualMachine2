using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMachine2
{
    abstract class Primitive
    {
        // Current output is calculated from the previous input history
        // and we don't want to have infinite history so we define a point
        // beyond which we no longer require old data, will be initialised
        // by derived classes
        protected readonly Int32 m_maxGateDelaysRequired;

        protected List<List<Int32>> m_inputHistory = new List<List<int>>();

        // Defines the wiring to this component.  The component held in item1
        // has its output defined by the index in item2 wired to the input of
        // this component defined by the index in item3
        List<Tuple<Primitive, Int32, Int32>> m_inputMappings;

        public List<Int32> Outputs { get; protected set; }

        /// <summary>
        /// After all components have finished updating their outputs we iterate
        /// them all again to read the next set of inputs.  Without breaking this
        /// into two steps we would end up with some updates acting on stale data
        /// </summary>
        public void GetInputsForNextTimestep()
        {
            var tempList = m_inputHistory[m_inputHistory.Count - 1];
            for(int listId = 1; listId < m_inputHistory.Count; listId++)
            {
                m_inputHistory[listId] = m_inputHistory[listId - 1];
            }
            m_inputHistory[0] = tempList;

            // The input mappings might not define a value for every input, we
            // should blank them all before getting new inputs just to be safe
            for(int valueId = 0; valueId < tempList.Count; valueId++)
            {
                tempList[valueId] = 0;
            }

            foreach (var mapping in m_inputMappings)
            {
                tempList[mapping.Item3] = mapping.Item1.Outputs[mapping.Item2];
            }
        }

        /// <summary>
        /// Updates the output of the component by one gate delay of time.
        /// Using the gate delay as our fundamental unit of time as all
        /// component update delays will be a multiple of the gate delay
        /// and it gives flexibility to try different pipeline lengths etc
        /// </summary>
        public abstract void UpdateInternalStateByOneGateDelay();
    }
}
