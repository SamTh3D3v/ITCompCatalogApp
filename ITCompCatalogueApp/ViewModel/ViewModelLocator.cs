using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using ITCompCatalogueApp.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ITCompCatalogueApp.Model;

namespace ITCompCatalogueApp.ViewModel
{
  
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CoursesListViewModel>();
            SimpleIoc.Default.Register<TechnologiesListViewModel>();
            SimpleIoc.Default.Register<CursusListViewModel>();
            SimpleIoc.Default.Register<CategoriesListViewModel>();
            SimpleIoc.Default.Register<PlanningViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
      
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public CoursesListViewModel CoursesListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CoursesListViewModel>();
            }
        }
 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public TechnologiesListViewModel TechnologiesListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TechnologiesListViewModel>();
            }
        } 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public CursusListViewModel CursusListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CursusListViewModel>();
            }
        }     
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public CategoriesListViewModel CategoriesListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CategoriesListViewModel>();
            }
        }       
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PlanningViewModel PlanningViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlanningViewModel>();
            }
        }
        public static void Cleanup()
        {
        }
    }
}