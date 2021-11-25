// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Razor.Application.MappingRules.Commands.AddEdit;

public class AddEditMappingRuleCommandValidator : AbstractValidator<AddEditMappingRuleCommand>
{
    public AddEditMappingRuleCommandValidator()
    {

        RuleFor(v => v.Name)
              .MaximumLength(256)
              .NotEmpty();

    }
}

