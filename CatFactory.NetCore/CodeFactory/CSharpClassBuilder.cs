using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CatFactory.CodeFactory;
using CatFactory.NetCore.ObjectOrientedProgramming;
using Microsoft.Extensions.Logging;

namespace CatFactory.NetCore.CodeFactory
{
    public class CSharpClassBuilder : CSharpCodeBuilder
    {
        public static void CreateFiles(string outputDirectory, string subdirectory, bool forceOverwrite, params CSharpClassDefinition[] definitions)
        {
            foreach (var definition in definitions)
            {
                var codeBuilder = new CSharpClassBuilder
                {
                    OutputDirectory = outputDirectory,
                    ForceOverwrite = forceOverwrite,
                    ObjectDefinition = definition
                };

                codeBuilder.CreateFile(subdirectory);
            }
        }

        public CSharpClassBuilder()
            : base()
        {
        }

        public CSharpClassBuilder(ILogger<CSharpClassBuilder> logger)
            : base(logger)
        {
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDotNetClassDefinition m_classDefinition;

        public IDotNetClassDefinition ClassDefinition
            => m_classDefinition ?? (m_classDefinition = ObjectDefinition as IDotNetClassDefinition);

        public override string FileName
            => ClassDefinition.Name;

        public override void Translating()
        {
            if (AddNamespacesAtStart && ObjectDefinition.Namespaces.Count > 0)
            {
                foreach (var item in ObjectDefinition.Namespaces)
                {
                    Lines.Add(new CodeLine("using {0};", item));
                }

                Lines.Add(new EmptyLine());
            }

            var start = 0;

            if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
            {
                start = 1;

                Lines.Add(new CodeLine("namespace {0}", ObjectDefinition.Namespace));
                Lines.Add(new CodeLine("{"));

                if (!AddNamespacesAtStart)
                {
                    foreach (var item in ObjectDefinition.Namespaces)
                    {
                        Lines.Add(new CodeLine("{0}using {1};", Indent(1), item));
                    }

                    Lines.Add(new EmptyLine());
                }
            }

            AddDocumentation(start, ObjectDefinition);

            this.AddAttributes(start);

            var declaration = new List<string>
            {
                ObjectDefinition.AccessModifier.ToString().ToLower()
            };

            if (ClassDefinition.IsStatic)
                declaration.Add("static");

            if (ClassDefinition.IsAbstract)
                declaration.Add("abstract");

            if (ClassDefinition.IsPartial)
                declaration.Add("partial");

            declaration.Add("class");

            if (ClassDefinition.GenericTypes.Count == 0)
                declaration.Add(ObjectDefinition.Name);
            else
                declaration.Add(string.Format("{0}<{1}>", ObjectDefinition.Name, string.Join(", ", ClassDefinition.GenericTypes.Select(item => item.Name))));

            if (ClassDefinition.HasInheritance)
            {
                declaration.Add(":");

                var parents = new List<string>();

                if (!string.IsNullOrEmpty(ClassDefinition.BaseClass))
                    parents.Add(ClassDefinition.BaseClass);

                if (ClassDefinition.Implements.Count > 0)
                    parents.AddRange(ClassDefinition.Implements);

                declaration.Add(string.Join(", ", parents));
            }

            if (ClassDefinition.GenericTypes.Count > 0)
                declaration.Add(string.Join(", ", ClassDefinition.GenericTypes.Where(item => !string.IsNullOrEmpty(item.Constraint)).Select(item => string.Format("where {0}", item.Constraint))));

            Lines.Add(new CodeLine("{0}{1}", Indent(start), string.Join(" ", declaration)));

            Lines.Add(new CodeLine("{0}{{", Indent(start)));

            AddConstants(start);

            AddEvents(start);

            AddFields(start);

            AddStaticConstructor(start);

            AddConstructors(start);

            AddFinalizer(start);

            AddIndexers(start);

            AddProperties(start);

            AddMethods(start);

            Lines.Add(new CodeLine("{0}{1}", Indent(start), "}"));

            if (!string.IsNullOrEmpty(ObjectDefinition.Namespace))
                Lines.Add(new CodeLine("}"));
        }

        protected virtual void AddConstants(int start)
        {
            if (ClassDefinition.Constants == null || ClassDefinition.Constants.Count == 0)
                return;

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), ConstantsRegionDescription));
                Lines.Add(new EmptyLine());
            }

            for (var i = 0; i < ClassDefinition.Constants.Count; i++)
            {
                var constant = ClassDefinition.Constants[i];

                if (constant.Value is bool)
                {
                    Lines.Add(new CodeLine("{0}{1} const {2} {3} = {4};", Indent(start + 1), constant.AccessModifier.ToString().ToLower(), constant.Type, constant.Name, constant.Value.ToString().ToLower()));
                }
                else if (constant.Value is string)
                {
                    Lines.Add(new CodeLine("{0}{1} const {2} {3} = \"{4}\";", Indent(start + 1), constant.AccessModifier.ToString().ToLower(), constant.Type, constant.Name, constant.Value.ToString()));
                }
                else
                {
                    Lines.Add(new CodeLine("{0}{1} const {2} {3} = {4};", Indent(start + 1), constant.AccessModifier.ToString().ToLower(), constant.Type, constant.Name, constant.Value.ToString()));
                }

                if (i < ClassDefinition.Constants.Count - 1)
                    Lines.Add(new EmptyLine());
            }

            Lines.Add(new EmptyLine());

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new EmptyLine());
            }
        }

        protected virtual void AddEvents(int start)
        {
            if (ClassDefinition.Events == null || ClassDefinition.Events.Count == 0)
                return;

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), EventsRegionDescription));
                Lines.Add(new EmptyLine());
            }

            for (var i = 0; i < ClassDefinition.Events.Count; i++)
            {
                var @event = ClassDefinition.Events[i];

                Lines.Add(new CodeLine("{0}{1} event {2} {3};", Indent(start + 1), @event.AccessModifier.ToString().ToLower(), @event.Type, @event.Name));

                if (i < ClassDefinition.Events.Count - 1)
                    Lines.Add(new EmptyLine());
            }

            Lines.Add(new EmptyLine());

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new EmptyLine());
            }
        }

        protected virtual void AddFields(int start)
        {
            if (ClassDefinition.Fields == null || ClassDefinition.Fields.Count == 0)
                return;

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), FieldsRegionDescription));
                Lines.Add(new EmptyLine());
            }

            for (var i = 0; i < ClassDefinition.Fields.Count; i++)
            {
                var field = ClassDefinition.Fields[i];

                var fieldSignature = new List<string>
                    {
                        field.AccessModifier.ToString().ToLower()
                    };

                if (field.IsStatic)
                    fieldSignature.Add("static");

                if (field.IsReadOnly)
                    fieldSignature.Add("readonly");

                fieldSignature.Add(field.Type);

                fieldSignature.Add(field.Name);

                if (!string.IsNullOrEmpty(field.Value))
                {
                    fieldSignature.Add("=");
                    fieldSignature.Add(field.Value);
                }

                Lines.Add(new CodeLine("{0}{1};", Indent(start + 1), string.Join(" ", fieldSignature)));
            }

            Lines.Add(new EmptyLine());

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new EmptyLine());
            }
        }

        protected virtual void AddStaticConstructor(int start)
        {
            if (ClassDefinition.StaticConstructor == null)
                return;

            Lines.Add(new CodeLine("{0}static {1}()", Indent(start + 1), ObjectDefinition.Name));
            Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "{"));

            foreach (var line in ClassDefinition.StaticConstructor.Lines)
            {
                if (line.IsComment())
                    Lines.Add(new CommentLine("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content)));
                else if (line.IsPreprocessorDirective())
                    Lines.Add(new PreprocessorDirectiveLine("{0}{1}", Indent(start + 2 + line.Indent), GetPreprocessorDirective(line.Content)));
                else if (line.IsReturn())
                    Lines.Add(new ReturnLine("{0}{1}", Indent(start + 2 + line.Indent), GetReturn(line.Content)));
                else if (line.IsTodo())
                    Lines.Add(new TodoLine("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content)));
                else
                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2 + line.Indent), line.Content));
            }

            Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "}"));
            Lines.Add(new EmptyLine());
        }

        protected virtual void AddConstructors(int start)
        {
            if (ClassDefinition.Constructors == null || ClassDefinition.Constructors.Count == 0)
                return;

            if (ClassDefinition.UseRegionsToGroupClassMembers)
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), ConstructorsRegionDescription));

            for (var i = 0; i < ClassDefinition.Constructors.Count; i++)
            {
                var constructor = ClassDefinition.Constructors[i];

                AddDocumentation(start + 1, constructor);

                Lines.Add(new CodeLine("{0}{1} {2}({3})", Indent(start + 1), constructor.AccessModifier.ToString().ToLower(), ObjectDefinition.Name, constructor.Parameters.Count == 0 ? string.Empty : string.Join(", ", constructor.Parameters.Select(item => string.Format("{0} {1}", item.Type, item.Name)))));

                if (!string.IsNullOrEmpty(constructor.Invocation))
                    Lines.Add(new CodeLine("{0}: {1}", Indent(start + 2), constructor.Invocation));

                Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "{"));

                foreach (var line in constructor.Lines)
                {
                    if (line.IsComment())
                        Lines.Add(new CommentLine("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content)));
                    else if (line.IsPreprocessorDirective())
                        Lines.Add(new PreprocessorDirectiveLine("{0}{1}", Indent(start + 2 + line.Indent), GetPreprocessorDirective(line.Content)));
                    else if (line.IsReturn())
                        Lines.Add(new ReturnLine("{0}{1}", Indent(start + 2 + line.Indent), GetReturn(line.Content)));
                    else if (line.IsTodo())
                        Lines.Add(new TodoLine("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content)));
                    else
                        Lines.Add(new CodeLine("{0}{1}", Indent(start + 2 + line.Indent), line.Content));
                }

                Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "}"));

                if (i < ClassDefinition.Constructors.Count - 1)
                    Lines.Add(new CodeLine());
            }

            Lines.Add(new EmptyLine());

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new EmptyLine());
            }
        }

        protected virtual void AddFinalizer(int start)
        {
            if (ClassDefinition.Finalizer == null || ClassDefinition.Finalizer.Lines == null || ClassDefinition.Finalizer.Lines.Count == 0)
                return;

            Lines.Add(new CodeLine("{0}~{1}()", Indent(start + 1), ObjectDefinition.Name));
            Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "{"));

            foreach (var line in ClassDefinition.Finalizer.Lines)
            {
                if (line.IsComment())
                    Lines.Add(new CommentLine("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content)));
                else if (line.IsPreprocessorDirective())
                    Lines.Add(new PreprocessorDirectiveLine("{0}", GetPreprocessorDirective(line.Content)));
                else if (line.IsReturn())
                    Lines.Add(new ReturnLine("{0}{1}", Indent(start + 2 + line.Indent), GetReturn(line.Content)));
                else if (line.IsTodo())
                    Lines.Add(new TodoLine("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content)));
                else
                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2 + line.Indent), line.Content));
            }

            Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "}"));
            Lines.Add(new EmptyLine());
        }

        protected virtual void AddIndexers(int start)
        {
            if (ClassDefinition.Indexers == null || ClassDefinition.Indexers.Count == 0)
                return;

            if (ClassDefinition.UseRegionsToGroupClassMembers)
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), IndexersRegionDescription));

            for (var i = 0; i < ClassDefinition.Indexers.Count; i++)
            {
                var indexer = ClassDefinition.Indexers[i];

                var parameters = string.Join(", ", indexer.Parameters.Select(item => string.Format("{0} {1}", item.Type, item.Name)));

                Lines.Add(new CodeLine("{0}{1} {2} {3}[{4}]", Indent(start + 1), indexer.AccessModifier.ToString().ToLower(), indexer.Type, "this", parameters));

                Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "{"));

                if (indexer.GetBody.Count > 0)
                {
                    Lines.Add(new CodeLine("{0}get", Indent(start + 2)));

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "{"));

                    foreach (var line in indexer.GetBody)
                    {
                        if (line.IsComment())
                            Lines.Add(new CommentLine("{0}{1}", Indent(start + 3 + line.Indent), GetComment(line.Content)));
                        else if (line.IsPreprocessorDirective())
                            Lines.Add(new PreprocessorDirectiveLine("{0}{1}", Indent(start + 3 + line.Indent), GetPreprocessorDirective(line.Content)));
                        else if (line.IsReturn())
                            Lines.Add(new ReturnLine("{0}{1}", Indent(start + 3 + line.Indent), GetReturn(line.Content)));
                        else if (line.IsTodo())
                            Lines.Add(new TodoLine("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content)));
                        else
                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 3 + line.Indent), line.Content));
                    }

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "}"));
                }

                if (indexer.SetBody.Count > 0)
                {
                    Lines.Add(new CodeLine("{0}set", Indent(start + 2)));

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "{"));

                    foreach (var line in indexer.SetBody)
                    {
                        if (line.IsComment())
                            Lines.Add(new CommentLine("{0}{1}", Indent(start + 3 + line.Indent), GetComment(line.Content)));
                        else if (line.IsPreprocessorDirective())
                            Lines.Add(new PreprocessorDirectiveLine("{0}{1}", Indent(start + 3 + line.Indent), GetPreprocessorDirective(line.Content)));
                        else if (line.IsReturn())
                            Lines.Add(new ReturnLine("{0}{1}", Indent(start + 3 + line.Indent), GetReturn(line.Content)));
                        else if (line.IsTodo())
                            Lines.Add(new TodoLine("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content)));
                        else
                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 3 + line.Indent), line.Content));
                    }

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "}"));
                }

                Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "}"));

                if (i < ClassDefinition.Indexers.Count - 1)
                    Lines.Add(new EmptyLine());
            }

            Lines.Add(new CodeLine());

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new EmptyLine());
            }
        }

        protected virtual void AddProperties(int start)
        {
            if (ClassDefinition.Properties == null || ClassDefinition.Properties.Count == 0)
                return;

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 1), PropertiesRegionDescription));
                Lines.Add(new EmptyLine());
            }

            for (var i = 0; i < ClassDefinition.Properties.Count; i++)
            {
                var property = ClassDefinition.Properties[i];

                if (property.Attributes.Count > 0)
                    this.AddAttributes(property, start);

                if (property.IsReadOnly)
                {
                    if (property.GetBody.Count == 0)
                    {
                        Lines.Add(new CodeLine("{0}{1} {2} {3} {{ get; }}", Indent(start + 1), property.AccessModifier.ToString().ToLower(), property.Type, property.Name));
                    }
                    else
                    {
                        var propertySignature = new List<string>
                        {
                            property.AccessModifier.ToString().ToLower()
                        };

                        if (property.IsOverride)
                            propertySignature.Add("override");
                        else if (property.IsVirtual)
                            propertySignature.Add("virtual");

                        propertySignature.Add(property.Type);

                        propertySignature.Add(property.Name);

                        Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), string.Join(" ", propertySignature)));

                        if (property.GetBody.Count == 1)
                        {
                            Lines.Add(new CodeLine("{0}=> {1}", Indent(start + 2), property.GetBody[0].Content.Replace("return ", string.Empty)));
                        }
                        else
                        {
                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "{"));

                            Lines.Add(new CodeLine("{0}get", Indent(start + 2)));

                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "{"));

                            foreach (var line in property.GetBody)
                            {
                                if (line.IsComment())
                                    Lines.Add(new CommentLine("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content)));
                                else if (line.IsReturn())
                                    Lines.Add(new ReturnLine("{0}{1}", Indent(start + 2 + line.Indent), GetReturn(line.Content)));
                                else if (line.IsTodo())
                                    Lines.Add(new TodoLine("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content)));
                                else
                                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2 + line.Indent), line.Content));
                            }

                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "}"));

                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "}"));
                        }
                    }
                }
                else if (property.IsAutomatic)
                {
                    var propertySignature = new List<string>
                    {
                        property.AccessModifier.ToString().ToLower()
                    };

                    if (property.IsOverride)
                        propertySignature.Add("override");
                    else if (property.IsVirtual)
                        propertySignature.Add("virtual");

                    propertySignature.Add(property.Type);

                    propertySignature.Add(property.Name);

                    if (string.IsNullOrEmpty(property.InitializationValue))
                        Lines.Add(new CodeLine("{0}{1} {{ get; set; }}", Indent(start + 1), string.Join(" ", propertySignature)));
                    else
                        Lines.Add(new CodeLine("{0}{1} {{ get; set; }} = {2};", Indent(start + 1), string.Join(" ", propertySignature), property.InitializationValue));
                }
                else
                {
                    Lines.Add(new CodeLine("{0}{1} {2} {3}", Indent(start + 1), property.AccessModifier.ToString().ToLower(), property.Type, property.Name));

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "{"));

                    Lines.Add(new CodeLine("{0}get", Indent(start + 2)));

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "{"));

                    foreach (var line in property.GetBody)
                    {
                        if (line.IsComment())
                            Lines.Add(new CommentLine("{0}{1}", Indent(start + 3 + line.Indent), GetComment(line.Content)));
                        else if (line.IsPreprocessorDirective())
                            Lines.Add(new PreprocessorDirectiveLine("{0}{1}", Indent(start + 3 + line.Indent), GetPreprocessorDirective(line.Content)));
                        else if (line.IsReturn())
                            Lines.Add(new ReturnLine("{0}{1}", Indent(start + 3 + line.Indent), GetReturn(line.Content)));
                        else if (line.IsTodo())
                            Lines.Add(new TodoLine("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content)));
                        else
                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 3 + line.Indent), line.Content));
                    }

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "}"));

                    Lines.Add(new CodeLine("{0}set", Indent(start + 2)));

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "{"));

                    foreach (var line in property.SetBody)
                    {
                        if (line.IsComment())
                            Lines.Add(new CommentLine("{0}{1}", Indent(start + 3 + line.Indent), GetComment(line.Content)));
                        else if (line.IsPreprocessorDirective())
                            Lines.Add(new PreprocessorDirectiveLine("{0}{1}", Indent(start + 3 + line.Indent), GetPreprocessorDirective(line.Content)));
                        else if (line.IsReturn())
                            Lines.Add(new ReturnLine("{0}{1}", Indent(start + 3 + line.Indent), GetReturn(line.Content)));
                        else if (line.IsTodo())
                            Lines.Add(new TodoLine("{0}{1}", Indent(start + 3 + line.Indent), GetTodo(line.Content)));
                        else
                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 3 + line.Indent), line.Content));
                    }

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 2), "}"));

                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "}"));
                }

                if (i < ClassDefinition.Properties.Count - 1)
                    Lines.Add(new EmptyLine());
            }

            Lines.Add(new EmptyLine());

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 1)));
                Lines.Add(new EmptyLine());
                Lines.Add(new EmptyLine());
            }
        }

        protected virtual void AddMethods(int start)
        {
            if (ClassDefinition.Methods == null || ClassDefinition.Methods.Count == 0)
                return;

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new CodeLine("{0}#region {1}", Indent(start + 2), MethodsRegionDescription));
                Lines.Add(new EmptyLine());
            }

            for (var i = 0; i < ClassDefinition.Methods.Count; i++)
            {
                var method = ClassDefinition.Methods[i];

                this.AddAttributes(method, start);

                var methodSignature = new List<string>
                {
                    method.AccessModifier.ToString().ToLower()
                };

                if (method.IsStatic)
                    methodSignature.Add("static");
                else if (method.IsOverride)
                    methodSignature.Add("override");
                else if (method.IsVirtual)
                    methodSignature.Add("virtual");
                else if (method.IsAbstract)
                    methodSignature.Add("abstract");

                if (method.IsAsync)
                    methodSignature.Add("async");

                methodSignature.Add(string.IsNullOrEmpty(method.Type) ? "void" : method.Type);

                var parameters = new List<string>();

                for (var j = 0; j < method.Parameters.Count; j++)
                {
                    var parameter = method.Parameters[j];

                    var parametersAttributes = this.AddAttributes(parameter);

                    var parameterDef = string.Empty;

                    if (string.IsNullOrEmpty(parameter.DefaultValue))
                    {
                        if (string.IsNullOrEmpty(parametersAttributes))
                            parameterDef = string.Format("{0} {1}", parameter.Type, parameter.Name);
                        else
                            parameterDef = string.Format("{0}{1} {2}", parametersAttributes, parameter.Type, parameter.Name);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(parametersAttributes))
                            parameterDef = string.Format("{0} {1} = {2}", parameter.Type, parameter.Name, parameter.DefaultValue);
                        else
                            parameterDef = string.Format("{0}{1} {2} = {3}", parametersAttributes, parameter.Type, parameter.Name, parameter.DefaultValue);
                    }

                    parameters.Add(method.IsExtension && j == 0 ? string.Format("this {0}", parameterDef) : parameterDef);
                }

                if (method.GenericTypes.Count == 0)
                    methodSignature.Add(string.Format("{0}({1})", method.Name, string.Join(", ", parameters)));
                else
                    methodSignature.Add(string.Format("{0}<{1}>({2})", method.Name, string.Join(", ", method.GenericTypes.Select(item => item.Name)), string.Join(", ", parameters)));

                if (method.GenericTypes.Count > 0)
                    methodSignature.Add(string.Join(", ", method.GenericTypes.Where(item => !string.IsNullOrEmpty(item.Constraint)).Select(item => string.Format("where {0}", item.Constraint))));

                if (method.IsAbstract)
                {
                    Lines.Add(new CodeLine("{0}{1};", Indent(start + 1), string.Join(" ", methodSignature)));
                }
                else
                {
                    Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), string.Join(" ", methodSignature)));

                    if (method.Lines.Count == 0)
                    {
                        Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "{"));
                        Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "}"));
                    }
                    else if (method.Lines.Count == 1)
                    {
                        var line = method.Lines[0];

                        if (line.IsReturn())
                        {
                            var content = line.Content.StartsWith("return ") ? line.Content.Insert(0, "=> ") : line.Content.Insert(0, "=> ");

                            Lines.Add(new ReturnLine("{0}{1}", Indent(start + 2), content));
                        }
                        else
                        {
                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "{"));

                            if (line.IsComment())
                                Lines.Add(new CommentLine("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content)));
                            else if (line.IsPreprocessorDirective())
                                Lines.Add(new PreprocessorDirectiveLine("{0}{1}", Indent(start + 2 + line.Indent), GetPreprocessorDirective(line.Content)));
                            else if (line.IsReturn())
                                Lines.Add(new ReturnLine("{0}{1}", Indent(start + 2 + line.Indent), GetReturn(line.Content)));
                            else if (line.IsTodo())
                                Lines.Add(new TodoLine("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content)));
                            else
                                Lines.Add(new CodeLine("{0}{1}", Indent(start + 2 + line.Indent), line.Content));

                            Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "}"));
                        }
                    }
                    else if (method.Lines.Count > 1)
                    {
                        Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "{"));

                        foreach (var line in method.Lines)
                        {
                            if (line.IsComment())
                                Lines.Add(new CommentLine("{0}{1}", Indent(start + 2 + line.Indent), GetComment(line.Content)));
                            else if (line.IsPreprocessorDirective())
                                Lines.Add(new PreprocessorDirectiveLine("{0}{1}", Indent(start + 2 + line.Indent), GetPreprocessorDirective(line.Content)));
                            else if (line.IsReturn())
                                Lines.Add(new ReturnLine("{0}{1}", Indent(start + 2 + line.Indent), GetReturn(line.Content)));
                            else if (line.IsTodo())
                                Lines.Add(new TodoLine("{0}{1}", Indent(start + 2 + line.Indent), GetTodo(line.Content)));
                            else
                                Lines.Add(new CodeLine("{0}{1}", Indent(start + 2 + line.Indent), line.Content));
                        }

                        Lines.Add(new CodeLine("{0}{1}", Indent(start + 1), "}"));
                    }
                }

                if (i < ClassDefinition.Methods.Count - 1)
                    Lines.Add(new EmptyLine());
            }

            if (ClassDefinition.UseRegionsToGroupClassMembers)
            {
                Lines.Add(new EmptyLine());
                Lines.Add(new CodeLine("{0}#endregion", Indent(start + 2)));
                Lines.Add(new EmptyLine());
            }
        }
    }
}
