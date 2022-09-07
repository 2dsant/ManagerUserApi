using System.Collections.Generic;
using Manager.Domain.Interfaces;
using Manager.Domain.Validators;

namespace Manager.Domain.Entities
{
    public class User : Base, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        //EF
        protected User(){ }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            _errors = new List<string>();

            this.Validate();
        }

        public bool Validate() 
            => base.Validate<UserValidator, User>(new UserValidator(), this);

        public void SetName(string name)
        {
            Name = name;
            Validate();
        }

        public void SetPassword(string password)
        {
            Password = password;
            Validate();
        }

        public void SetEmail(string email)
        {
            Email = email;
            Validate();
        }
    }
}