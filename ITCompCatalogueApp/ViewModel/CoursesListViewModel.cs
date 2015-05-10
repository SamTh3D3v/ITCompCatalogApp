using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ITCompCatalogueApp.Model;

namespace ITCompCatalogueApp.ViewModel
{
    public class CoursesListViewModel : ViewModelBase
    {
        #region Consts
        public const string ListCoursesPropertyName = "ListCourses";
        public const string SelectedCoursePropertyName = "SelectedCourse";
        public const string BusyIndicatorPropertyName = "BusyIndicator";
        public const string ListCategoriesPropertyName = "ListCategories";
        public const string SearchItemsPropertyName = "SearchItems";
        public const string SearchTextPropertyName = "SearchText";
        #endregion
        #region Fields
        private ObservableCollection<Cour> _listCourses;
        private Cour _selectedCourse;
        private  ITCompTrainingDBEntities1 _dbContext = new ITCompTrainingDBEntities1();
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private bool _busyIndicator = false;
        private ObservableCollection<Category> _listCategories;
        private string _searchText;
        private ObservableCollection<SearchItem> _searchItems;

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
        public ObservableCollection<Cour> ListCourses
        {
            get
            {
                return _listCourses;
            }

            set
            {
                if (_listCourses == value)
                {
                    return;
                }

                _listCourses = value;
                RaisePropertyChanged(ListCoursesPropertyName);
            }
        }
        public Cour SelectedCourse
        {
            get
            {
                return _selectedCourse;
            }

            set
            {
                if (_selectedCourse == value)
                {
                    return;
                }

                _selectedCourse = value;
                RaisePropertyChanged(SelectedCoursePropertyName);
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
        public ObservableCollection<SearchItem> SearchItems
        {
            get
            {
                return _searchItems;
            }

            set
            {
                if (_searchItems == value)
                {
                    return;
                }

                _searchItems = value;
                RaisePropertyChanged(SearchItemsPropertyName);
            }
        }
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                //*Dirty Code* Need Optimisation
                var seachText = _searchText.ToLower();
                RaisePropertyChanged(SearchTextPropertyName);
                if (seachText != "")
                {
                    var code = SearchItems[0].IsSelected;
                    var intitule = SearchItems[1].IsSelected;
                    var duree = SearchItems[2].IsSelected;
                    var niveau = SearchItems[3].IsSelected;
                    var annee = SearchItems[4].IsSelected;
                    var description = SearchItems[5].IsSelected;
                    var categorie = SearchItems[6].IsSelected;
                    var technology = SearchItems[7].IsSelected;
                    ListCourses = new ObservableCollection<Cour>(_dbContext.Cours.Where(x =>
                           x.Code.ToLower().Contains(seachText) && code
                        || x.Intitule.ToLower().Contains(seachText) && intitule
                        || x.Duree.ToLower().Contains(seachText) && duree
                        || x.Niveau.ToLower().Contains(seachText) && niveau
                        || x.Annee.ToLower().Contains(seachText) && annee
                        || x.Description.ToLower().Contains(seachText) && description
                        || x.Category.Intitule.ToLower().Contains(seachText) && categorie
                        || x.Category.Technology.Intitule.ToLower().Contains(seachText) && technology));
                }
                else
                {
                    ListCourses = new ObservableCollection<Cour>(_dbContext.Cours);
                }
                //*Dirty Code* Need Optimisation

            }
        }
        #endregion
        #region Commands
        private RelayCommand _closeWindowCommand;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand
                    ?? (_closeWindowCommand = new RelayCommand(
                        () =>
                        {
                            _dbContext=new ITCompTrainingDBEntities1();
                            ListCourses=new ObservableCollection<Cour>(_dbContext.Cours);
                            Messenger.Default.Send(new NotificationMessage("CloseAddCourse"));
                        }));
            }
        }
        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                    ?? (_saveCommand = new RelayCommand(
                    () =>
                    {
                        if (SelectedCourse != null)
                        {
                            if (SelectedCourse.C_id == -1)
                            {
                                if (!String.IsNullOrEmpty(SelectedCourse.Code) &&
                                  !String.IsNullOrEmpty(SelectedCourse.Intitule) &&
                                  SelectedCourse.Duree != null &&
                                   SelectedCourse.Category != null &&
                                SelectedCourse.Annee != null)
                                {
                                    _dbContext.Cours.Add(SelectedCourse);
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Remplir tous les champs obligatoires, avec des données approprié", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                            }

                            _dbContext.SaveChanges();
                            ListCourses = new ObservableCollection<Cour>(_dbContext.Cours);

                        }
                        else
                        {
                            SelectedCourse = new Cour()
                            {
                                C_id = -1
                            };
                            MessageBox.Show("Remplir tous les champs obligatoires, avec des données approprié", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        Messenger.Default.Send(new NotificationMessage("CloseAddCourse"));
                    }));
            }
        }
        private RelayCommand _addWindowLoadedCommnd;
        public RelayCommand AddWindowLoadedCommand
        {
            get
            {
                return _addWindowLoadedCommnd
                    ?? (_addWindowLoadedCommnd = new RelayCommand(
                    () =>
                    {
                        ListCategories = new ObservableCollection<Category>(_dbContext.Categories);
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
                    () => Messenger.Default.Send(new NotificationMessage("AddCourse"))));
            }
        }
        private RelayCommand _modifierCourseCommand;
        public RelayCommand ModifierCourseCommand
        {
            get
            {
                return _modifierCourseCommand
                    ?? (_modifierCourseCommand = new RelayCommand(
                        () => Messenger.Default.Send(new NotificationMessage("ModifierCourse"))));
            }
        }

        private RelayCommand _supprimerCourseCommand;
        public RelayCommand SupprimerCourseCommand
        {
            get
            {
                return _supprimerCourseCommand
                    ?? (_supprimerCourseCommand = new RelayCommand(
                    () => Messenger.Default.Send(new NotificationMessage("DeleteCourse"))));
            }
        }
        #endregion
        #region Ctos and Methods
        public CoursesListViewModel()
        {
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "NewCourse":
                        SelectedCourse = new Cour()
                        {
                            C_id = -1
                        };
                        break;
                }
            });
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "DeleteCourse":
                        if (SelectedCourse != null)
                        {
                            var mres = MessageBox.Show("Supprimer Cour", "Vous etes Sur ?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (mres == MessageBoxResult.Yes)
                            {
                                _dbContext.Cours.Remove(_dbContext.Cours.Find(SelectedCourse.C_id));
                                _dbContext.SaveChanges();
                                ListCourses = new ObservableCollection<Cour>(_dbContext.Cours);
                            }
                        }
                        else
                        {
                            var mres = MessageBox.Show("Selectionner un cour a supprimer ?", "Supprimer Cour", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                }
            });
            SearchItems = new ObservableCollection<SearchItem>()
            {
                new SearchItem()
                {
                    IsSelected = true,
                    Name = "Code"
                },
                new SearchItem()
                {
                    IsSelected = true,
                    Name = "Intitule"
                },
                new SearchItem()
                {
                    IsSelected = true,
                    Name = "Duree"
                },
                new SearchItem()
                {
                    IsSelected = true,
                    Name = "Niveau"
                },
                new SearchItem()
                {
                    IsSelected = true,
                    Name = "Annee"
                },
                new SearchItem()
                {
                    IsSelected = true,
                    Name = "Description"
                },
                new SearchItem()
                {
                    IsSelected = true,
                    Name = "Categorie"
                }
                ,
                new SearchItem()
                {
                    IsSelected = true,
                    Name = "Technology"
                }
         };
            foreach (var item in SearchItems)    //DirtyCode * Needs Optimisation 
            {
                item.PropertyChanged += UpdateSearchTerms;
            }
            _worker.DoWork += LoadCourses;
            _worker.RunWorkerCompleted += LoadCoursesCompleted;
            _worker.RunWorkerAsync();

        }

        private void UpdateSearchTerms(object sender, PropertyChangedEventArgs e)
        {
            if (_searchText == null) return;
            SearchText = _searchText;
        }

        private void LoadCoursesCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BusyIndicator = false;
            ListCourses = new ObservableCollection<Cour>((IEnumerable<Cour>)e.Result);
        }

        private void LoadCourses(object sender, DoWorkEventArgs e)
        {
            BusyIndicator = true;
            e.Result = _dbContext.Cours;
        }




        #endregion
    }
}