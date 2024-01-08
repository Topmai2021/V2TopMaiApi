namespace TopMai.Domain.DTO.Profiles
{
	public class ConsultProfileDto
    {
        public Guid Id { get; set; }
        public string? ProfileUrl { get; set; }        
        public int? Sales { get; set; }
        public string? LastName { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }
        public string? Lenguages { get; set; }
        public string UrlImage{ get; set; }
        public string? Land { get; set; }   
        public Guid? WalletId { get; set; }
        public string StrGender { get; set; }
        public string FullName { get { return $"{this.Name} {this.LastName}"; } }
        public DateTime? createdAt { get; set; }
        public DateTime? birthDate { get; set; }
    }
}
