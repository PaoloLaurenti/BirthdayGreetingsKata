using paramore.brighter.commandprocessor.Logging;
using paramore.brighter.commandprocessor.messagestore.mssql;

namespace BirthdayGreetings.Main
{
    public class MsSqlMessageStoreFactory
    {
        private readonly string _brighterMssqlDBConnectionString;
        private readonly ILog _logger;

        public MsSqlMessageStoreFactory(string brighterMssqlDBConnectionString, ILog logger)
        {
            _brighterMssqlDBConnectionString = brighterMssqlDBConnectionString;
            _logger = logger;
        }

        public MsSqlMessageStore GetMsSqlMessageStore(string tableName)
        {
            var msSqlMessageStoreConfiguration = new MsSqlMessageStoreConfiguration(_brighterMssqlDBConnectionString,
                                                                                    tableName,
                                                                                    MsSqlMessageStoreConfiguration.DatabaseType.MsSqlServer);
            return new MsSqlMessageStore(msSqlMessageStoreConfiguration, _logger);
        }
    }
}