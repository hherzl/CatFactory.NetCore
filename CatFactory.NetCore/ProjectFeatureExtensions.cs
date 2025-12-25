using System.Collections.Generic;
using System.Linq;
using CatFactory.CodeFactory.Scaffolding;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.NetCore;

public static class ProjectFeatureExtensions
{
    public static IEnumerable<Table> GetTables<TProjectSettings>(this ProjectFeature<TProjectSettings> projectFeature) where TProjectSettings : class, IProjectSettings, new()
    {
        foreach (var table in projectFeature.Project.Database.Tables.Where(item => projectFeature.DbObjects.Select(dbo => dbo.FullName).Contains(item.FullName)))
            yield return table;
    }

    public static IEnumerable<View> GetViews<TProjectSettings>(this ProjectFeature<TProjectSettings> projectFeature) where TProjectSettings : class, IProjectSettings, new()
    {
        foreach (var view in projectFeature.Project.Database.Views.Where(item => projectFeature.DbObjects.Select(dbo => dbo.FullName).Contains(item.FullName)))
            yield return view;
    }

    //public static IEnumerable<ScalarFunction> GetScalarFunctions<TProjectSettings>(this ProjectFeature<TProjectSettings> projectFeature) where TProjectSettings : class, IProjectSettings, new()
    //{
    //    foreach (var scalarFunction in projectFeature.Project.Database.ScalarFunctions.Where(item => projectFeature.DbObjects.Select(dbo => dbo.FullName).Contains(item.FullName)))
    //        yield return scalarFunction;
    //}

    //public static IEnumerable<TableFunction> GetTableFunctions<TProjectSettings>(this ProjectFeature<TProjectSettings> projectFeature) where TProjectSettings : class, IProjectSettings, new()
    //{
    //    foreach (var tableFunction in projectFeature.Project.Database.TableFunctions.Where(item => projectFeature.DbObjects.Select(dbo => dbo.FullName).Contains(item.FullName)))
    //        yield return tableFunction;
    //}

    //public static IEnumerable<StoredProcedure> GetStoredProcedures<TProjectSettings>(this ProjectFeature<TProjectSettings> projectFeature) where TProjectSettings : class, IProjectSettings, new()
    //{
    //    foreach (var storedProcedure in projectFeature.Project.Database.StoredProcedures.Where(item => projectFeature.DbObjects.Select(dbo => dbo.FullName).Contains(item.FullName)))
    //        yield return storedProcedure;
    //}

    //public static IEnumerable<Sequence> GetSequences<TProjectSettings>(this ProjectFeature<TProjectSettings> projectFeature) where TProjectSettings : class, IProjectSettings, new()
    //{
    //    foreach (var sequence in projectFeature.Project.Database.Sequences.Where(item => projectFeature.DbObjects.Select(dbo => dbo.FullName).Contains(item.FullName)))
    //        yield return sequence;
    //}
}
