using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingForHands.Helper
{
    class MaterialTypeHelper : ModelHelper
    {
        private MsgBoxHelper msgBoxHelper = new MsgBoxHelper();
        public List<Model.TypeMaterial> GetTypeMaterials()
        {
            try
            {
                return context.TypeMaterial.ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                msgBoxHelper.Error("Ошибка подключения к базе данных");
                return new List<Model.TypeMaterial>();
            }
            catch (Exception ex)
            {
                msgBoxHelper.Error(ex);
                return new List<Model.TypeMaterial>();
            }
        }

        public List<Model.TypeMaterial> GetTypeMaterials(string element)
        {
            List<Model.TypeMaterial> typeMaterials;
            try
            {
                typeMaterials = context.TypeMaterial.AsNoTracking().ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                msgBoxHelper.Error("Ошибка подключения к базе данных");
                typeMaterials = new List<Model.TypeMaterial>();
            }
            catch (Exception ex)
            {
                typeMaterials = new List<Model.TypeMaterial>();
                msgBoxHelper.Error(ex);
            }
            typeMaterials.Insert(0, new Model.TypeMaterial
            {
                NameTypeMaterial = element
            });
            return typeMaterials;

        }
    }
}
