using System;
using System.Diagnostics;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ITCompCatalogueApp.Model;
using Microsoft.Win32;

namespace ITCompCatalogueApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Consts
        public const string FrameSourcePropertyName = "MainFrameSource";
        public const string ListerTechTbPropertyName = "ListerTechTb";
        public const string ListerCatTbPropertyName = "ListerCatTb";
        public const string ListerCusrTbPropertyName = "ListerCusrTb";
        public const string ListerCourTbPropertyName = "ListerCourTb";
        public const string PlanningTabIsSelectedPropertyName = "PlanningTabIsSelected";
        public const string IsFilterVisiblePropertyName = "IsFilterVisible";
        #endregion
        #region Fields
        private Uri _mainFrameSource;
        private bool _listerCurTb = false;
        private bool _listerCoursTb = false;
        private bool _listerTechTb = false;
        private bool _ListerCatTb = false;
        private bool _planningTabIsSelected;
        private bool _isFilterVisible;
        #endregion
        #region Properties
        public Uri MainFrameSource
        {
            get
            {
                return _mainFrameSource;
            }

            set
            {
                if (_mainFrameSource == value)
                {
                    return;
                }

                _mainFrameSource = value;
                RaisePropertyChanged(FrameSourcePropertyName);
            }
        }
        public bool ListerCourTb
        {
            get
            {
                return _listerCoursTb;
            }

            set
            {
                if (_listerCoursTb == value)
                {
                    return;
                }

                _listerCoursTb = value;
                RaisePropertyChanged(ListerCourTbPropertyName);
                if (_listerCoursTb)
                {
                    ListerCusrTb = false; ListerTechTb = false; ListerCatTb = false;
                }
            }
        }     
        public bool ListerTechTb
        {
            get
            {
                return _listerTechTb;
            }

            set
            {
                if (_listerTechTb == value)
                {
                    return;
                }

                _listerTechTb = value;
                RaisePropertyChanged(ListerTechTbPropertyName);
                if (_listerTechTb)
                {
                    ListerCusrTb = false; ListerCourTb = false; ListerCatTb = false;
                }
            }
        }    
        public bool ListerCatTb
        {
            get
            {
                return _ListerCatTb;
            }

            set
            {
                if (_ListerCatTb == value)
                {
                    return;
                }

                _ListerCatTb = value;
                RaisePropertyChanged(ListerCatTbPropertyName);
                if (_ListerCatTb)
                {
                    ListerCusrTb = false; ListerCourTb = false; ListerTechTb = false;
                }
            }
        }
        public bool ListerCusrTb
        {
            get
            {
                return _listerCurTb;
            }

            set
            {
                if (_listerCurTb == value)
                {
                    return;
                }

                _listerCurTb = value;
                RaisePropertyChanged(ListerCusrTbPropertyName);
                if (_listerCurTb)
                {
                    ListerCatTb = false; ListerCourTb = false; ListerTechTb = false;
                }
            }
        }
        public bool PlanningTabIsSelected
        {
            get
            {
                return _planningTabIsSelected;
            }

            set
            {
                if (_planningTabIsSelected == value)
                {
                    return;
                }

                _planningTabIsSelected = value;
                RaisePropertyChanged(PlanningTabIsSelectedPropertyName);
                if(_planningTabIsSelected)
                    MainFrameSource = new Uri("/../View/PlanningView.xaml", UriKind.Relative);

            }
        }
        public bool IsFilterVisible
        {
            get
            {
                return _isFilterVisible;
            }

            set
            {
                if (_isFilterVisible == value)
                {
                    return;
                }

                _isFilterVisible = value;
                RaisePropertyChanged(IsFilterVisiblePropertyName);
                if (_isFilterVisible)
                {
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage("ShowFilter"));
                }
                else
                {
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage("HideFilter"));
                }
            }
        }
        
        #endregion
        #region Commands
        private RelayCommand _listCoucesCommand;
        public RelayCommand ListCoucesCommand
        {
            get
            {
                return _listTechnologiesCommand
                    ?? (_listTechnologiesCommand = new RelayCommand(
                    () =>
                    {
                        MainFrameSource = new Uri("/../View/CoursesListView.xaml",UriKind.Relative);
                    }));
            }
        }
        private RelayCommand _ajouterCourseCommand;
        public RelayCommand AjouterCourseCommand
        {
            get
            {
                return _ajouterCourseCommand
                    ?? (_ajouterCourseCommand = new RelayCommand(
                    () =>
                    {
                        ListerCourTb = true;
                        MainFrameSource = new Uri("/../View/CoursesListView.xaml",UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("AddCourse"));
                        
                    }));
            }
        }
        private RelayCommand _modifierCourseCommand;
        public RelayCommand ModifierCourseCommand
        {
            get
            {
                return _modifierCourseCommand
                    ?? (_modifierCourseCommand = new RelayCommand(
                    () =>
                    {
                        ListerCourTb = true;
                        MainFrameSource = new Uri("/../View/CoursesListView.xaml",UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("ModifierCourse"));
                        

                    }));
            }
        }
        private RelayCommand _supprimerCourseCommand;
        public RelayCommand SupprimerCourseCommand
        {
            get
            {
                return _supprimerCourseCommand
                    ?? (_supprimerCourseCommand = new RelayCommand(
                    () =>
                    {
                        ListerCourTb = true;
                        MainFrameSource = new Uri("/../View/CoursesListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("DeleteCourse"));
                        
                    }));
            }
        }
        private RelayCommand _listTechnologiesCommand;
        public RelayCommand ListTechnologiesCommand
        {
            get
            {
                return _listCoucesCommand
                    ?? (_listCoucesCommand = new RelayCommand(
                    () =>
                    {
                        MainFrameSource = new Uri("/../View/TechnologiesListView.xaml", UriKind.Relative);
                    }));
            }
        }
        private RelayCommand _ajouterTechnologyCommand;
        public RelayCommand AjouterTechnologyCommand
        {
            get
            {
                return _ajouterTechnologyCommand
                    ?? (_ajouterTechnologyCommand = new RelayCommand(
                    () =>
                    {
                        ListerTechTb = true;
                        MainFrameSource = new Uri("/../View/TechnologiesListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("AddTechnology"));
                        
                    }));
            }
        }
        private RelayCommand _modifierTechnologyCommand;
        public RelayCommand ModifierTechnologyCommand
        {
            get
            {
                return _modifierTechnologyCommand
                    ?? (_modifierTechnologyCommand = new RelayCommand(
                    () =>
                    {
                        ListerTechTb = true;
                        MainFrameSource = new Uri("/../View/TechnologiesListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("ModifierTechnology"));
                        
                    }));
            }
        }
        private RelayCommand _supprimerTechnologyCommand;
        public RelayCommand SupprimerTechnologyCommand
        {
            get
            {
                return _supprimerTechnologyCommand
                    ?? (_supprimerTechnologyCommand = new RelayCommand(
                    () =>
                    {
                        ListerTechTb = true;
                        MainFrameSource = new Uri("/../View/TechnologiesListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("DeleteTechnology"));
                        
                    }));
            }
        }
        private RelayCommand _listCategoriesCommand;
        public RelayCommand ListCategoriesCommand
        {
            get
            {
                return _listCategoriesCommand
                    ?? (_listCategoriesCommand = new RelayCommand(
                    () =>
                    {
                        MainFrameSource = new Uri("/../View/CategoriesListView.xaml", UriKind.Relative);
                    }));
            }
        }
        private RelayCommand _ajouterCategoryCommand;
        public RelayCommand AjouterCategoryCommand
        {
            get
            {
                return _ajouterCategoryCommand
                    ?? (_ajouterCategoryCommand = new RelayCommand(
                    () =>
                    {
                        ListerCatTb = true;
                        MainFrameSource = new Uri("/../View/CategoriesListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("AddCategory"));
                        
                    }));
            }
        }
        private RelayCommand _modifierCategoryCommand;
        public RelayCommand ModifierCategoryCommand
        {
            get
            {
                return _modifierCategoryCommand
                    ?? (_modifierCategoryCommand = new RelayCommand(
                    () =>
                    {
                        ListerCatTb = true;
                        MainFrameSource = new Uri("/../View/CategoriesListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("ModifierCategory"));
                        
                    }));
            }
        }
        private RelayCommand _supprimerCategoryCommand;
        public RelayCommand SupprimerCategoryCommand
        {
            get
            {
                return _supprimerCategoryCommand
                    ?? (_supprimerCategoryCommand = new RelayCommand(
                    () =>
                    {
                        ListerCatTb = true;
                        MainFrameSource = new Uri("/../View/CategoriesListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("DeleteCategory"));
                        
                    }));
            }
        }
        private RelayCommand _listCusrsusCommand;
        public RelayCommand ListCursusCommand
        {
            get
            {
                return _listCusrsusCommand
                    ?? (_listCusrsusCommand = new RelayCommand(
                    () =>
                    {
                        MainFrameSource = new Uri("/../View/CursusListView.xaml", UriKind.Relative);
                    }));
            }
        }
        private RelayCommand _ajouterCursusCommand;
        public RelayCommand AjouterCursusCommand
        {
            get
            {
                return _ajouterCursusCommand
                    ?? (_ajouterCursusCommand = new RelayCommand(
                    () =>
                    {
                        ListerCusrTb = true;
                        MainFrameSource = new Uri("/../View/CursusListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("AddCursus"));
                        
                    }));
            }
        }
        private RelayCommand _modifierCursusCommand;
        public RelayCommand ModifierCursusCommand
        {
            get
            {
                return _modifierCursusCommand
                    ?? (_modifierCursusCommand = new RelayCommand(
                    () =>
                    {
                        ListerCusrTb = true;
                        MainFrameSource = new Uri("/../View/CursusListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("ModifierCursus"));
                        
                    }));
            }
        }
        private RelayCommand _supprimerCursusCommand;
        public RelayCommand SupprimerCursusCommand
        {
            get
            {
                return _supprimerCursusCommand
                    ?? (_supprimerCursusCommand = new RelayCommand(
                    () =>
                    {
                        ListerCusrTb = true;
                        MainFrameSource = new Uri("/../View/CursusListView.xaml", UriKind.Relative);
                        Messenger.Default.Send(new NotificationMessage("DeleteCursus"));
                        
                    }));
            }
        }
        private RelayCommand _exportDataBaseCommand;
        public RelayCommand ExportDataBaseCommand
        {
            get
            {
                return _exportDataBaseCommand
                    ?? (_exportDataBaseCommand = new RelayCommand(
                    () =>
                    {
                        var dlg = new SaveFileDialog
                        {
                            DefaultExt = ".db",
                            Filter = "Sqlite database (.db)|*.db",
                            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                        };
                        if (dlg.ShowDialog()==true)
                        {
                            try
                            {
                                string fileName = dlg.FileName;
                                var reader = new StreamReader("Data//ITCompTrainingDB.db");
                                using (var memoryStream = new MemoryStream())
                                {
                                    reader.BaseStream.CopyTo(memoryStream);
                                    File.WriteAllBytes(fileName, memoryStream.ToArray());
                                }
                            }
                            catch (Exception e)
                            {                                
                                Debug.WriteLine("In StreamReader Exception ->"+e.Message);
                            }                          
                        }
                    }));
            }
        }
        private RelayCommand _importDataBaseCommand;
        public RelayCommand ImportDataBaseCommand
        {
            get
            {
                return _importDataBaseCommand
                    ?? (_importDataBaseCommand = new RelayCommand(
                    () =>
                    {
                        var dlg = new OpenFileDialog
                        {
                            DefaultExt = ".db",
                            Filter = "Sqlite database (.db)|*.db",
                            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                        };
                        if (dlg.ShowDialog() == true)
                        {
                            try
                            {
                                var itCompDbPath = "Data//ITCompTrainingDB.db";
                                string fileName = dlg.FileName;
                                var reader = new StreamReader(fileName);
                                using (var memoryStream = new MemoryStream())
                                {
                                    reader.BaseStream.CopyTo(memoryStream);
                                    File.WriteAllBytes(itCompDbPath, memoryStream.ToArray());
                                    
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("In StreamReader Exception ->" + e.Message);
                            }
                        }
                        
                    }));
            }
        }
        private RelayCommand _importDatesCommand;
        public RelayCommand ImportDatesCommand
        {
            get
            {
                return _importDatesCommand
                    ?? (_importDatesCommand = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send(new NotificationMessage("PullChanges"));
                    }));
            }
        }
        private RelayCommand _exportDatesCommand;
        public RelayCommand ExportDatesCommand
        {
            get
            {
                return _exportDatesCommand
                    ?? (_exportDatesCommand = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send(new NotificationMessage("PushChanges"));
                    }));
            }
        }       
        #endregion
        #region Ctos and Methods
        public MainViewModel()
        {
            MainFrameSource = new Uri("/../View/CoursesListView.xaml", UriKind.Relative);
            ListerCourTb = true;
        }
        #endregion
    }
}