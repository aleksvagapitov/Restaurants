namespace Application.Admin.Owners
{
    public class ConfirmAccountEmailViewModel
    {
        public ConfirmAccountEmailViewModel(string confirmEmailUrl, string password)
        {
            ConfirmEmailUrl = confirmEmailUrl;
            Password = password;
        }

        public string ConfirmEmailUrl { get; set; }
        public string Password { get; set; }
    }
}
