using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using JATotalservice.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Android.Support.V4.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;
using Android.Content.PM;
namespace JATotalservice.Droid.Views
{

    [MvxActivityPresentation]
    [Activity(Label = "JA Totalservce",
           LaunchMode = LaunchMode.SingleTop
       
           )]
    public class FirstView : MvxAppCompatActivity<FirstViewModel>
    {
        TextView textView;

        //View button;
        // protected override int LayoutResource => Resource.Layout.FirstView;
        public DrawerLayout DrawerLayout { get; set; }
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            // UserDialogs.Init(this);

            SetContentView(Resource.Layout.FirstView);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if (bundle == null)
            {
                ViewModel.NavigateToEstimatePriceCommand.Execute(null);
                ViewModel.ShowMenuViewModelCommand.Execute(null);

            }

            //var dt = new DateTime(2010, 10, 12);   
            //var dt1 = new DateTime(2011, 08, 06);    

            //TimeRegistartion tr = new TimeRegistartion();   
            //tr.startTime = dt; 
            //tr.endTime = dt1;
            //tr.Id = 123456;
            //tr.task = new ModelLayer.Task();
            //tr.task.id = 1;
            //tr.task.name = "snerydning";
            //tr.task.materials = new List<Material>();
            //tr.employee = new Employee();
            //tr.task.timeRegistrations = new List<TimeRegistartion>();
            //tr.task.description = "bla bla bla";
            //tr.employee.Id = 1;
            //TimeRegistartionService.postTimeInfoAsync(tr);
            //var ttt = TimeRegistartionService.GetAllTimeInfo();
            //var tt = TimeRegistartionService.getTimeInfo(300);
            //TimeRegistartionService.DeleteTimeInfo(300);

            //EstimatedPrice estimatedPrice = new EstimatedPrice();
            //estimatedPrice.Id = 11;
            //estimatedPrice.estimatedTime = 987654321;
            //estimatedPrice.materials = null;
            //Console.WriteLine("------------------------POST-------------------------------------");
            ////EstimatedPriceService.PostEstimatedPrice(estimatedPrice);
            //Console.WriteLine("----------------------------GET---------------------------------");
            //var tt = EstimatedPriceService.GetEstimatedPrice(11);
            //Console.WriteLine("-------------------------------PUT------------------------------");
            ////estimatedPrice.estimatedTime = 99999999;
            ////EstimatedPriceService.PutEstimatedPrice(estimatedPrice);
            //Console.WriteLine("----------------------GETALL---------------------------------------");
            //var ttt = EstimatedPriceService.GetAllEstimatedPrices();
            //Console.WriteLine("----------------------------DELETE---------------------------------");
            //EstimatedPriceService.DeleteEstimatedPrice(123456789);
            //Console.WriteLine("-------------------------------------------------------------");

            //EstimatedPrice estimatedPrice = new EstimatedPrice();
            //estimatedPrice.Id = 11;
            //estimatedPrice.estimatedTime = 10;
            //Material material = new Material();
            //material.id = 1;
            //List<Tuple<Material, int>> listen = new List<Tuple<Material, int>>();
            //var tt = Tuple.Create(material, 1);
            //listen.Add(tt);
            //estimatedPrice.materials = listen;

            //EstimatedPriceService.CalculatePrice(estimatedPrice);

            // SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            /* Material m = new Material
             {
                 name = "wood",
                 description = "noget tr�",
                 price = 300
             };
             MaterialService.PostMaterial(m); */

            // SupportActionBar.SetDisplayHomeAsUpEnabled(false);


            textView = FindViewById(Resource.Id.textView1) as TextView;

            /*  FindViewById(Resource.Id.button1).Click += (o, e) =>
              textView.Text = "Dennis er awesome";
              */
            FirstViewModel t = new FirstViewModel();
            //Button navigateToMaterialsButton = FindViewById<Button>(Resource.Id.navigateToNextView); //Finds the button
            //navigateToMaterialsButton.Click += delegate { StartActivity(typeof(MaterialsView)); }; //Navigates to the next view
            //navigateToMaterialsButton.Click += delegate { t.navigateCommand.Execute(); };

            //t.navigateCommand.Execute();

            /*  Button navigateToMaterialButton = FindViewById<Button>(Resource.Id.navigateToMaterialsView);
              navigateToMaterialButton.Click += delegate { ViewModel.NavigateToMaterialsCommand.Execute(); };

              Button navigateToTimeButton = FindViewById<Button>(Resource.Id.navigateToTimeView);
              navigateToTimeButton.Click += delegate { ViewModel.NavigateToTimeRegistrationCommand.Execute(); };

              Button navigateToEstimateButton = FindViewById<Button>(Resource.Id.navigateToEstimateView);
              navigateToEstimateButton.Click += delegate { ViewModel.NavigateToEstimatePriceCommand.Execute(); };*/
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    DrawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
                DrawerLayout.CloseDrawers();
            else
                base.OnBackPressed();
        }
        public void HideSoftKeyboard()
        {
            if (CurrentFocus == null)
                return;

            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);

            CurrentFocus.ClearFocus();
        }
    }
}
