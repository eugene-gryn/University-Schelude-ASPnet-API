using DAL.Entities;

User user = new User()
{
    Id = 0,
    Login = "123123",
    Name = "Kuk",
    ImageLocation = "kukokkkk",
    Password = new byte[] {0x12, 0x14, 0xFF},
    Salt = new byte[] {0x14, 0x10, 0x00}
};
