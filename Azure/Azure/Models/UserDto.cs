using System;

namespace Azure.Models
{
    public class UserDto
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public AddressDto Address { get; set; }
    }
}
