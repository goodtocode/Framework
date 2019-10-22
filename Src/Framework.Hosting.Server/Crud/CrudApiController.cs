using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GoodToCode.Framework.Hosting.Server
{
    /// <summary>
    /// WebAPI CRUD controller to receive CRUD requests.
    /// Complimented by HttpCrudService loaded into MVC IServiceCollection
    /// Expects the following action/endpoint calls
    ///   C - HttpPut(TDto item)
    ///   R - HttpGet(string idOrKey)
    ///   U - HttpPost(TDto item)
    ///   D - HttpDelete(string idOrKey)
    /// </summary>
    [Route("api/[Controller]")]
    public class CrudApiController<TEntity> : ControllerBase where TEntity : ActiveRecordEntity<TEntity>, new()
    {
        /// <summary>
        /// Name of the controller and path part
        /// </summary>
        public string ControllerName => typeof(TEntity).Name;

        /// <summary>
        /// Path part inbetween domain and controller name
        /// I.e. http:/www.domain.com/RootPathPart/ControllerName
        /// </summary>
        public string RootPathPart = "api";

        /// <summary>
        /// Path part to the controller
        /// </summary>
        public string ControllerRoute => $"{RootPathPart}/{ControllerName}";

        /// <summary>
        /// Exposed endpoint name for HTTP_GET
        /// </summary>
        public const string GetAction = "Get";

        /// <summary>
        /// Exposed endpoint name for HTTP_PUT
        /// </summary>
        public const string PutAction = "Put";

        /// <summary>
        /// Exposed endpoint name for HTTP_POST
        /// </summary>
        public const string PostAction = "Post";

        /// <summary>
        /// Exposed endpoint name for HTTP_DELETE
        /// </summary>
        public const string DeleteAction = "Delete";

        /// <summary>
        /// Constructor
        /// </summary>
        public CrudApiController()
        {
        }

        /// <summary>
        /// Retrieves item by Id
        /// </summary>        
        /// <param name="key"></param>
        /// <returns>Person that matches the Id, or initialized PersonDto for not found condition</returns>
        [HttpGet("{key}")]
        public IActionResult Get(string key)
        {
            using (var reader = new EntityReader<TEntity>())
            {
                var entity = reader.GetByIdOrKey(key);
                return Ok(entity);
            }
        }

        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody]TEntity entity)
        {
            entity = entity.Save();
            return Ok(entity);
        }

        /// <summary>
        /// Saves changes to a item
        /// </summary>
        /// <param name="entity">Full Person model worth of data with user changes</param>
        /// <returns>PersonDto containing Person data</returns>
        [HttpPost]
        public IActionResult Post([FromBody]TEntity entity)
        {
            entity = entity.Save();
            return Ok(entity);
        }

        /// <summary>
        /// Saves changes to a item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            using (var reader = new EntityReader<TEntity>())
            {
                var entity = reader.GetByIdOrKey(key);
                entity = entity.Delete();

                return Ok(entity);
            }
        }
    }
}

