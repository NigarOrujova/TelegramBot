using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramAPI.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public string Text { get; set; }
        public string MyProperty { get; set; }
    }
}
