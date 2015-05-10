using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAzure.MobileServices;

namespace ITCompCatalogueApp.ViewModel
{

    public class PlanningViewModel : ViewModelBase
    {
        #region Conts
        public const string ListTechnologiesPropertyName = "ListTechnologies";
        public const string ListCursusPropertyName = "ListCursus";
        public const string ListCategoriesPropertyName = "ListCategories";
        public const string SelectedTechnologyPropertyName = "SelectedTechnology";
        public const string SelectedCursusPropertyName = "SelectedCursus";
        public const string SelectedCategoryPropertyName = "SelectedCategory";
        public const string ListCoursesPropertyName = "ListCourses";
        public const string IsFilterVisiblePropertyName = "IsFilterVisible";
        public const string ListCoursesDatesPropertyName = "ListCoursesDates";
        public const string BusyIndicatorPropertyName = "BusyIndicator";
        #endregion
        #region Fields
        private bool _busyIndicator = false;
        private readonly BackgroundWorker _backgroudWorker=new BackgroundWorker();
        private ITCompTrainingDBEntities1 _dbContext = new ITCompTrainingDBEntities1();
        private ObservableCollection<Technology> _listTechnologies;
        private ObservableCollection<Cursu> _listCursus;
        private ObservableCollection<Category> _listCategories;
        private Technology _selectedTechnology;
        private Cursu _selectedCursus;
        private Category _selectedCategory;
        private ObservableCollection<Cour> _listCoures;
        private Visibility _isFilterVisible=Visibility.Collapsed;
        private ObservableCollection<CourDate> _listCoursesDates;

        #endregion
        #region Properties        
        public bool BusyIndicator
        {
            get
            {
                return _busyIndicator;
            }

            set
            {
                if (_busyIndicator == value)
                {
                    return;
                }

                _busyIndicator = value;
                RaisePropertyChanged(BusyIndicatorPropertyName);
            }
        }
        public ObservableCollection<Technology> ListTechnologies
        {
            get
            {
                return _listTechnologies;
            }

            set
            {
                if (_listTechnologies == value)
                {
                    return;
                }

                _listTechnologies = value;
                RaisePropertyChanged(ListTechnologiesPropertyName);
            }
        }
        public ObservableCollection<Cursu> ListCursus
        {
            get
            {
                return _listCursus;
            }

            set
            {
                if (_listCursus == value)
                {
                    return;
                }

                _listCursus = value;
                RaisePropertyChanged(ListCursusPropertyName);
            }
        }     
        public ObservableCollection<Category> ListCategories
        {
            get
            {
                return _listCategories;
            }

            set
            {
                if (_listCategories == value)
                {
                    return;
                }

                _listCategories = value;
                RaisePropertyChanged(ListCategoriesPropertyName);
            }
        }
        public Technology SelectedTechnology
        {
            get
            {
                return _selectedTechnology;
            }

            set
            {
                if (_selectedTechnology == value)
                {
                    return;
                }

                _selectedTechnology = value;
                RaisePropertyChanged(SelectedTechnologyPropertyName);
            }
        }
        public Cursu SelectedCursus
        {
            get
            {
                return _selectedCursus;
            }

            set
            {
                if (_selectedCursus == value)
                {
                    return;
                }

                _selectedCursus = value;
                RaisePropertyChanged(SelectedCursusPropertyName);
            }
        }
        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }

            set
            {
                if (_selectedCategory == value)
                {
                    return;
                }

                _selectedCategory = value;
                RaisePropertyChanged(SelectedCategoryPropertyName);
            }
        }
        public ObservableCollection<Cour> ListCourses
        {
            get
            {
                return _listCoures;
            }

            set
            {
                if (_listCoures == value)
                {
                    return;
                }

                _listCoures = value;
                RaisePropertyChanged(ListCoursesPropertyName);
            }
        }    
        public Visibility IsFilterVisible
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
            }
        }
        public ObservableCollection<CourDate> ListCoursesDates
        {
            get
            {
                return _listCoursesDates;
            }

            set
            {
                if (_listCoursesDates == value)
                {
                    return;
                }

                _listCoursesDates = value;
                RaisePropertyChanged(ListCoursesDatesPropertyName);
            }
        }
        #endregion
       
        #region Commands
        private RelayCommand _pullChangesCommand;
        public RelayCommand PullChangesCommand
        {
            get
            {
                return _pullChangesCommand
                    ?? (_pullChangesCommand = new RelayCommand(
                    () =>
                    {
                        
                    }));
            }
        }
        private RelayCommand _pushChangesCommand;
        public RelayCommand PushChangesCommand
        {
            get
            {
                return _pushChangesCommand
                    ?? (_pushChangesCommand = new RelayCommand(
                    () =>
                    {
                        
                    }));
            }
        }

        private RelayCommand _planningViewLoadedCommand;
        public RelayCommand PlanningViewLoadedCommand
        {
            get
            {
                return _planningViewLoadedCommand
                    ?? (_planningViewLoadedCommand = new RelayCommand(
                    () =>
                    {
                        
                        _backgroudWorker.RunWorkerAsync();
                    }));
            }
        }
        #endregion
        #region Methods and Ctors
        public PlanningViewModel()
        {

            Messenger.Default.Register<NotificationMessage>(this, (m) =>
            {
                switch (m.Notification)
                {
                    case "ShowFilter":
                        IsFilterVisible = Visibility.Visible;
                        break;
                    case "HideFilter":
                        IsFilterVisible = Visibility.Collapsed;
                        break;
                }
            });
            _backgroudWorker.DoWork += LoadCourses;
            _backgroudWorker.RunWorkerCompleted += LoadingCompleted;

        }

        private void LoadCourses(object sender, DoWorkEventArgs e)
        {
            BusyIndicator = true;
            e.Result=_dbContext.Cours.ToList();            
        }
        private void LoadingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BusyIndicator = false;
            ListCourses=new ObservableCollection<Cour>((List<Cour>) e.Result);
        }


        public void LoadCoursesDatesAsync()
        {
            //Load From The Azur Db Async
        }
       
        #endregion       
    }
}