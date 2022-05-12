using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClothingForHands.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChangeMaterial.xaml
    /// </summary>
    public partial class ChangeMaterial : Window
    {
        private Helper.SupplierHelper supplierHelper = new Helper.SupplierHelper();
        private Helper.MaterialHelper materialHelper;
        private Helper.MaterialTypeHelper materialTypeHelper = new Helper.MaterialTypeHelper();
        private Helper.TypeMetricHelper typeMetricHelper = new Helper.TypeMetricHelper();
        private Helper.MsgBoxHelper msgBox = new Helper.MsgBoxHelper();

        #region Load
        public ChangeMaterial(Helper.MaterialHelper materialHelper)
        {
            InitializeComponent();
            if (materialHelper != null)
                this.materialHelper = materialHelper;
            else
                this.materialHelper = new Helper.MaterialHelper();
            LoadData();
        }

        public ChangeMaterial()
        {
            InitializeComponent();
            this.materialHelper = new Helper.MaterialHelper();
            LoadData();
        }

        private void LoadData()
        {
            DataContext = materialHelper.ChangesMaterial;
            CmbType.ItemsSource = materialTypeHelper.GetTypeMaterials();
            CmbMetric.ItemsSource = typeMetricHelper.GetTypeMetric();
            CmbMetric.SelectedIndex = 1;
            CmbType.SelectedIndex = 1;
            DtgSupplier.ItemsSource = materialHelper.GetSuppliers(materialHelper.ChangesMaterial);
            CmbSupplier.ItemsSource = supplierHelper.GetSuppliers();
        }

        #endregion
        private void NewImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".png";
            openFileDialog.Filter = "Изображения | *.png; *.jpg";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                image.Source = new BitmapImage(new Uri(materialHelper.NewImage(openFileDialog)));
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TxbName.Text))
                stringBuilder.Append("\nНазвание не введено.");

            if (TxbName.Text.Length >= 50)
                stringBuilder.Append("\nНазвание слишком длинное.");

            if (!decimal.TryParse(TxbPrice.Text, out decimal price))
                if (!decimal.TryParse(TxbPrice.Text.Replace('.', ','), out price))
                    stringBuilder.Append("\n Цена введена неверно.");

            if (price < 0)
                stringBuilder.Append("\n Цена не может быть меньше нуля");

            if (!int.TryParse(TxbMinNumber.Text, out int minQuantity))
                stringBuilder.Append("\n Минимальное количество на складе введено неверно.");

            if (minQuantity < 1)
                stringBuilder.Append("\n Минимальное количество на складе не может быть меньше нуля");
            if (!int.TryParse(TxbNumberInStorage.Text, out int inStock))
                stringBuilder.Append("\n Количество на складе введено неверно.");

            if (inStock < 1)
                stringBuilder.Append("\n Количество на складе не может быть меньше нуля");

            if (!int.TryParse(TxbNumberInPack.Text, out int inPackage))
                stringBuilder.Append("\n Количество в упаковке введено неверно.");

            if (inPackage < 1)
                stringBuilder.Append("\n Количество в упаковке не может быть меньше нуля");

            if (stringBuilder.Length > 0)
            {
                msgBox.Error($"Не удалось сохранить изменения: {stringBuilder}");
            }
            else
                materialHelper.SaveMaterial(materialHelper.ChangesMaterial, DtgSupplier.ItemsSource);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (materialHelper.ChangesMaterial == null)
                return;
            if (msgBox.Question("Вы точно хотите удалить этот материал?", "Удаление материала"))
            {
                materialHelper.DeleteMaterial(materialHelper.ChangesMaterial);
                this.DialogResult = true;
            }
        }

        private void CanselWindow(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
