namespace CatFactory.NetCore.Tests
{
    public class ScaffoldingTest
    {
        protected readonly string _baseDirectory;
        protected readonly string _solutionDirectory;
        protected readonly string _domainDirectory;
        protected readonly string _entitiesDirectory;
        protected readonly string _exceptionsDirectory;
        protected readonly string _enumsDirectory;
        protected readonly string _infrastructureDirectory;
        protected readonly string _persistenceDirectory;
        protected readonly string _queryModelsDirectory;

        public ScaffoldingTest()
        {
            _baseDirectory = @"C:\Temp\CatFactory.NetCore";
            _solutionDirectory = "CleanArchitecture";
            _domainDirectory = "Domain";
            _entitiesDirectory = "Entities";
            _exceptionsDirectory = "Exceptions";
            _enumsDirectory = "Enums";
            _infrastructureDirectory = "Infrastructure";
            _persistenceDirectory = "Persistence";
            _queryModelsDirectory = "QueryModels";
        }
    }
}
