using RamDam.BackEnd.Core.Models.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RamDam.BackEnd.Core.Models.Table;

[Table("Submovie", Schema = "dbo")]
public class SubMovie
{
    [Key]
    public Guid Id { get; set; }

    public Guid IdMovie { get; set; }
    public string Title { get; set; }
    public string Attach { get; set; }

    [ForeignKey(nameof(IdMovie))]
    public Movie Movie { get; set; }
    public int Order { get; set; }
}
