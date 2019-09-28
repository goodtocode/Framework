using GoodToCode.Framework.Application;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Operation;
using System;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public class TestViewModel<TDto> : ViewModel<TDto> where TDto : EntityDto<TDto>, new()
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
        }        
    }
}
