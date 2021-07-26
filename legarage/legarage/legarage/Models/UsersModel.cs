using System;

namespace legarage.Models
{
    public class UsersModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int IsAdmin { get; set; }
        public string Website { get; set; }
        public string Youtube { get; set; }
        public string Linkedin { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Snapchat { get; set; }
        public string Tiktok { get; set; }
        public string Facebook { get; set; }
        public string Whatsapp { get; set; }
        public string Fax { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public AddressModel Address { get; set; }
        public ImagesModel Image { get; set; }
        public URLModel URL { get; set; }


    }
}