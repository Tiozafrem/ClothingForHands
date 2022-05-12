using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingForHands.Helper
{
    class SupplierHelper : ModelHelper
    {
        MsgBoxHelper msgBoxHelper = new MsgBoxHelper();
        public List<Model.Supplier> GetSuppliers()
        {
            List<Model.Supplier> lstSupplier;
            try
            {
                lstSupplier = context.Supplier.ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                msgBoxHelper.Error("Ошибка подключения к базе данных");
                lstSupplier = new List<Model.Supplier>();
            }
            catch (Exception ex)
            {
                lstSupplier = new List<Model.Supplier>();
                msgBoxHelper.Error(ex);
            }
            return lstSupplier;
        }

    }
}
