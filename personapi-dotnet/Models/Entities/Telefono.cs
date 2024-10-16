using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace personapi_dotnet.Models.Entities;

public partial class Telefono
{
    public string Num { get; set; } = null!;

    public string? Oper { get; set; }

    public long? Dueno { get; set; }

    [JsonIgnore]
    public virtual Persona? DuenoNavigation { get; set; }
}
