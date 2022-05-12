using System;
using System.Collections.Generic;
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
using ClothingForHands.Helper;

namespace ClothingForHands.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ListMaterial.xaml
    /// </summary>
    public partial class ListMaterial : Window
    {

        private List<string> typeSort = new List<string>
        {
            "По возрастанию: Наименование",
            "По возрастанию: Остаток на складе",
            "По возрастанию: Цена",
            "По убыванию: Наименование",
            "По убыванию: Остаток на складе",
            "По убыванию: Цена"
        };

        private MaterialHelper materialHelper =
            new MaterialHelper();
        private MaterialTypeHelper materialTypeHelper =
            new MaterialTypeHelper();
        private MsgBoxHelper msgBoxHelper =
            new MsgBoxHelper();

        private int listCurrentNumber = 1;
        private List<int> listAllNumber = new List<int>();

        private int cointList = 15;

        public ListMaterial()
        {
            InitializeComponent();
            CmbSort.ItemsSource = typeSort;
            CmbSort.SelectedIndex = 0;
            LoadFilter();
        }

        private void Listing()
        {
            listAllNumber.Clear();
            for (int i = 1; i <= Math.Ceiling(materialHelper.Materials.Count / (double)cointList); i++)
            {
                listAllNumber.Add(i);
            }
            CmbList.ItemsSource = listAllNumber;
            CmbList.SelectedItem = listCurrentNumber;
            var buff = materialHelper.Listing(materialHelper.Materials, (listCurrentNumber * cointList) - cointList);
            LstMaterial.ItemsSource = buff;
            TxtCount.Text = $"{(listCurrentNumber * 15) - 15 + buff.Count} из {materialHelper.Materials.Count}";

        }

        #region Filter
        private void LoadFilter()
        {
            CmbFilter.ItemsSource = materialTypeHelper.GetTypeMaterials("Все типы");
            CmbFilter.SelectedIndex = 0;
        }

        private void LoadFilter(int i)
        {
            CmbFilter.ItemsSource = materialTypeHelper.GetTypeMaterials("Все типы");
            CmbFilter.SelectedIndex = i;
        }
        #endregion

        #region Search
        private void ChangeTextFind(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void ChangeTypeSortFind(object sender, SelectionChangedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            if (CmbFilter == null || CmbSort == null)
                return;

            if (CmbFilter.SelectedIndex < 1 && TxbSearch.Text == "Введите для поиска")
                materialHelper.Materials = materialHelper.GetMaterial();
            else if (CmbFilter.SelectedIndex >= 1 && TxbSearch.Text == "Введите для поиска")
                materialHelper.Materials = materialHelper.GetMaterial(CmbFilter.SelectedIndex);
            else if (CmbFilter.SelectedIndex < 1 && TxbSearch.Text != "Введите для поиска")
                materialHelper.Materials = materialHelper.GetMaterial(TxbSearch.Text);
            else
                materialHelper.Materials = materialHelper.GetMaterial(TxbSearch.Text, CmbFilter.SelectedIndex);

            if ((CmbSort.SelectedItem as string).Contains("По возрастанию:"))
            {
                if ((CmbSort.SelectedItem as string).Contains("Наименование"))
                    materialHelper.SortNameAsc(materialHelper.Materials);
                else if ((CmbSort.SelectedItem as string).Contains("Остаток на складе"))
                    materialHelper.SortCountAsc(materialHelper.Materials);
                else if ((CmbSort.SelectedItem as string).Contains("Цена"))
                    materialHelper.SortPriceAsc(materialHelper.Materials);
            }
            else if ((CmbSort.SelectedItem as string).Contains("По убыванию:"))
            {
                if ((CmbSort.SelectedItem as string).Contains("Наименование"))
                    materialHelper.SortNameDesc(materialHelper.Materials);
                else if ((CmbSort.SelectedItem as string).Contains("Остаток на складе"))
                    materialHelper.SortCountDesc(materialHelper.Materials);
                else if ((CmbSort.SelectedItem as string).Contains("Цена"))
                    materialHelper.SortPriceDesc(materialHelper.Materials);
            }

            Listing();
        }

        #endregion

        #region Focus
        private void TxbSearchLostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TxbSearch.Text))
                TxbSearch.Text = "Введите для поиска";
        }

        private void TxbSearchGotFocus(object sender, RoutedEventArgs e)
        {
            if (TxbSearch.Text == "Введите для поиска")
                TxbSearch.Text = String.Empty;
        }

        #endregion

        #region Navigate
        private void Back(object sender, RoutedEventArgs e)
        {
            if (listCurrentNumber == 1)
                return;
            listCurrentNumber--;
            CmbList.SelectedIndex--;
            Listing();
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            if (listCurrentNumber >= listAllNumber.Max())
                return;
            listCurrentNumber++;
            Listing();
        }

        private void ListChange(object sender, SelectionChangedEventArgs e)
        {
            listCurrentNumber = (int)CmbList.SelectedValue;
            Listing();
        }

        #endregion

        #region MinNumber
        private void ChangeMinNumber(object sender, RoutedEventArgs e)
        {
            ChangeMinNumber changeMinNumber =
                new ChangeMinNumber(materialHelper.MaxMinNumber(LstMaterial.SelectedItems));

            if (changeMinNumber.ShowDialog() == true)
            {
                materialHelper.ChangeMinNumber(LstMaterial.SelectedItems, changeMinNumber.MaxNumber);
                Listing();
            }
            else
            {
                LoadFilter(CmbFilter.SelectedIndex);
            }
        }

        private void ChangeSelectMaterial(object sender, SelectionChangedEventArgs e)
        {
            if (LstMaterial.SelectedItems.Count > 0)
                BtnMinNumber.Visibility = Visibility.Visible;
            else
                BtnMinNumber.Visibility = Visibility.Hidden;
        }

        #endregion

        #region ChangeMaterial
        private void AddMaterial(object sender, RoutedEventArgs e)
        {
            ChangeMaterial changeMaterial = new ChangeMaterial();
            changeMaterial.ShowDialog();
            Listing();
        }

        private void MaterialClick(object sender, MouseButtonEventArgs e)
        {
            if (LstMaterial.SelectedItems.Count == 1)
            {
                materialHelper.ChangesMaterial = LstMaterial.SelectedItem;
                ChangeMaterial changeMaterial = new ChangeMaterial(materialHelper);
                if (changeMaterial.ShowDialog() != true)
                {
                    materialHelper.RollBack();
                    LoadFilter(CmbFilter.SelectedIndex);

                }
                Listing();

            }

        }

        #endregion

        private void Exit(object sender, RoutedEventArgs e)
        {
            if (msgBoxHelper.Question("Вы действительно хотите выйти?"))
                Environment.Exit(1);
        }
    }
}
