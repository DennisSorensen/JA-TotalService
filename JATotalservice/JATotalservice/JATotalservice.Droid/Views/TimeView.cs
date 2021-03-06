﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using JATotalservice.Core.Service;
using JATotalservice.Core.ViewModels;
using JATotalservice.Droid.Adapter;
using JATotalservice.Droid.Helpers;
using ModelLayer;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;


namespace JATotalservice.Droid.Views
{
    [MvxFragmentPresentation(typeof(FirstViewModel), Resource.Id.content_frame, true)]
    [Activity(Label = "View for TimeViewModel")]
    public class TimeView : BaseFragment<TimeViewModel>
    {

        protected override int FragmentId => Resource.Layout.TimeView;

        ListView Materials;
        Button sendTimeRegistration;
        Button locationButton;
        Task task;
        NumberPicker hour;
        NumberPicker minute;
        NumberPicker hour1;
        NumberPicker minute1;
        Spinner TaskDropDown;
        View view;
        MaterialsListViewAdapter materialsListViewAdapter;
        EditText employeeId;
        private Android.Support.V7.Widget.Toolbar _toolbar;

        LocationHelper locationHelper = new LocationHelper();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);

            _toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            _toolbar.Title = "Tidsregistering";
            //Set up time spinners
            SetupTime();

            //task drop down liste, create, land listen on itemselected
            TaskDropDown = view.FindViewById<Spinner>(Resource.Id.dropdown);
            TaskDropDown.Adapter = new DropDownTaskAdapter(ViewModel.Tasks, view.Context);

            TaskDropDown.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            //Floating botton add material
            var add = view.FindViewById<FloatingActionButton>(Resource.Id.Add);
            add.Click += delegate { AddMaterial(); };

            //MaterialList 
            materialsListViewAdapter = new MaterialsListViewAdapter(ViewModel.MaterialsAmount, view.Context, ViewModel);
            Materials = view.FindViewById<ListView>(Resource.Id.MaterialsListView);
            Materials.Adapter = materialsListViewAdapter;
            Utility.setListViewHeightBasedOnChildren(Materials); //Hack maybe it works when we are using bindings - Read something about it?

            employeeId = view.FindViewById<EditText>(Resource.Id.Employee);

            //Send Timeregistration
            sendTimeRegistration = view.FindViewById<Button>(Resource.Id.Submit);
            sendTimeRegistration.Click += delegate { sendData(view.Context); };

            //Finder elementer i view'et
            locationButton = view.FindViewById<Button>(Resource.Id.Location);

            //Forbinder klik eventet til lokation kanppen
            //locationButton.Click += delegate { GetLocation(view.Context); };
            locationButton.Click += delegate { Location(view.Context); };
            

