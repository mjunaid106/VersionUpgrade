using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor.Interfaces;

namespace Processor.Test.MockObjects
{
    public class MockIndex : IIndex
    {
        public void Update(int thread, ISource source)
        {
            throw new NotImplementedException();
        }

        public bool IsSourceProcessed(ISource source)
        {
            throw new NotImplementedException();
        }


        public void RecordAlreadyProcessed(int thread, ISource source)
        {
            throw new NotImplementedException();
        }
    }
}
