namespace Model.Entities
{
    public class Users
    {
        public Users()
        { }

        public Users(string name, string email, string password, string phoneNumber, int roleId)
        {
            Name = name;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
            IsActive = true;
            RoleId = roleId;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}