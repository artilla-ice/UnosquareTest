using Plugin.Media;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using UnosquareTest.Models;
using UnosquareTest.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UnosquareTest.ViewModels
{
    public class EmployeesViewModel : INotifyPropertyChanged
    {
        public EmployeesViewModel()
        {
            this.OnAddNewEmployeeCommand = new Command(OnAddNewEmployee);
            this.OnTakePhotoCommand = new Command(OnTakePhotoAsync);

            var packageService = DependencyService.Get<IPackageService>();
            this.PackageName = packageService.GetAppPackageName();

            this.Employees = new ObservableCollection<EmployeeModel>
            {
                new EmployeeModel
                {
                    ID = Guid.NewGuid(),
                    Name = "Maria Ibarra",
                    Birthdate = new DateTime(1988, 2, 14),
                    ExperienceYears = 8,
                    ProfileImage = "mariapp.jpg"
                },

                new EmployeeModel
                {
                    ID = Guid.NewGuid(),
                    Name = "Kevin Perez",
                    Birthdate = new DateTime(1999, 11, 8),
                    ExperienceYears = 3,
                    ProfileImage ="kevinpp.jpg"
                }
            };
        }

        private async void OnTakePhotoAsync()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status == PermissionStatus.Granted)
                {
                    var photo = await MediaPicker.CapturePhotoAsync();

                    var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using (var stream = await photo.OpenReadAsync())
                    {
                        var resizedStream = ResizeImage(stream, photo.FullPath);
                        await stream.CopyToAsync(resizedStream);

                        var resizedImage = File.Create(newFile);
                        resizedStream.Seek(0, SeekOrigin.Begin);
                        await resizedStream.CopyToAsync(resizedImage);
                        resizedImage.Close();

                        this.NewEmployee.ProfileImage = newFile;
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                throw fnsEx;
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                throw pEx;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"TakePhotoAsync Exception: {ex.Message}");
            }
        }

        private void OnAddNewEmployee()
        {
            // Cuando se le de clic al botón de Agregar, te debe agregar un nuevo elemento al
            // collectionview con los datos ingresados en los entries, debe validar que no se repitan
            // ítems con los campos iguales.

            if (string.IsNullOrEmpty(NewEmployee.Name))
            {
                App.Current.MainPage.DisplayAlert("Información incompleta",
                    "Parece que hay campos vacios, por favor comprueba la información", "Entendido");

                return;
            }

            var employeeExists = Employees.Any(e => e.ID == NewEmployee.ID ||
                                        e.Name == NewEmployee.Name || e.ProfileImage == NewEmployee.ProfileImage);

            if (employeeExists)
            {
                App.Current.MainPage.DisplayAlert("Algo ha salido mal", 
                    "Parece que este empleado ya ha sido registrado", "Entendido");

                return;
            }

            Employees.Add(NewEmployee);

            NewEmployee = new EmployeeModel();
        }

        private Stream ResizeImage(Stream resultStream, string path)
        {
            byte[] imageData;

            Stream stream = resultStream;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            IResizeImageManager resizer = DependencyService.Get<IResizeImageManager>();
            byte[] resizedimage = resizer.ResizeImage(imageData, 100, 100, path);

            Stream resizedImageStream = new MemoryStream(resizedimage);
            return resizedImageStream;
        }

        public Command OnTakePhotoCommand { get; set; }
        public Command OnAddNewEmployeeCommand { get; set; }

        private string m_packageName;
        public string PackageName
        {
            get => m_packageName;
            set
            {
                m_packageName = value;
                OnPropertyChanged();
            }
        }

        private EmployeeModel m_newEmployee = new EmployeeModel();
        public EmployeeModel NewEmployee 
        { 
            get => m_newEmployee ?? new EmployeeModel(); 
            set
            {
                m_newEmployee = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<EmployeeModel> m_employees;
        public ObservableCollection<EmployeeModel> Employees 
        { 
            get => m_employees ?? new ObservableCollection<EmployeeModel>(); 
            set
            {
                m_employees = value;
                OnPropertyChanged();
            }
        }

        public string Title { get => "Feliz año nuevo!!"; }
        public DateTime MaxDate { get => DateTime.Now; }

        // Move to base class for various viewmodels
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
