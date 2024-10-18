namespace personapi_dotnet.Dto
{
    public class EstudioDto
    {
        public int IdProf { get; set; }
        public long CcPer { get; set; }
        public DateOnly? Fecha { get; set; }
        public string? Univer { get; set; }
        public string? CcPerName { get; set; } 
        public string? IdProfName { get; set; } 
    }
}
