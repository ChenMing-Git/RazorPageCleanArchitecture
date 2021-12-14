// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace CleanArchitecture.Razor.Domain.Enums;

public enum UploadType : byte
{
    [Description(@"Products")]
    Product,
    [Description(@"ProfilePictures")]
    ProfilePicture,
    [Description(@"Documents")]
    Document,
    [Description(@"TemplateFiles")]
    TemplateFile,
    [Description(@"ResultMappingFiles")]
    ResultMappingFile,
    [Description(@"FieldMappingValueFile")]
    FieldMappingValueFile
}
