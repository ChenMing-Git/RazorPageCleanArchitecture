// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.FieldMappingValues.DTOs;

namespace CleanArchitecture.Razor.Application.FieldMappingValues.Commands.Delete;

    public class DeleteFieldMappingValueCommand: IRequest<Result>
    {
      public int Id {  get; set; }
    }
    public class DeleteCheckedFieldMappingValuesCommand : IRequest<Result>
    {
      public int[] Id {  get; set; }
    }

    public class DeleteFieldMappingValueCommandHandler : 
                 IRequestHandler<DeleteFieldMappingValueCommand, Result>,
                 IRequestHandler<DeleteCheckedFieldMappingValuesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteFieldMappingValueCommandHandler> _localizer;
        public DeleteFieldMappingValueCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteFieldMappingValueCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteFieldMappingValueCommand request, CancellationToken cancellationToken)
        {
           
           var item = await _context.FieldMappingValues.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.FieldMappingValues.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedFieldMappingValuesCommand request, CancellationToken cancellationToken)
        {
         
           var items = await _context.FieldMappingValues.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.FieldMappingValues.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }

