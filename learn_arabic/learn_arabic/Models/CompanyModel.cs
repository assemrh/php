using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class ShowCompanyModel
    {
        public string ID { get; set; } = "";
        public string Name { get; set; } = "";
        public string WorkType { get; set; } = "";
        public string Address { get; set; } = "";
        public string Email { get; set; } = "";
    }
    public class CompanyModel
    {
        public string  Company_Name { get; set; } = "";
        public string Work_Type_ID { get; set; } = "";
        public string Country_ID { get; set; } = "";
        public string City { get; set; } = "";
        public string Address_Details { get; set; } = "";
        public string Num_Of_Students { get; set; } = "";
        public string Students_Ages_From { get; set; } = "";
        public string Students_Ages_To { get; set; } = "";
        public string Num_Of_Teachers { get; set; } = "";
        public string Manager_Name { get; set; } = "";
        public DateTime Establish_Date { get; set; } = new DateTime();
        public string License_Number { get; set; } = "";
        public string Communications_Officer_Number { get; set; } = "";
        public string Email { get; set; } = "";
        public string Other_Info { get; set; } = "";
        public Attachment License_Image { get; set; } = new Attachment();
        public List<string> Work_Domains { get; set; } = new List<string>();
        public List<string> Learning_Techniques { get; set; } = new List<string>();


    }
    public class CompanyDetailsModel
    {
        public string Company_Name { get; set; } = "";
        public string Work_Type { get; set; } = "";
        public string Address { get; set; } = "";
        public string Num_Of_Students { get; set; } = "";
        public string Students_Ages_From { get; set; } = "";
        public string Students_Ages_To { get; set; } = "";
        public string Num_Of_Teachers { get; set; } = "";
        public string Manager_Name { get; set; } = "";
        public string Establish_Date { get; set; } = "";
        public string License_Number { get; set; } = "";
        public string Communications_Officer_Number { get; set; } = "";
        public string Email { get; set; } = "";
        public string Other_Info { get; set; } = "";
        public string  License_Image { get; set; } = "";
        public List<string> Work_Domains { get; set; } = new List<string>();
        public List<string> Learning_Techniques { get; set; } = new List<string>();


    }
}
