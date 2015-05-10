using System;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using ITCompCatalogueApp.View;
using ITCompCatalogueApp.ViewModel;

namespace ITCompCatalogueApp
{
    public partial class MainWindow : Window
    {
        #region fields
        private AjouterCourView _ajouterCourView;
        private AjouterCursusView _ajouterCursus;
        private AjouterCategoryView _ajouterCategory;
        private AjouterTechnologyView _ajouterTechnology;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            #region Messages from ViewModels
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "AddCourse":
                        Messenger.Default.Send(new NotificationMessage("NewCourse"));
                        _ajouterCourView = new AjouterCourView();
                        _ajouterCourView.ShowDialog();               
                        break;
                    case "CloseAddCourse":
                        _ajouterCourView.Close();
                        break;
                    case "ModifierCourse":
                        _ajouterCourView=new AjouterCourView();
                        _ajouterCourView.ShowDialog();
                        break;
                    case "AddCursus":
                        Messenger.Default.Send(new NotificationMessage("NewCursus"));
                        _ajouterCursus = new AjouterCursusView();
                        _ajouterCursus.ShowDialog();
                        break;
                    case "CloseAddCursus":
                        _ajouterCursus.Close();
                        break;
                    case "ModifierCursus":
                        _ajouterCursus = new AjouterCursusView();
                        _ajouterCursus.ShowDialog();
                        break;


                    case "AddTechnology":
                        Messenger.Default.Send(new NotificationMessage("NewTechnology"));
                        _ajouterTechnology = new AjouterTechnologyView();
                        _ajouterTechnology.ShowDialog();
                        break;
                    case "CloseAddTechnology":
                        _ajouterTechnology.Close();
                        break;
                    case "ModifierTechnology":
                        _ajouterTechnology = new AjouterTechnologyView();
                        _ajouterTechnology.ShowDialog();
                        break;


                    case "AddCategory":
                        Messenger.Default.Send(new NotificationMessage("NewCategory"));
                       _ajouterCategory = new AjouterCategoryView();
                       _ajouterCategory.ShowDialog();
                        break;
                    case "CloseAddCategory":
                        _ajouterCategory.Close();
                        break;
                    case "ModifierCategory":
                        _ajouterCategory = new AjouterCategoryView();
                        _ajouterCategory.ShowDialog();
                        break;
                }
            });
            #endregion
            
        }

        private void MainFrame_OnSourceUpdated(object sender, EventArgs eventArgs)
        {
            var fileName=MainFrame.Source;
            switch (fileName.ToString())
            {
                case "View/CoursesListView.xaml":
                    ListerCourTb.IsChecked = true;
                    break;
                case "View/CategoriesListView.xaml":
                    ListerCatTb.IsChecked = true;
                    break;
                case "View/CursusListView.xaml":
                    ListerCurTb.IsChecked = true;
                    break;
                case "View/TechnologiesListView.xaml":
                    LIsterTechTb.IsChecked = true;
                    break;
            }
        }
    }
}