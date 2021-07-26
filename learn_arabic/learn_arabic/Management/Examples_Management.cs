using learn_arabic.Classes;
using learn_arabic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Management
{
    public class Examples_Management
    {
        public async static Task<bool> Add(AddExampleModel example, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "shape_id", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("shape_examples", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("shape_examples", ID);
            }
            object[] valsinput = { example.Shape_ID, DateTime.Now };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("shape_examples", ID, cols, vals, msg))
            {
                //TODO add example Image
                //TODO add example Voice_spelling_normal row index 1
                //TODO add example Voice_spelling_description row index 2
                //TODO add example Voice_spelling_formatting row index 3

                //// add Arabic translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("shape_example_translations", "AR", example.Arabic_Name, ID, msg);


                //// add English translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("shape_example_translations", "EN", example.English_Name, ID, msg);


                //// add Turkish translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("shape_example_translations", "TR", example.Turkish_Name, ID, msg);


                //// add Russian translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("shape_example_translations", "RU", example.Russian_Name, ID, msg);

                return true;
            }
            return false;
        }


        public async static Task<bool> Edit(Guid ID, AddExampleModel example, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "shape_id", "updated_at" };
            cols.AddRange(colsinput);
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("shape_examples", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }
            object[] valsinput = { DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.UpdateRow("shape_examples", ID, cols, vals, msg))
            {

                //TODO add shape Image
                //TODO add shape Voice_spelling_normal
                //TODO add shape Voice_spelling_description
                //TODO add shape Voice_spelling_formatting

                //// edit Arabic translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("shape_example_translations", "AR", example.Arabic_Name, ID, msg);


                //// edit English translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("shape_example_translations", "EN", example.English_Name, ID, msg);


                //// edit Turkish translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("shape_example_translations", "TR", example.Turkish_Name, ID, msg);


                //// edit Russian translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("lesson_shape_shape_example_translationstranslations", "RU", example.Russian_Name, ID, msg);

                return true;
            }
            return false;
        }

        public async static Task<bool> Delete(Guid ID, ER_Ref<string> msg)
        {

            DataRow temp = await Database.GetRow("shape_examples", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }

            return await Database.DeleteRow("shape_examples", ID, msg);

        }

        public async static Task<List<ShowExampleModel>> Get_Examples(string lang, ER_Ref<string> msg)
        {
            List<ShowExampleModel> dtList = new List<ShowExampleModel>();
            string sql = @"select s.id,lt.value ,att.URL as Image,st.value as Name,
                        v1.URL as Voice_spelling_normal,
                        v2.URL as Voice_spelling_description,
                        v3.URL as Voice_spelling_formatting
                        from shape_examples as s
                        inner join lesson_shape_translations as lt
                        on s.lesson_id = lt.src_id and lt.language=@lang
                        inner join attachments as att
                        on s.id = att.Src_ID and att.Src_Type='Image'
                        inner join shape_example_translations as st  
                        on s.id=st.src_id and st.language@lang
                        inner join attachments as v1
                        on s.id=v1.src_id and v1.Src_Type='Voice' and v1.Row_Index =1
                        inner join attachments as v2
                        on s.id=v2.src_id and v2.Src_Type='Voice' and v2.Row_Index =2
                        inner join attachments as v3
                        on s.id=v3.src_id and v3.Src_Type='Voice' and v3.Row_Index =3";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable examples = await Database.ReadTableByQuery(sql, li, msg);
            if (examples != null && examples.Rows.Count > 0)
            {
                dtList = examples.AsEnumerable()
                       .Select(row => new ShowExampleModel
                       {
                           ID = row["id"].ToString(),
                           Shape_Name = row["value"].ToString(),
                           Image_URL = row["Image"].ToString(),
                           Voice_spelling_description_URL = row["Video"].ToString(),
                           Voice_spelling_formatting_URL = row["description"].ToString(),
                           Name = row["Name"].ToString(),
                           Voice_spelling_normal_URL = row["Voice"].ToString(),
                       }).ToList();
            }
            return dtList;
        }
        public async static Task<ExampleModel> Get_Example(Guid ID, ER_Ref<string> msg)
        {
            ExampleModel Example = new ExampleModel();
            string sql = @"select s.id,s.shape_id ,att.URL as Image,
                        ar.value as ar,en.value as en,
                        tr.value as tr,ru.value as ru,
                        v1.URL as Voice_spelling_normal,
                        v2.URL as Voice_spelling_description,
                        v3.URL as Voice_spelling_formatting
                        from shape_examples as s
                        inner join attachments as att
                        on s.id = att.Src_ID and att.Src_Type='Image'
                        inner join shape_example_translations as ar
                        on s.id=ar.src_id and ar.language='AR'
                        inner join shape_example_translations as en
                        on s.id=en.src_id and en.language='EN'
                        inner join shape_example_translations as tr
                        on s.id=tr.src_id and tr.language='TR'
                        inner join shape_example_translations as ru
                        on s.id=ru.src_id and ru.language='RU'
                        inner join attachments as v1
                        on s.id=v1.src_id and v1.Src_Type='Voice' and v1.Row_Index =1
                        inner join attachments as v2
                        on s.id=v2.src_id and v2.Src_Type='Voice' and v2.Row_Index =2
                        inner join attachments as v3
                        on s.id=v3.src_id and v3.Src_Type='Voice' and v3.Row_Index =3
                        where s.id=@id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@id", ID));
            DataTable examples = await Database.ReadTableByQuery(sql, li, msg);
            if (examples != null && examples.Rows.Count > 0)
            {
                DataRow row = examples.Rows[0];
                Example = new ExampleModel()
                {
                    ID = row["id"].ToString(),
                    Shape_ID = new Guid(row["shape_id"].ToString()),
                    Image_URL = row["Image"].ToString(),
                    Arabic_Name = row["ar"].ToString(),
                    English_Name = row["en"].ToString(),
                    Turkish_Name = row["tr"].ToString(),
                    Russian_Name = row["ru"].ToString(),
                    Voice_spelling_description_URL = row["Voice_spelling_description"].ToString(),
                    Voice_spelling_formatting_URL = row["Voice_spelling_formatting"].ToString(),
                    Voice_spelling_normal_URL = row["Voice_spelling_normal"].ToString()
                };
            }
            return Example;
        }

        public async static Task<ShapeExampleModel> Get_shap_examples(string lang, Guid shape_id, ER_Ref<string> msg)
        {
            ShapeExampleModel shape_ex = null;
            string sql = @"select ls.id, lst.value as title, att.URL as Image
                            from lesson_shapes as ls
                            inner join lesson_shape_translations as lst on ls.id = lst.src_id and lst.language = @lang
                            left join attachments att on ls.id = att.src_id and att.Src_ID = 'Image' where id = @shape_id";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@shape_id", shape_id));
            li.Add(new SqlParameter("@lang", lang));
            DataTable examples = await Database.ReadTableByQuery(sql, li, msg);
            if (examples != null && examples.Rows.Count > 0)
            {
                DataRow row = examples.Rows[0];
                shape_ex = new ShapeExampleModel()
                {
                    ShapeName = row["name"].ToString(),
                    ShapImage = row["Image"].ToString(),
                };

                string sql_ = @"select s.id, shet.value as Name , att.URL as Image,
                                v1.URL as Voice_spelling_normal,
                                v2.URL as Voice_spelling_description,
                                v3.URL as Voice_spelling_formatting
                                from shape_examples as s

                                inner join shape_example_translations as shet
                                on s.id = shet.src_id and shet.language =  @lang

                                left join attachments as att
                                on s.id = att.Src_ID and att.Src_Type='Image'

                                left join attachments as v1
                                on s.id = v1.src_id and v1.Src_Type='Voice' and v1.Row_Index = 1

                                left join attachments as v2
                                on s.id = v2.src_id and v2.Src_Type='Voice' and v2.Row_Index = 2

                                left join attachments as v3
                                on s.id = v3.src_id and v3.Src_Type='Voice' and v3.Row_Index = 3
                                where s.shape_id = @shape_id";

                List<SqlParameter> li_ = new List<SqlParameter>();
                li.Add(new SqlParameter("@shape_id", shape_id));
                li.Add(new SqlParameter("@lang", lang));
                DataTable ex_shapes = await Database.ReadTableByQuery(sql_, li_, msg);
                if (ex_shapes != null && ex_shapes.Rows.Count > 0)
                {
                    shape_ex.ExampleShape = ex_shapes.AsEnumerable().Select(row => new ShowExampleModel
                    {
                        ID = row["id"].ToString(),
                        Name = row["Name"].ToString(),
                        Image_URL = row["Image"].ToString(),
                        Voice_spelling_description_URL = row["Voice_spelling_description"].ToString(),
                        Voice_spelling_formatting_URL = row["Voice_spelling_formatting"].ToString(),
                        Voice_spelling_normal_URL = row["Voice_spelling_normal"].ToString()
                    }).ToList();
                }
            }
            return shape_ex;
        }
    }
}