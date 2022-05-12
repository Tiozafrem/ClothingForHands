using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClothingForHands.Helper
{
    public class MaterialHelper : ModelHelper
    {
        MsgBoxHelper msgBoxHelper = new MsgBoxHelper();
        #region Get
        public List<Model.Material> GetMaterial()
        {
            List<Model.Material> lstMaterial;
            try
            {
                lstMaterial = context.Material.ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                msgBoxHelper.Error("Ошибка подключения к базе данных");
                lstMaterial = new List<Model.Material>();
            }
            catch (Exception ex)
            {
                msgBoxHelper.Error(ex);
                lstMaterial = new List<Model.Material>();
            }
            return lstMaterial;
        }

        public List<Model.Material> GetMaterial(string find)
        {
            List<Model.Material> lstMaterial;
            try
            {
                lstMaterial = context.Material.Where(i => i.NameMaterial.Contains(find) || i.Desctiption.Contains(find)).ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                msgBoxHelper.Error("Ошибка подключения к базе данных");
                lstMaterial = new List<Model.Material>();
            }
            catch (Exception ex)
            {
                msgBoxHelper.Error(ex);
                lstMaterial = new List<Model.Material>();
            }
            return lstMaterial;
        }

        public List<Model.Material> GetMaterial(int typeMaterialId)
        {
            List<Model.Material> lstMaterial;
            try
            {
                lstMaterial = context.Material.Where(i => i.TypeMaterial.Id == typeMaterialId).ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                msgBoxHelper.Error("Ошибка подключения к базе данных");
                lstMaterial = new List<Model.Material>();
            }
            catch (Exception ex)
            {
                msgBoxHelper.Error(ex);
                lstMaterial = new List<Model.Material>();
            }
            return lstMaterial;
        }

        public List<Model.Material> GetMaterial(string find, int typeMaterialId)
        {
            List<Model.Material> lstMaterial;
            try
            {
                lstMaterial = context.Material.Where(i => (i.Desctiption.Contains(find) || i.NameMaterial.Contains(find)) && i.TypeMaterial.Id == typeMaterialId).ToList();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                msgBoxHelper.Error("Ошибка подключения к базе данных");
                lstMaterial = new List<Model.Material>();
            }
            catch (Exception ex)
            {
                msgBoxHelper.Error(ex);
                lstMaterial = new List<Model.Material>();
            }
            return lstMaterial;
        }

        public List<Model.Supplier> GetSuppliers(object material)
        {
            List<Model.Supplier> lstSupplier = new List<Model.Supplier>();
            try
            {
                var buff = ((Model.Material)material).MaterialBySupplier;
                foreach (var item in buff)
                {
                    lstSupplier.Add(item.Supplier);

                }
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
        #endregion


        #region Sort
        public void SortPriceAsc(List<Model.Material> materials)
        {
            materials.Sort((x, y) => x.Price.CompareTo(y.Price));
        }

        public void SortPriceDesc(List<Model.Material> materials)
        {
            materials.Sort((x, y) => y.Price.CompareTo(x.Price));
        }

        public void SortNameAsc(List<Model.Material> materials)
        {
            materials.Sort((x, y) => x.NameMaterial.CompareTo(y.NameMaterial));
        }

        public void SortNameDesc(List<Model.Material> materials)
        {
            materials.Sort((x, y) => y.NameMaterial.CompareTo(x.NameMaterial));
        }

        public void SortCountAsc(List<Model.Material> materials)
        {
            materials.Sort((x, y) => x.Count.CompareTo(y.Count));
        }

        public void SortCountDesc(List<Model.Material> materials)
        {
            materials.Sort((x, y) => y.Count.CompareTo(x.Count));
        }

        #endregion

        #region MInNumber
        public int MaxMinNumber(IList materials)
        {
            var buff = new List<Model.Material>();
            foreach (Model.Material item in materials)
            {
                buff.Add(item);
            }
            if (buff.Count > 0)
                return buff.Max(i => i.MinCount);
            return 0;
        }
        public bool ChangeMinNumber(IList materials, int minNumber)
        {
            var buff = new List<Model.Material>();
            foreach (Model.Material item in materials)
            {
                item.MinCount = minNumber;
            }
            return Save();
        }

        #endregion
        public List<Model.Material> Listing(List<Model.Material> materials, int startNumber, int count = 15)
        {
            List<Model.Material> buff = new List<Model.Material>();
            for (int i = 0; startNumber < materials.Count; startNumber++)
            {

                if (i >= count)
                    break;

                buff.Add(materials[startNumber]);
                i++;
            }
            return buff;
        }

        public bool DeleteMaterial(Model.Material material)
        {
            context.Material.Remove(material);
            return Save();
        }

        public bool DeleteMaterial(object material)
        {
            context.Material.Remove((Model.Material)material);
            return Save();
        }


        public string NewImage(OpenFileDialog openFileDialog)
        {
            changesMaterial.Photo = File.ReadAllBytes(openFileDialog.FileName);
            //string path = Assembly.GetExecutingAssembly().Location + @"materials\" + System.IO.Path.GetFileName(openFileDialog.FileName);
            //File.Copy(openFileDialog.FileName, @"123");
            //changesMaterial.image = path;
            return openFileDialog.FileName;
        }

        public bool SaveMaterial(object material)
        {
            if (material is Model.Material currentMaterial)
            {
                if (currentMaterial.Id == 0)
                    context.Material.Add(currentMaterial);
            }
            return Save();
        }

        public bool SaveMaterial(object material, IEnumerable supplier)
        {
            if (material is Model.Material curentMaterial && supplier is List<Model.Supplier> currentSupplier)
            {
                context.MaterialBySupplier.RemoveRange(curentMaterial.MaterialBySupplier);
                foreach (var item in currentSupplier)
                {
                    Model.Supplier buffSupplier = context.Supplier.Where(i => i.Id == item.Id).FirstOrDefault();
                    if (curentMaterial.MaterialBySupplier.Where(i => i.IdSupplier == item.Id).Count() > 1)
                    {
                        msgBoxHelper.Error("Есть дублирующиеся возможные поставщики.");
                        return false;
                    }
                    curentMaterial.MaterialBySupplier.Add(new Model.MaterialBySupplier()
                    {
                        IdMaterial = curentMaterial.Id,
                        Material = curentMaterial,
                        IdSupplier = item.Id,
                    });
                }
                return SaveMaterial(material);
            }
            return false;
        }
        public List<Model.Material> Materials;

        private Model.Material changesMaterial = new Model.Material();
        public object ChangesMaterial
        {
            get
            {
                return changesMaterial;
            }
            set
            {
                changesMaterial = (Model.Material)value;
            }
        }



    }
}
