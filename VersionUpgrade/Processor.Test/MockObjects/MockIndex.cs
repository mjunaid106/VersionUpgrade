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
        public void Update(ISource source)
        {
            throw new NotImplementedException();
        }
    }
}
