namespace ExampleAPI.Models
{
    public class UserWithTocken: Users
    {
        public string Token { get; set; }

        public UserWithTocken(Users user)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.Login = user.Login;
        }
    }
}
