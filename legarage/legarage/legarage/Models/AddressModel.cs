using System;

namespace legarage.Models
{
    public class AddressModel
    {
        public Guid CountryId { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid AddressId { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string AddressName { get; set; }

        public static string GetAllAddress(Guid Province_id, string Address_name)
        {
            AddressModel address = new AddressModel();
            address.ProvinceId = Province_id;
            address.AddressName = Address_name;
            return address.Country + " / " + address.Province + " / " + address.AddressName;
        }
    }
}