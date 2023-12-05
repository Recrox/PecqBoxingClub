using System;

namespace RamDam.BackEnd.Core.Models.Api;

public class SubMovie
{
    public Guid Id { get; set; }

    public Guid IdMovie { get; set; }
    public string Title { get; set; }
    public string Attach { get; set; }
    public int Order { get; set; }
}
