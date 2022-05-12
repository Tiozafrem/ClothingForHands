using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingForHands.Helper
{
    class TypeMetricHelper : ModelHelper
    {
        private MsgBoxHelper msgBoxHelper = new MsgBoxHelper();
        public List<Model.TypeDimennsion> GetTypeMetric()
        {
            List<Model.TypeDimennsion> lstMetricMaterial;
            try
            {
                lstMetricMaterial = context.TypeDimennsion.ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                msgBoxHelper.Error("Ошибка подключения к базе данных");
                lstMetricMaterial = new List<Model.TypeDimennsion>();
            }
            catch (Exception ex)
            {
                msgBoxHelper.Error(ex);
                lstMetricMaterial = new List<Model.TypeDimennsion>();
            }
            return lstMetricMaterial;
        }
    }
}
