using Processor.Interfaces;

namespace Processor.Test.MockObjects
{
    public class MockSource: ISource
    {
        public string ReadData()
        {
            return "REM v1.1.0";
        }

        public bool WriteData(string updatedText)
        {
            return updatedText == "REM v1.2.0";
        }
    }
}
