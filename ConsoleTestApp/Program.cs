using DAL.Entities;

User user = new User()
{
    Id = 0,
    Login = "123123",
    Name = "Kuk",
    ImageLocation = "kukokkkk",
    Password = new byte[] {0x12, 0x14, 0xFF},
    Salt = new byte[] {0x14, 0x10, 0x00},
    Settings =
    {
        NotifyAboutCouple = true,
        NotifyBeforeCouple = true,
        NotifyAboutHomework = true,
        NotifyAboutDeadlineHomework = true,
        NotifyAboutLoseDeadlineHomework = true
    },

    Groups = new List<Group>()
    {
        new Group() { 
            Id = 1,
            Name = "ddd", 
            Users = new List<User>()
            {

            },


        }
    },

    Homeworks = new List<Homework>()
    {
    },

    IsAdmin = true
};
