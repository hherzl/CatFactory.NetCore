using System.Collections.Generic;
using System.Linq;
using CatFactory.CodeFactory.Scaffolding;
using CatFactory.NetCore.CodeFactory;
using CatFactory.ObjectRelationalMapping;
using Microsoft.Extensions.Logging;

namespace CatFactory.NetCore
{
    public class CSharpProject<TProjectSettings> : Project<TProjectSettings> where TProjectSettings : class, IProjectSettings, new()
    {
        public CSharpProject()
            : base()
        {
            CodeNamingConvention = new DotNetNamingConvention();
            NamingService = new NamingService();
        }

        public CSharpProject(ILogger<CSharpProject<TProjectSettings>> logger)
            : base(logger)
        {
            CodeNamingConvention = new DotNetNamingConvention();
            NamingService = new NamingService();
        }

        protected virtual IEnumerable<DbObject> GetDbObjectsBySchema(string schema)
        {
            foreach (var item in Database.Tables.Where(table => table.Schema == schema))
            {
                yield return new DbObject(item.Schema, item.Name)
                {
                    Type = "Table"
                };
            }

            foreach (var item in Database.Views.Where(view => view.Schema == schema))
            {
                yield return new DbObject(item.Schema, item.Name)
                {
                    Type = "View"
                };
            }

            foreach (var item in Database.ScalarFunctions.Where(scalarFunction => scalarFunction.Schema == schema))
            {
                yield return new DbObject(item.Schema, item.Name)
                {
                    Type = "ScalarFunction"
                };
            }

            foreach (var item in Database.TableFunctions.Where(tableFunction => tableFunction.Schema == schema))
            {
                yield return new DbObject(item.Schema, item.Name)
                {
                    Type = "TableFunction"
                };
            }

            foreach (var item in Database.StoredProcedures.Where(storedProcedure => storedProcedure.Schema == schema))
            {
                yield return new DbObject(item.Schema, item.Name)
                {
                    Type = "StoredProcedure"
                };
            }

            foreach (var item in Database.Sequences.Where(sequence => sequence.Schema == schema))
            {
                yield return new DbObject(item.Schema, item.Name)
                {
                    Type = "Sequence"
                };
            }
        }
    }
}