            SetupBindings();
            return view;
        }

        private void Location(Context context)
        {
            Location location = locationHelper.GetLocation(context); //Her får du den location tilbage med lat og long
            if (location == null) //Spørger om du fik en lokation tilbage, og hvis ikke så laver en en toast og returner
            {
                Toast.MakeText(context, "Kan ikke finde din lokation", ToastLength.Long).Show();
                return;
            }

            LocationModel address = LocationService.getLocationAddress(location.Latitude, location.Longitude); //Gets the adress for the lat and long given

            var foundTask = ViewModel.CheckIfLocationTaskExist(address); //Finds the task where the location maches
            int indexForFoundTask = ViewModel.Tasks.FindLastIndex(m => m.id == foundTask.id); //Finds the index for the task 

            TaskDropDown.SetSelection(indexForFoundTask); //Sets the task as selected in the dropdown/spinner for tasks in the view
            
            Toast.MakeText(context, "Fandt opgaven: " + foundTask.name + ". Som var tættest på din placering" , ToastLength.Long).Show();
            
        }

        private void AddMaterial()
        {
            //Adds a material to the MaterialTask list

            ViewModel.AddMaterials(Tuple.Create(new Material { id = 1 }, 1));
            materialsListViewAdapter = new MaterialsListViewAdapter(ViewModel.MaterialsAmount, view.Context, ViewModel);

            Materials.Adapter = materialsListViewAdapter;
            Utility.setListViewHeightBasedOnChildren(Materials); //Hack maybe it works when we are using bindings - Read something about it?
        }

        private void sendData(Context context)
        {
            //Sends object timeregistartion to Viewmodel
            TimeRegistartion timeRegistartion = new TimeRegistartion();
            var hourS = hour.Value;
            var minS = minute.Value;
            var hourE = hour1.Value;
            var minE = minute1.Value;
            var date = DateTime.Now;
            timeRegistartion.startTime = new DateTime(date.Year, date.Month, date.Day, hourS, minS, 00); //He last parameter is sceond, and we dont use that, so we ar setting it to 0
            timeRegistartion.endTime = new DateTime(date.Year, date.Month, date.Day, hourE, minE, 00);
            timeRegistartion.task = task;
            timeRegistartion.employee = new Employee { };
            timeRegistartion.employee.Id = int.Parse(employeeId.Text);
            //var MaterialsAmount = materialsListViewAdapter.materials;
            //ViewModel.MaterialsAmount = MaterialsAmount;

            //TODO: Koble de valgte matrialer på med antal

            //timeRegistartion.employee = new Employee { Id = 1 }; //TODO: Fjernes når vi har styr på en employee - for nu er den presat

            ViewModel.PostTime.Execute(timeRegistartion);
            Materials.Adapter = materialsListViewAdapter;
            Utility.setListViewHeightBasedOnChildren(Materials); //Hack maybe it works when we are using bindings - Read something about it?

            Toast.MakeText(context, "Det er sendt", ToastLength.Long).Show(); //Det er den lille sorte popup nede i bunden, ligesom en allert på iOS

        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //tjekker hvilket task der er valgt og sætter den til at være = task
            //En task er den opgave der bliver udført for kunden, som medarbejderen vælger og registrere brugt tid til
            task = ViewModel.Tasks[e.Position];
        }
        private void ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            //tjekker hvilket task der er valgt og sætter den til at være = task
            //En task er den opgave der bliver udført for kunden, som medarbejderen vælger og registrere brugt tid til
            // task = ViewModel.Tasks[e.Position];
        }


        private void SetupTime()
        {
            //Til timepicker - bestemmer hvilke værdier man kan vælge mellem på timePickeren
            hour = view.FindViewById<NumberPicker>(Resource.Id.numberPickerHour);
            minute = view.FindViewById<NumberPicker>(Resource.Id.numberPickerMinute);
            hour.MaxValue = 23;
            hour.MinValue = 0;
            hour.Value = DateTime.Now.Hour;
            minute.MaxValue = 3;
            minute.MinValue = 0;
            minute.SetDisplayedValues(new String[] { "0", "15", "30", "45" }); //Interval på 15,30,45 minutter startid
            //minute.Value = DateTime.Now.Minute;
            hour1 = view.FindViewById<NumberPicker>(Resource.Id.numberPickerHour2);
            minute1 = view.FindViewById<NumberPicker>(Resource.Id.numberPickerMinute2);
            hour1.MaxValue = 23;
            hour1.MinValue = 0;
            hour1.Value = DateTime.Now.Hour;
            minute1.MaxValue = 3;
            minute1.MinValue = 0;
            minute1.SetDisplayedValues(new String[] { "0", "15", "30", "45" }); //Interval på 15,30,45 minutter sluttid
            //minute1.Value = DateTime.Now.Minute;
        }
        protected void SetupBindings()
        {
            var set = this.CreateBindingSet<TimeView, TimeViewModel>(); //Creates the binding between the view and viewModel
                                                                        //set.Bind(Materials).For(m => m.)
                                                                        // set.Bind(Tasks).To(vm => vm.Tasks); //Binds the test from the viewModel til the view's textView
            set.Apply();
        }

        
    }
}