using HWA.Data;
using HWA.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HWA.ViewModels
{
    public class ClaimsSubViewModel : BaseViewModel
    {
        public ObservableCollection<FileResult> ChoosenFiles { get; set; }
        public Command CancelCommand { get; }
        public Command SubmitCommand { get; }
        private ClaimSubmissionManager submissionManager;
        public ClaimsSubViewModel()
        {
            CancelCommand = new Command(Cancel);
            SubmitCommand = new Command(async () => await Submit());
            ChoosenFiles = new ObservableCollection<FileResult>();
            submissionManager = new ClaimSubmissionManager();
        }

        public void Delete(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
                return;
            var itemToDelete = ChoosenFiles.Where(x => x.FileName == itemName).FirstOrDefault();
            ChoosenFiles.Remove(itemToDelete);
        }
        public string ClaimName { get; set; }
        private async Task Submit()
        {
            try
            {
                IsBusy = true;
                var cid = App.Customer.ID;
                var result = await submissionManager.Submit(
                    cid.ToString(),ClaimName,ChoosenFiles.ToList());
                Cancel();
                await ToastUser(result.IsSuccessful, AppResources.appoinment_result, AppResources.appoinment_result_error);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                await ToastUser(false, "UnexpectedError");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
