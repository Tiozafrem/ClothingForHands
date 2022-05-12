using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingForHands.Model
{
    public partial class Material
    {
        public string Supplier
        {
            get
            {
                string result = String.Empty;
                foreach (var item in MaterialBySupplier)
                {
                    if (result != String.Empty)
                        result += ", ";
                    result += item.Supplier.NameSupplier;
                }
                if (result == String.Empty)
                    result = "Поставщиков нет";
                return result;
            }
        }

        public string ColorType
        {
            get
            {
                if (Count < MinCount)
                    return "#f19292";

                else if (Count > MinCount * 3)
                    return "#ffba01";

                else
                    return "White";


            }
        }

        public string MinimalPrice
        {
            get
            {

                double result = 0;

                if (MinCount > Count)
                {
                    var buff = Math.Ceiling((MinCount - Count) / (double)CountInBox);
                    result = buff * (double)Price;
                }

                return result == 0 ? "закупка не требуется" : result.ToString();
            }
        }

    }
}
