using FluentValidation;
using Microsoft.AspNetCore.Http;
using SecShare.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shares.Validation;
public class AttachFileValidator : AbstractValidator<AttachFile>
{
    private readonly string[] _imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
    private readonly string[] _videoExtensions = { ".mp4", ".webm", ".avi", ".mov", ".flv", ".3gp" };
    private readonly string[] _docExtensions = { ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx" };
    public AttachFileValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(file => file.Length > 0).WithMessage("File is empty.")
            .Must(file => IsExtensionAllowed(file.FileName))
            .WithMessage("Invalid file type. Allowed: image, video, pdf, doc, ppt, xls")
            .Must(file => IsSizeAllowed(file))
            .WithMessage("File size exceeds limit for this file type.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("FileName is required.");
    }

    private bool IsExtensionAllowed(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return _imageExtensions.Contains(ext) || _videoExtensions.Contains(ext) || _docExtensions.Contains(ext);
    }

    private bool IsSizeAllowed(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (_imageExtensions.Contains(ext))
            return file.Length <= 10 * 1024 * 1024; // 10 MB

        if (_videoExtensions.Contains(ext))
            return file.Length <= 100 * 1024 * 1024; // 100 MB

        if (_docExtensions.Contains(ext))
            return file.Length <= 20 * 1024 * 1024; // 20 MB (tùy chọn)

        return false;
    }

}
