using Moneybox.Domain.SeedWork;
using System;

namespace Moneybox.Domain
{
    public class User: Entity
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public User(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
