using GoodToCode.Framework.Application;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Net;

namespace Framework.Test
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public class TestViewModel<TModel> : ViewModel<TModel> where TModel : EntityDto<TModel>, new()
    {
        /// <summary>
        /// Currently running application
        /// </summary>
        public override IApplication MyApplication { get; protected set; } = new TestApplication();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="myApplication"></param>
        public TestViewModel(string webServiceControllerName)
            : base(webServiceControllerName)
        {
            var x = new HttpVerbSender();
        }        
    }
}
