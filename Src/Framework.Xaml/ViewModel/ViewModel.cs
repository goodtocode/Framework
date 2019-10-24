using GoodToCode.Extensions;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Net;
using GoodToCode.Framework.Operation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Application
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public abstract class ViewModel<TDto> : IViewModel<TDto>, IGetOperationAsync<TDto>, ICreateOperationAsync<TDto>, IUpdateOperationAsync<TDto>, IDeleteOperationAsync<TDto> where TDto : IEntity, ISerializable<TDto>, new()
    {
        /// <summary>
        /// Name of the Web API controller, that will become the path in the Uri
        ///  - ViewModelUrl = MyWebService + / + ControllerName
        ///  - MyWebService from AppSettings.config MyWebService element
        /// </summary>
        public string MyWebServiceController { get; protected set; } = Defaults.String;

        /// <summary>
        /// Application context of current process
        /// </summary>
        public abstract IApplication MyApplication { get; protected set; }

        /// <summary>
        /// MyWebService
        /// </summary>
        public Uri MyViewModelWebService { get { return new Uri(MyApplication?.MyWebService.ToStringSafe().AddLast("/") + MyWebServiceController, UriKind.RelativeOrAbsolute); } }

        /// <summary>
        /// Model data
        /// </summary>
        public TDto MyModel { get; set; } = new TDto();

        /// <summary>
        /// Sender of main Http Verbs
        /// </summary>
        public HttpVerbSender Sender { get; protected set; } = new HttpVerbSender();

        /// <summary>
        /// OnSendBegin()
        /// </summary>
        public void OnSendBegin() { if (this.SendBegin != null) { SendBegin(this, EventArgs.Empty); } }

        /// <summary>
        /// OnSendEnd()
        /// </summary>
        public void OnSendEnd() { if (this.SendEnd != null) { SendEnd(this, EventArgs.Empty); } }

        /// <summary>
        /// Can this screen go back
        /// </summary>
        public bool CanGoBack { get { return MyApplication.CanGoBack; } }

        /// <summary>
        /// Navigates back to previous screen
        /// </summary>
        public void GoBack() { MyApplication.GoBack(); }

        /// <summary>
        /// Property changed event handler for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed event for INotifyPropertyChanged
        /// </summary>
        /// <param name="propertyName">String name of property</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Send is about to begin
        /// </summary>
        public event SendBeginEventHandler SendBegin;

        /// <summary>
        /// Send is complete
        /// </summary>
        public event SendBeginEventHandler SendEnd;

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void SendBeginEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Constructs a MVVM view-model that stores MyModel, MyApplication and MyWebService
        ///  for self-sufficient page navigation, view binding and web service calls.
        /// </summary>
        /// <param name="myWebServiceController">Name of the Web API controller that supports this view model</param>        
        public ViewModel(string myWebServiceController)
            : base()
        {
            MyWebServiceController = myWebServiceController;
        }

        /// <summary>
        /// Pulls all records from a HttpGet request
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var fullUrl = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/"), UriKind.RelativeOrAbsolute);
            MyModel = new TDto();
            var results = await Sender.SendGetAsync<List<TDto>>(fullUrl);
            return results;
        }

        /// <summary>
        /// Pulls all records from a HttpGet request
        ///  Excludes any record that has an Id == -1 or Key == 00000000-0000-0000-0000-000000000000
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TDto>> GetAllExcludeDefaultAsync()
        {
            var fullUrl = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/"), UriKind.RelativeOrAbsolute);
            MyModel = new TDto();
            var results = await Sender.SendGetAsync<List<TDto>>(fullUrl);
            return results.Where(x => x.Key != Defaults.Guid);
        }

        /// <summary>
        /// Pulls record by Id
        /// </summary>
        /// <param name="id">Integer Id of the record to pull</param>
        /// <returns></returns>
        public async Task<TDto> GetByIdAsync(int id)
        {
            var fullUrl = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/") + id.ToString(), UriKind.RelativeOrAbsolute);
            MyModel = await Sender.SendGetAsync<TDto>(fullUrl);
            return MyModel;
        }

        /// <summary>
        /// Pulls record by Key
        /// </summary>
        /// <param name="key">Guid Key of the record to pull</param>
        /// <returns></returns>
        public async Task<TDto> GetByKeyAsync(Guid key)
        {
            var fullUrl = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/") + key.ToString(), UriKind.RelativeOrAbsolute);
            MyModel = await Sender.SendGetAsync<TDto>(fullUrl);
            return MyModel;
        }

        /// <summary>
        /// Create a record
        /// </summary>
        /// <returns></returns>
        public async Task<TDto> CreateAsync()
        {
            MyModel = await Sender.SendPostAsync<TDto>(MyViewModelWebService, MyModel);
            return MyModel;
        }

        /// <summary>
        /// Edits a record
        /// </summary>
        /// <returns></returns>
        public async Task<TDto> UpdateAsync()
        {
            MyModel = await Sender.SendPutAsync<TDto>(MyViewModelWebService, MyModel);
            return MyModel;
        }

        /// <summary>
        /// Deletes this object from the database via Http Delete
        /// </summary>
        /// <returns>True for success, false for failure</returns>
        public async Task<TDto> DeleteAsync()
        {
            var success = Defaults.Boolean;
            var fullUrll = new Uri(MyViewModelWebService.ToStringSafe().AddLast("/").AddLast(MyModel.Id.ToString()).AddLast("/"), UriKind.RelativeOrAbsolute);
            success = await Sender.SendDeleteAsync(fullUrll);
            if (success) MyModel = new TDto();
            return MyModel;
        }

        public bool CanCreate()
        {
            throw new NotImplementedException();
        }

        public bool CanUpdate()
        {
            throw new NotImplementedException();
        }

        public bool CanDelete()
        {
            throw new NotImplementedException();
        }
    }
}