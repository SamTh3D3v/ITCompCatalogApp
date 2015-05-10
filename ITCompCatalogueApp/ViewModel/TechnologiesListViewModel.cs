using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ITCompCatalogueApp.Model;

namespace ITCompCatalogueApp.ViewModel
{

    public class TechnologiesListViewModel : ViewModelBase
    {
        #region Consts
        public const string ListTechnologiesPropertyName = "ListTechnologies";
        public const string SelectedTechnologyPropertyName = "SelectedTechnology";
        public const string BusyIndicatorPropertyName = "BusyIndicator";
        public const string SearchItemsPropertyName = "SearchItems";
        public const string SearchTextPropertyName = "SearchText";
        #endregion
        #region Fields
        private ObservableCollection<Technology> _listTechnologies;
        private Technology _selectedTechnology;
        private bool _busyIndicator = false;
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private  ITCompTrainingDBEntities1 _dbContext = new ITCompTrainingDBEntities1();
        private string _searchText;
        private ObservableCollection<SearchItem> _searchItems;
        #endregion
        #region Properties
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
                    ListTechnologies = new ObservableCollection<Technology>(_dbContext.Technologies.Where(x =>
                           x.Code.ToLower().Contains(seachText) && code
                        || x.Intitule.ToLower().Contains(seachText) && intitule));
                }
                else
                {
                    ListTechnologies = new ObservableCollection<Technology>(_dbContext.Technologies);
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
                            _worker.RunWorkerAsync();
                            Messenger.Default.Send(new NotificationMessage("CloseAddTechnology"));
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
                        if (SelectedTechnology !=null)
                        {
                            if (SelectedTechnology.C_id == -1)
                            {
                                if (!String.IsNullOrEmpty(SelectedTechnology.Code)&& !String.IsNullOrEmpty(SelectedTechnology.Intitule))
                                {
                                    _dbContext.Technologies.Add(SelectedTechnology);
                                }
                                else
                                {
                                    MessageBox.Show("Remplir tous les champs obligatoires, avec des données approprié", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                
                            }

                                
                            _dbContext.SaveChanges(); 
                        }
                        else
                        {
                            SelectedTechnology = new Technology()
                            {
                                C_id = -1
                            };
                            MessageBox.Show("Remplir tous les champs obligatoires, avec des données approprié", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return; 
                        }
                        ListTechnologies = new ObservableCollection<Technology>(_dbContext.Technologies);
                        Messenger.Default.Send(new NotificationMessage("CloseAddTechnology"));
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
                    () => Messenger.Default.Send(new NotificationMessage("AddTechnology"))));
            }
        }
        private RelayCommand _modifierTechnologyCommand;
        public RelayCommand ModifierTechnologyCommand
        {
            get
            {
                return _modifierTechnologyCommand
                    ?? (_modifierTechnologyCommand = new RelayCommand(
                    () => Messenger.Default.Send(new NotificationMessage("ModifierTechnology"))));
            }
        }
        private RelayCommand _supprimerTechnologyCommand;
        public RelayCommand SupprimerTechnologyCommand
        {
            get
            {
                return _supprimerTechnologyCommand
                    ?? (_supprimerTechnologyCommand = new RelayCommand(
                    () => Messenger.Default.Send(new NotificationMessage("DeleteTechnology"))));
            }
        }
        private RelayCommand _technologiesListPageLoadedCommand;
        public RelayCommand TechnologiesListPageLoadedCommand
        {
            get
            {
                return _technologiesListPageLoadedCommand
                    ?? (_technologiesListPageLoadedCommand = new RelayCommand(
                    () =>
                    {
                         _dbContext=new ITCompTrainingDBEntities1();
                        _worker.RunWorkerAsync();
                    }));
            }
        }
        #endregion
        #region Ctos and Methods
        public TechnologiesListViewModel()
        {
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "NewTechnology":

                        SelectedTechnology = new Technology()
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
                    case "DeleteTechnology":
                        if (SelectedTechnology != null)
                        {
                            var mres = MessageBox.Show("Supprimer Technology", "Vous etes Sur ?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (mres == MessageBoxResult.Yes)
                            {
                                _dbContext.Technologies.Remove(_dbContext.Technologies.Find(SelectedTechnology.C_id));
                                _dbContext.SaveChanges();
                                ListTechnologies = new ObservableCollection<Technology>(_dbContext.Technologies);
                            }
                        }
                        else
                        {
                            var mres = MessageBox.Show("Selectionner un technology a supprimer ?", "Supprimer Technology", MessageBoxButton.OK, MessageBoxImage.Error);
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
                }
                
            };
            foreach (var item in SearchItems)    // * Needs better Solution 
            {
                item.PropertyChanged += UpdateSearchTerms;
            }
            _worker.DoWork += LoadTechnologies;
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
            ListTechnologies = new ObservableCollection<Technology>((IEnumerable<Technology>)e.Result);
        }
        private void LoadTechnologies(object sender, DoWorkEventArgs e)
        {
            BusyIndicator = true;
            e.Result = _dbContext.Technologies;
        }
        #endregion
    }
}