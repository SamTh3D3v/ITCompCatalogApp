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

    public class CursusListViewModel : ViewModelBase
    {
        #region Consts
        public const string ListCursusPropertyName = "ListCursus";
        public const string SelectedCursusPropertyName = "SelectedCursus";
        public const string BusyIndicatorPropertyName = "BusyIndicator";
        public const string SearchItemsPropertyName = "SearchItems";
        public const string SearchTextPropertyName = "SearchText";
        public const string SelectedCourCursusPropertyName = "SelectedCourCursus";
        public const string ListCoursesPropertyName = "ListCourses";
        public const string CursusCourGbVisibilityPropertyName = "CursusCourGbVisibility";
        public const string CursusCourObservablePropertyName = "CursusCourObservable";
        #endregion
        #region Fields
        private ObservableCollection<Cursu> _listCusrsus;
        private Cursu _selectedCursus;
        private  ITCompTrainingDBEntities1 _dbContext = new ITCompTrainingDBEntities1();
        private bool _busyIndicator = false;
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private string _searchText;
        private ObservableCollection<SearchItem> _searchItems;
        private CursusCour _selectedCursusCour;
        private ObservableCollection<Cour> _listCourses;
        private Visibility _cursusCourGbVisibility = Visibility.Visible;
        private ObservableCollection<CursusCour> _cursusCoursObservable;
        #endregion
        #region Properties
        public ObservableCollection<Cursu> ListCursus
        {
            get
            {
                return _listCusrsus;
            }

            set
            {
                if (_listCusrsus == value)
                {
                    return;
                }

                _listCusrsus = value;
                RaisePropertyChanged(ListCursusPropertyName);
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
                if (SelectedCursus != null)
                    CursusCourObservable = new ObservableCollection<CursusCour>(SelectedCursus.CursusCours);
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

                    ListCursus = new ObservableCollection<Cursu>(_dbContext.Cursus.Where(x =>
                           x.Code.ToLower().Contains(seachText) && code
                        || x.Intitule.ToLower().Contains(seachText) && intitule));
                }
                else
                {
                    ListCursus = new ObservableCollection<Cursu>(_dbContext.Cursus);
                }
                //*Dirty Code* Need Optimisation

            }
        }
        public CursusCour SelectedCourCursus
        {
            get
            {
                return _selectedCursusCour;
            }

            set
            {
                if (_selectedCursusCour == value)
                {
                    return;
                }

                _selectedCursusCour = value;
                RaisePropertyChanged(SelectedCourCursusPropertyName);
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
        public Visibility CursusCourGbVisibility
        {
            get
            {
                return _cursusCourGbVisibility;
            }

            set
            {
                if (_cursusCourGbVisibility == value)
                {
                    return;
                }

                _cursusCourGbVisibility = value;
                RaisePropertyChanged(CursusCourGbVisibilityPropertyName);
            }
        }
        public ObservableCollection<CursusCour> CursusCourObservable
        {
            get
            {
                return _cursusCoursObservable;
            }

            set
            {
                if (_cursusCoursObservable == value)
                {
                    return;
                }

                _cursusCoursObservable = value;
                RaisePropertyChanged(CursusCourObservablePropertyName);
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
                            Messenger.Default.Send(new NotificationMessage("CloseAddCursus"));
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
                        if (SelectedCursus!=null)
                        {
                            if (SelectedCursus.C_id == -1)
                            {
                                if (!String.IsNullOrEmpty(SelectedCursus.Code)&& !String.IsNullOrEmpty(SelectedCursus.Intitule))
                                {
                                    _dbContext.Cursus.Add(SelectedCursus);
                                }
                                else
                                {
                                    MessageBox.Show("Remplir tous les champs obligatoires, avec des données approprié", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                
                            }
                                
                            _dbContext.SaveChanges();
                            ListCursus = new ObservableCollection<Cursu>(_dbContext.Cursus); 
                        }
                        else
                        {
                            SelectedCursus = new Cursu()
                            {
                                C_id = -1
                            };
                            MessageBox.Show("Remplir tous les champs obligatoires, avec des données approprié", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        Messenger.Default.Send(new NotificationMessage("CloseAddCursus"));
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
                        ListCourses = new ObservableCollection<Cour>(_dbContext.Cours);
                    }));
            }
        }
        private RelayCommand _addCursusCourCommand;
        public RelayCommand AddCursusCourCommand
        {
            get
            {
                return _addCursusCourCommand
                    ?? (_addCursusCourCommand = new RelayCommand(
                    () =>
                    {
                        CursusCourGbVisibility = Visibility.Visible;
                        if (SelectedCourCursus == null)
                            SelectedCourCursus = new CursusCour()
                            {
                                C_id = -1
                            };
                        else if (SelectedCourCursus.C_id == -1)
                        {
                            //Some Checking Is Needed In Here 
                            //already In 
                            //check If All Fields Are Filled

                            if (SelectedCourCursus.Cour == null ||
                                String.IsNullOrEmpty(SelectedCourCursus.Recommandation) ||
                                SelectedCourCursus.Ordre==null)
                            {
                                MessageBox.Show("Remplir tous les champs obligatoires, avec des données approprié", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            if (SelectedCursus!=null)
                            {
                                SelectedCursus.CursusCours.Add(SelectedCourCursus);
                                SelectedCourCursus.C_id = Guid.NewGuid().GetHashCode();
                                CursusCourObservable.Add(SelectedCourCursus);   //This is a hack
                                SelectedCourCursus = new CursusCour()
                                {
                                    C_id = -1
                                }; 
                            }
                        }
                        else  //An already Selecetd CursusCour Is In 
                            SelectedCourCursus = new CursusCour()
                            {
                                C_id = -1
                            };

                    }));
            }
        }
        private RelayCommand _deleteCursusCourCommand;
        public RelayCommand DeleteCursusCourCommand
        {
            get
            {
                return _deleteCursusCourCommand
                    ?? (_deleteCursusCourCommand = new RelayCommand(
                    () =>
                    {
                        if (SelectedCourCursus != null)
                        {
                            if (SelectedCourCursus.C_id != -1)
                            {

                                var mres = MessageBox.Show("Supprimer CourCursus", "Vous etes sure ?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                if (mres == MessageBoxResult.Yes)
                                {
                                    _dbContext.CursusCours.Remove(_dbContext.CursusCours.Find(SelectedCourCursus.C_id));
                                    CursusCourObservable.Remove(
                                        CursusCourObservable.First(x => x.C_id == SelectedCourCursus.C_id));
                                }
                            }
                            else
                            {
                                CursusCourObservable.Remove(CursusCourObservable.First(x => x.C_id == SelectedCourCursus.C_id));
                            }
                            SelectedCourCursus = new CursusCour()
                            {
                                C_id = -1
                            };
                        }
                    }));
            }
        }
        private RelayCommand _modifyCursusCourCommand;
        public RelayCommand ModifyCursusCourCommand
        {
            get
            {
                return _modifyCursusCourCommand
                    ?? (_modifyCursusCourCommand = new RelayCommand(
                    () =>
                    {
                        CursusCourGbVisibility = Visibility.Visible;
                    }));
            }
        }
        private RelayCommand _saveCursusCourCommand;
        public RelayCommand SaveCursusCourCommand
        {
            get
            {
                return _saveCursusCourCommand
                    ?? (_saveCursusCourCommand = new RelayCommand(
                    () =>
                    {
                        if (SelectedCourCursus.C_id == -1)
                        {
                            _dbContext.CursusCours.Add(SelectedCourCursus);  //The Id Of The Cursus In The  Selected CourCursus Record Hasn't Been Setted Yet 
                        }

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
                    () => Messenger.Default.Send(new NotificationMessage("AddCursus"))));
            }
        }
        private RelayCommand _modifierCursusCommand;
        public RelayCommand ModifierCursusCommand
        {
            get
            {
                return _modifierCursusCommand
                    ?? (_modifierCursusCommand = new RelayCommand(
                    () => Messenger.Default.Send(new NotificationMessage("ModifierCursus"))));
            }
        }
        private RelayCommand _supprimerCursusCommand;
        public RelayCommand SupprimerCursusCommand
        {
            get
            {
                return _supprimerCursusCommand
                    ?? (_supprimerCursusCommand = new RelayCommand(
                    () => Messenger.Default.Send(new NotificationMessage("DeleteCursus"))));
            }
        }
        private RelayCommand _cursusListPageLoadedCommand;
        public RelayCommand CursusListPageLoadedCommand
        {
            get
            {
                return _cursusListPageLoadedCommand
                    ?? (_cursusListPageLoadedCommand = new RelayCommand(
                        () =>
                        {
                            _dbContext=new ITCompTrainingDBEntities1();
                            _worker.RunWorkerAsync();
                        }));
            }
        }
        #endregion
        #region Ctos and Methods
        public CursusListViewModel()
        {
            SelectedCourCursus = new CursusCour()
            {
                C_id = -1
            };
            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "NewCursus":

                        SelectedCursus = new Cursu()
                        {
                            C_id = -1
                        };
                        SelectedCourCursus = new CursusCour()
                        {
                            C_id = -1
                        };

                        break;
                    case "DeleteCursus":
                        if (SelectedCursus != null)
                        {
                            var mres = MessageBox.Show("Supprimer Cursus", "Vous etes Sur ?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (mres == MessageBoxResult.Yes)
                            {
                                _dbContext.CursusCours.RemoveRange(
                                    _dbContext.CursusCours.Where(x => x.CursusID == SelectedCursus.C_id));
                                _dbContext.Cursus.Remove(_dbContext.Cursus.Find(SelectedCursus.C_id));
                                _dbContext.SaveChanges();
                                ListCursus = new ObservableCollection<Cursu>(_dbContext.Cursus);
                            }
                        }
                        else
                        {
                            var mres = MessageBox.Show("Selectionner un cursus a supprimer ?", "Supprimer Cursus", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    case "ModifierCursus":
                        SelectedCourCursus = new CursusCour()
                        {
                            C_id = -1
                        };
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
            ListCursus = new ObservableCollection<Cursu>((IEnumerable<Cursu>)e.Result);
        }

        private void LoadCourses(object sender, DoWorkEventArgs e)
        {
            BusyIndicator = true;
            e.Result = _dbContext.Cursus;
        }
        #endregion
    }
}