using Microsoft.Extensions.Logging;

namespace Emergence.Test
{
    public class TestBase
    {
        private readonly ILoggerFactory _loggerFactory;
        protected ILogger Logger { get; }
        public TestBase()
        {
            _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        }

        protected ILogger GetLogger<T>() => _loggerFactory.CreateLogger<T>();
    }
}
