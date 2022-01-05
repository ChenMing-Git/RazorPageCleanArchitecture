// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CleanArchitecture.Razor.Application.MappingRules.DTOs;

namespace CleanArchitecture.Razor.Application.MappingRules.Commands.AddEdit;

public class AddEditMappingRuleCommand : MappingRuleDto, IRequest<Result<int>>, IMapFrom<MappingRule>
{
    public UploadRequest UploadRequest { get; set; }
}

public class AddEditMappingRuleCommandHandler : IRequestHandler<AddEditMappingRuleCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditMappingRuleCommandHandler> _localizer;
    private readonly IUploadService _uploadService;

    public AddEditMappingRuleCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditMappingRuleCommandHandler> localizer,
            IUploadService uploadService,
            IMapper mapper
            )
    {
        _context = context;
        _localizer = localizer;
        _uploadService = uploadService;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditMappingRuleCommand request, CancellationToken cancellationToken)
    {

        var isexist = await isExist(request.Id,
            request.LegacyField1,
            request.LegacyField2,
            request.LegacyField3,
            request.NewValueField,
            request.LegacySystem,
            request.MigrationApproach);
        if (isexist)
        {
            throw new Exception($"Duplicate mapping rule:{request.Name} ");
        }
        
        if (request.Id > 0)
        {
            var item = await _context.MappingRules.FindAsync(new object[] { request.Id }, cancellationToken);
            item = _mapper.Map(request, item);
            if (request.UploadRequest != null && request.UploadRequest.Data != null)
            {
                var buffer = RemoveDataIfExists(request.UploadRequest.Data);
                request.UploadRequest.Data= buffer;
                var result = await _uploadService.UploadAsync(request.UploadRequest);
                item.TemplateFile = result;
            }
            var hasdata = await _context.FieldMappingValues.AnyAsync(x => x.MappingRuleId == item.Id);
            if (hasdata && item.Status== "Not started")
            {
                item.Status = "Ongoing";
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
           
            var item = _mapper.Map<MappingRule>(request);
            if (request.UploadRequest != null && request.UploadRequest.Data != null)
            {
                var buffer = RemoveDataIfExists(request.UploadRequest.Data);
                request.UploadRequest.Data = buffer;
                var result = await _uploadService.UploadAsync(request.UploadRequest);
                item.TemplateFile = result;
            }
            _context.MappingRules.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
    private byte[] RemoveDataIfExists(byte[] buffer)
    {
        var xmlstring = System.Text.Encoding.UTF8.GetString(buffer).Trim();
        var xdoc = XDocument.Parse(xmlstring, LoadOptions.PreserveWhitespace);
        var data = xdoc.Descendants().Where(x => x.Name.LocalName == "Worksheet" && x.FirstAttribute.Value == "Data").First();
        var datatable = data.Descendants().Where(x => x.Name.LocalName == "Table");
        var expandedRowCount = datatable.Attributes().Where(x => x.Name.LocalName == "ExpandedRowCount").First();
        var count = Convert.ToInt32(expandedRowCount.Value);
        if (count == 5)
        {
            return buffer;
        }
        var dt = new DataTable();
        foreach (var row in datatable.Descendants().Where(x => x.Name.LocalName == "Row").Skip(4).ToList())
        {
            row.Remove();
        }
        expandedRowCount.Value = "5";
        using (TextWriter writer = new StringWriter())
        {
            xdoc.Save(writer, SaveOptions.DisableFormatting);
            var output = writer.ToString();
            // replace default namespace prefix:<ss:
            string result = Regex.Replace(Regex.Replace(output, "<ss:", "<"), "</ss:", "</");
            return Encoding.UTF8.GetBytes(result);
        }

    }
    private async Task<bool> isExist(int id,string legacyField1, string legacyField2, string legacyField3, string newValueField,string legacySystem,string migrationApproach)
    {
        return await _context.MappingRules.AnyAsync(x =>x.Id!=id &&
        x.LegacyField1 == legacyField1 &&
        x.LegacyField2 == legacyField2 &&
        x.LegacyField3 == legacyField3 &&
        x.NewValueField == newValueField &&
        x.LegacySystem ==legacySystem &&
        x.MigrationApproach== migrationApproach);
    }
}

