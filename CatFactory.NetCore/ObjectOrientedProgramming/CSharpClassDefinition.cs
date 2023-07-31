﻿using System.Collections.Generic;
using CatFactory.ObjectOrientedProgramming;

namespace CatFactory.NetCore.ObjectOrientedProgramming
{
    public class CSharpClassDefinition : ClassDefinition, IDotNetClassDefinition
    {
        public static CSharpClassDefinition Create(AccessModifier accessModifier, string name, string ns = null, string baseClass = null)
        {
            var definition = new CSharpClassDefinition
            {
                AccessModifier = accessModifier,
                Name = name
            };

            if (!string.IsNullOrEmpty(ns))
                definition.Namespace = ns;

            if (!string.IsNullOrEmpty(baseClass))
                definition.BaseClass = baseClass;

            return definition;
        }

        public static ClassConstructorDefinition CreateCtor(AccessModifier accessModifier = AccessModifier.Public, string invocation = null)
        {
            var definition = new ClassConstructorDefinition(accessModifier);

            if (!string.IsNullOrEmpty(invocation))
                definition.Invocation = invocation;

            return definition;
        }

        public static PropertyDefinition CreateAutomaticProp(string type, string name, AccessModifier accessModifier = AccessModifier.Public, MetadataAttribute[] attributes = null)
            => new()
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsAutomatic = true,
                Attributes = attributes == null ? new List<MetadataAttribute>() : new List<MetadataAttribute>(attributes)
            };

        public static PropertyDefinition CreateReadonlyProp(string type, string name, AccessModifier accessModifier = AccessModifier.Public)
            => new()
            {
                AccessModifier = accessModifier,
                Type = type,
                Name = name,
                IsAutomatic = true,
                IsReadOnly = true
            };

        public CSharpClassDefinition()
            : base()
        {
        }

        public bool UseRegionsToGroupClassMembers { get; set; }
    }
}
