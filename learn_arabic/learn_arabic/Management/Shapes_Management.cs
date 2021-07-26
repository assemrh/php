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
    public class Shapes_Management
    {
        public async static Task<bool> Add(AddShapeModel shape, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "lesson_id", "created_at" };
            cols.AddRange(colsinput);
            Guid ID = Guid.NewGuid();
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("lesson_shapes", ID);
            while (temp != null)
            {
                ID = Guid.NewGuid();
                temp = await Database.GetRow("lesson_shapes", ID);
            }
            object[] valsinput = { shape.Lesson_ID, DateTime.Now };
            vals.AddRange(valsinput);
            if (await Database.InsertRow("lesson_shapes", ID, cols, vals, msg))
            {
                //TODO add shape Image
                //TODO add shape Voice_spelling_normal row index 1
                //TODO add shape Voice_spelling_description row index 2
                //TODO add shape Voice_spelling_formatting row index 3

                //// add Arabic translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("lesson_shape_translations", "AR", shape.Arabic_Name, ID, msg);


                //// add English translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("lesson_shape_translations", "EN", shape.English_Name, ID, msg);



                //// add Turkish translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("lesson_shape_translations", "TR", shape.Turkish_Name, ID, msg);


                //// add Russian translation
                msg.Error = string.Empty;
                await Database.InsertTranslation("lesson_shape_translations", "RU", shape.Russian_Name, ID, msg);
                return true;
            }
            return false;
        }

        public async static Task<bool> Edit(Guid ID, AddShapeModel shape, ER_Ref<string> msg)
        {
            //// add exam
            List<string> cols = new List<string>();
            List<Object> vals = new List<object>();


            string[] colsinput = { "lesson_id", "updated_at" };
            cols.AddRange(colsinput);
            msg.Error = string.Empty;

            DataRow temp = await Database.GetRow("lesson_shapes", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }
            object[] valsinput = { DateTime.Now.ToShortDateString() };
            vals.AddRange(valsinput);
            if (await Database.UpdateRow("lesson_shapes", ID, cols, vals, msg))
            {

                //TODO add shape Image
                //TODO add shape Voice_spelling_normal
                //TODO add shape Voice_spelling_description
                //TODO add shape Voice_spelling_formatting

                //// edit Arabic translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("lesson_shape_translations", "AR", shape.Arabic_Name, ID, msg);


                //// edit English translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("lesson_shape_translations", "EN", shape.English_Name, ID, msg);


                //// edit Turkish translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("lesson_shape_translations", "TR", shape.Turkish_Name, ID, msg);


                //// edit Russian translation
                msg.Error = string.Empty;
                await Database.UpdateTranslation("lesson_shape_translations", "RU", shape.Russian_Name, ID, msg);

                return true;
            }
            return false;
        }

        public async static Task<bool> Delete(Guid ID, ER_Ref<string> msg)
        {

            DataRow temp = await Database.GetRow("lesson_shapes", ID);
            if (temp == null)
            {
                msg.Error = "No Data Found!";
                return false;
            }

            return await Database.DeleteRow("lesson_shapes", ID, msg);

        }

        public async static Task<List<ShowShapeModel>> Get_Shapes(string lang, ER_Ref<string> msg)
        {
            List<ShowShapeModel> dtList = new List<ShowShapeModel>();
            string sql = @"select s.id,lt.value ,att.URL as Image,st.value as Name,
                        v1.URL as Voice_spelling_normal,
                        v2.URL as Voice_spelling_description,
                        v3.URL as Voice_spelling_formatting
                        from lesson_shapes as s
                        inner join lesson_translations as lt
                        on s.lesson_id = lt.src_id and lt.language=@lang
                        inner join attachments as att
                        on s.id = att.Src_ID and att.Src_Type='Image'
                        inner join lesson_shape_translations as st
                        on s.id=st.src_id and st.language@lang
                        inner join attachments as v1
                        on s.id=v1.src_id and v1.Src_Type='Voice' and v1.Row_Index =1
                        inner join attachments as v2
                        on s.id=v2.src_id and v2.Src_Type='Voice' and v2.Row_Index =2
                        inner join attachments as v3
                        on s.id=v3.src_id and v3.Src_Type='Voice' and v3.Row_Index =3";
            List<SqlParameter> li = new List<SqlParameter>();
            li.Add(new SqlParameter("@lang", lang));
            DataTable shapes = await Database.ReadTableByQuery(sql, li, msg);
            if (shapes != null && shapes.Rows.Count > 0)
            {
                dtList = shapes.AsEnumerable()
                       .Select(row => new ShowShapeModel
                       {
                           ID = row["id"].ToString(),
                           Lesson_Name = row["value"].ToString(),
                           Image_URL = row["Image"].ToString(),
                           Voice_spelling_description_URL = row["Video"].ToString(),
                           Voice_spelling_formatting_URL = row["description"].ToString(),
                           Name = row["Name"].ToString(),
                           Voice_spelling_normal_URL = row["Voice"].ToString(),
                       }).ToList();
            }
            return dtList;
        }
        public async static Task<ShapeModel> Get_Shape(Guid ID, ER_Ref<string> msg)
        {
            ShapeModel shape = new ShapeModel();
            string sql = @"select s.id,s.lesson_id ,att.URL as Image,
                        ar.value as ar,en.value as en,
                        tr.value as tr,ru.value as ru,
                        v1.URL as Voice_spelling_normal,
                        v2.URL as Voice_spelling_description,
                        v3.URL as Voice_spelling_formatting
                        from lesson_shapes as s
                        inner join attachments as att
                        on s.id = att.Src_ID and att.Src_Type='Image'
                        inner join lesson_shape_translations as ar
                        on s.id=ar.src_id and ar.language='AR'
                        inner join lesson_shape_translations as en
                        on s.id=en.src_id and en.language='EN'
                        inner join lesson_shape_translations as tr
                        on s.id=tr.src_id and tr.language='TR'
                        inner join lesson_shape_translations as ru
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
            DataTable shapes = await Database.ReadTableByQuery(sql, li, msg);
            if (shapes != null && shapes.Rows.Count > 0)
            {
                DataRow row = shapes.Rows[0];
                shape = new ShapeModel()
                {
                    ID = row["id"].ToString(),
                    Lesson_ID = new Guid(row["lesson_id"].ToString()),
                    Image_URL = row["Image"].ToString(),
                    Arabic_Name = row["ar"].ToString(),
                    English_Name = row["en"].ToString(),
                    Turkish_Name = row["tr"].ToString(),
                    Russian_Name = row["ru"].ToString(),
                    Voice_spelling_description_URL= row["Voice_spelling_description"].ToString(),
                    Voice_spelling_formatting_URL = row["Voice_spelling_formatting"].ToString(),
                    Voice_spelling_normal_URL = row["Voice_spelling_normal"].ToString()
                };
            }
            return shape;
        }
    }
}
