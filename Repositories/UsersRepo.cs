using Models;
using System.Collections.Generic;

namespace Repositories
{
    public static class UsersRepo
    {
        public static List<User> Users = new List<User>()
        {
            new User(){Id=1,Email="ioana.morariu@gmail.com",Password="12345",Phone="0749385514"},
            new User(){Id=2,Email="ana.coporan@gmail.com",Password="LEIA",Phone="0720646776"},
            new User(){Id=2,Email="patricia.ruhstrat@gmail.com",Password="Fiat500",Phone="0756099765"},
            new User(){Id=2,Email="alexandru.rusmir@gmail.com",Password="FOOD",Phone="0784292935"}
        };
    }
}