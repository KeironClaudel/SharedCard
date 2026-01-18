namespace SharedCard.Api.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; } = null!;
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    }
}
