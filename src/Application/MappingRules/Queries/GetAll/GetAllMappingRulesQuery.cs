// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.MappingRules.DTOs;

namespace CleanArchitecture.Razor.Application.MappingRules.Queries.GetAll;

    public class GetAllMappingRulesQuery : IRequest<IEnumerable<MappingRuleDto>>
    {
       
    }
public class GetAllMappingRulesWithKeyQuery : IRequest<IEnumerable<MappingRuleDto>>
{
    public string Key { get; set; }
}

public class GetAllMappingRulesQueryHandler :
         IRequestHandler<GetAllMappingRulesWithKeyQuery, IEnumerable<MappingRuleDto>>,
         IRequestHandler<GetAllMappingRulesQuery, IEnumerable<MappingRuleDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllMappingRulesQueryHandler> _localizer;

        public GetAllMappingRulesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllMappingRulesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<MappingRuleDto>> Handle(GetAllMappingRulesQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.MappingRules
                         .ProjectTo<MappingRuleDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    public async Task<IEnumerable<MappingRuleDto>> Handle(GetAllMappingRulesWithKeyQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.MappingRules.Where(x=>x.Name.Contains(request.Key))
                     .OrderBy(x=>x.Name)
                     .ProjectTo<MappingRuleDto>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
        return data;
    }
}


