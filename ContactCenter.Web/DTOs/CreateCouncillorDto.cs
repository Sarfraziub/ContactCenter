namespace EDRSM.API.DTOs
{
    public class CreateCouncillorDto
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string Ward { get; set; }
        public IFormFile Image { get; set; }
        public string CloudinaryPublicId { get; set; }
    }
}
