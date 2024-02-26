namespace DEVAMEET_CSharp.Dto
{
    public class MeetUpdateDto
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public List<MeetObjectsDto> MeetObjects { get; set; }
    }
}
