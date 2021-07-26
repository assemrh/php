using System;

namespace legarage.Models
{
    public class SlidersModel
    {
        public Guid ID { get; set; }
        public Guid ReferralID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public int RowOrder { get; set; }
        public string Description { get; set; }
        public ImagesModel Image { get; set; }
        public URLModel URL { get; set; }
        public string ReferralType { get; set; }
    }
}