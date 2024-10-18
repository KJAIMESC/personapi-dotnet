namespace personapi_dotnet.Dto
{
    public class EstudioCreateUpdateDto
    {
        public int IdProf { get; set; }
        public long CcPer { get; set; }
        public DateOnly? Fecha { get; set; }
        public string? Univer { get; set; }
    }
}
