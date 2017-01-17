using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Caliburn.Micro;
using EasyStocks.App.Droid.Platform;
using EasyStocks.Setup;
using EasyStocks.ViewModel;

namespace EasyStocks.App.Droid
{
    [Application]
    public class AndroidApplication : CaliburnApplication
    {
        private SimpleContainer _container;
        private BootstrapperHelper _bootstrapper;

        public AndroidApplication(
            IntPtr javaReference, 
            JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();
            _bootstrapper = new AndroidBootstrapperHelper(_container);
            _bootstrapper.SetupContainer();
            _container.Instance(_container);
            _container.Instance(_bootstrapper);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[]
            {
             GetType().Assembly,
             // this is important otherwise the views will not be found
             // because the views are not in this assembly but in the platform independent one
             typeof (App).Assembly,
             typeof(MainViewModel).Assembly
         };
        }

        protected override object GetInstance(Type service, string key) => _container.GetInstance(service, key);
        protected override IEnumerable<object> GetAllInstances(Type service) => _container.GetAllInstances(service);
        protected override void BuildUp(object instance) => _container.BuildUp(instance);
    }
}