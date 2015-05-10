using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ITCompCatalogueApp.Model;

namespace ITCompCatalogueApp.ViewModel
{

    public class CategoriesListViewModel : ViewModelBase
    {
        #region consts
        public const string ListCategoriesPropertyName = "ListCategories";
        public const string SelectedCategoryPropertyName = "SelectedCategory";
        public const string BusyIndicatorPropertyName = "BusyIndicator";
        public const string SearchItemsPropertyName = "SearchItems";
        public const string SearchTextPropertyName = "SearchText";
        public const string ListTechnologiesPropertyName = "ListTechnologies";
        #endregion
        #region Fields
        private ObservableCollection<Category> _listCategories;
        private Category _selectedCategory;
        private bool _busyIndicator = false;
        private  ITCompTrainingDBEntities1 _dbContext = new ITCompTrainingDBEntities1();
        private BackgroundWorker _worker = new BackgroundWorker();
        private string _searchText;
        private ObservableCollection<SearchItem> _searchItems;
        private ObservableCollection<Technology> _listTechnologies;
        #endregion
        #region Properties
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
                    var technology = SearchItems[2].IsSelected;

                    ListCategories = new ObservableCollection<Category>(_dbContext.Categories.Where(x =>
                           x.Code.ToLower().Contains(seachText) && code
                        || x.Intitule.ToLower().Contains(seachText) && intitule
                        || x.Technology.Intitule.ToLower().Contains(seachText) && technology));
                }
                else
                {
                    ListCategories = new ObservableCollection<Category>(_dbContext.Categories);
                }
                //*Dirty Code* Need Optimisation

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
                            _dbContext = new ITCompTrainingDBEntities1();
                            _worker.RunWorkerAsync();
                            Messenger.Default.Send(new NotificationMessage("CloseAddCategory"));
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
                        if (SelectedCategory != null)
                        {
                            if (SelectedCategory.C_id == -1)
                            {
                                if (!String.IsNullOrEmpty(SelectedCategory.Code) && !String.IsNullOrEmpty(SelectedCategory.Intitule) && SelectedCategory.Technology != null)
                                {
                                    _dbContext.Categories.Add(SelectedCategory);
                                }
                                else
                                {
                                    MessageBox.Show("Remplir tous les champs obligatoires, avec des données approprié", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                
                            }

                            _dbContext.SaveChanges();
                            ListCategories = new ObservableCollection<Category>(_dbContext.Categories);
                        }
                        else
                        {
                            SelectedCategory = new Category()
                            {
                                C_id = -1
                            };
                            MessageBox.Show("Remplir tous les champs obligatoires, avec des données approprié", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        Messenger.Default.Send(new NotificationMessage("CloseAddCategory"));

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
                        ListTechnologies = new ObservableCollection<Technology>(_dbContext.Technologies);
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
                    () => Messenger.Default.Send(new NotificationMessage("AddCategory"))));
            }
        }
        private RelayCommand _modifierCategoryCommand;
        public RelayCommand ModifierCategoryCommand
        {
            get
            {
                return _modifierCategoryCommand
                    ?? (_modifierCategoryCommand = new RelayCommand(
                    () => Messenger.Default.Send(new NotificationMessage("ModifierCategory"))));
            }
        }
        private RelayCommand _supprimerCategoryCommand;
        public RelayCommand SupprimerCategoryCommand
        {
            get
            {
                return _supprimerCategoryCommand
                    ?? (_supprimerCategoryCommand = new RelayCommand(
                    () => Messenger.Default.Send(new NotificationMessage("DeleteCategory"))));
            }
        }
        private RelayCommand _categoriesListPageLoadedCommand;
        public RelayCommand CategoriesListPageLoadedCommand
        {
            get
            {
                return _categoriesListPageLoadedCommand
                    ?? (_categoriesListPageLoadedCommand = new RelayCommand(
                    () =>
                    {
                        _dbContext=new ITCompTrainingDBEntities1();
                        _worker.RunWorkerAsync();
                    }));
            }
        }
        #endregion
        #region Ctos and Methods
        public CategoriesListViewModel()
        {

            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "NewCategory":

                        SelectedCategory = new Category()
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
                    case "DeleteCategory":
                        if (SelectedCategory != null)
                        {

                            var mres = MessageBox.Show("Supprimer Category", "Vous etes Sur ?", MessageBoxButton.YesNo,
                                MessageBoxImage.Warning);
                            if (mres == MessageBoxResult.Yes)
                            {
                                _dbContext.Categories.Remove(_dbContext.Categories.Find(SelectedCategory.C_id));
                                _dbContext.SaveChanges();
                                ListCategories = new ObservableCollection<Category>(_dbContext.Categories);
                            }
                        }
                        else
                        {
                            var mres = MessageBox.Show("Selectionner un category a supprimer ?", "Supprimer Category", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    Name = "Technology"
                }
            };
            foreach (var item in SearchItems)    //DirtyCode * Needs Optimisation 
            {
                item.PropertyChanged += UpdateSearchTerms;
            }
            _dbContext = new ITCompTrainingDBEntities1();
            _worker.DoWork += LoadCategories;
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
            ListCategories = new ObservableCollection<Category>((IEnumerable<Category>)e.Result);
        }

        private void LoadCategories(object sender, DoWorkEventArgs e)
        {
            BusyIndicator = true;
            e.Result = _dbContext.Categories;
        }
        #endregion
    }
}