using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Mappings;
using CleanArchitecture.Razor.Application.Common.Models;
using CleanArchitecture.Razor.Application.Products.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using CleanArchitecture.Razor.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchitecture.Razor.Application.Products.Commands.Delete
{
    public class DeleteProductCommand: IRequest<Result>
    {
      public int Id {  get; set; }
    }
    public class DeleteCheckedProductsCommand : IRequest<Result>
    {
     public int[] Id { get; set; }
    }

    public class DeleteProductCommandHandler : 
                 IRequestHandler<DeleteProductCommand, Result>,
                 IRequestHandler<DeleteCheckedProductsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteProductCommandHandler> _localizer;
        public DeleteProductCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteProductCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteProductCommandHandler method 
            var item = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
            _context.Products.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> Handle(DeleteCheckedProductsCommand request, CancellationToken cancellationToken)
        {
            //TODO:Implementing DeleteCheckedProductsCommandHandler method 
            var items = await _context.Products.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.Products.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
