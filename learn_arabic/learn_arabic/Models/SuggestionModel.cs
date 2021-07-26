using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class VistorSuggestionModel:UserSuggestionModel
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Mobile { get; set; } = "";
    }

    public class UserSuggestionModel
    {
        public string Suggestion { get; set; } = "";
    }
}
