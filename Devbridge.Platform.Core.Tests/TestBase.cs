using Devbridge.Platform.Core.Tests.TestHelpers;

namespace Devbridge.Platform.Core.Tests
{
    public abstract class TestBase
    {
        private RandomTestDataProvider testDataProvider;

        private static bool started;

        protected TestBase()
        {
            if (!started)
            {
                Application.Initialize();

                started = true;
            }
        }

        public RandomTestDataProvider TestDataProvider
        {
            get
            {
                if (testDataProvider == null)
                {
                    testDataProvider = new RandomTestDataProvider();
                }
                return testDataProvider;
            }
        }
    }
}
