﻿using GoodToCode.Framework.Entity;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

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
    public class CrudApiController<TEntity> : ControllerBase where TEntity : EntityBase<TEntity>, new()
    {
        private IEntityWriterConfiguration<TEntity> _dbConfig;
        private IConfiguration _appConfig;

        /// <summary>
        /// Name of the controller and path part
        /// </summary>
        public string ControllerName => typeof(TEntity).Name;

        /// <summary>
        /// Database configuration
        /// </summary>
        private IEntityWriterConfiguration<TEntity> Config
        {
            get
            {
                _dbConfig = _dbConfig ?? (_dbConfig = new EntityWriterConfiguration<TEntity>(_appConfig["ConnectionStrings:DefaultConnection"]));
                return _dbConfig;
            }
        }

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
        public const string PutAction = "PutAsync";

        /// <summary>
        /// Exposed endpoint name for HTTP_POST
        /// </summary>
        public const string PostAction = "PostAsync";

        /// <summary>
        /// Exposed endpoint name for HTTP_DELETE
        /// </summary>
        public const string DeleteAction = "DeleteAsync";

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public CrudApiController()
        //{
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        public CrudApiController(IConfiguration appConfig, IEntityWriterConfiguration<TEntity> dbConfig)
        {
            _dbConfig = dbConfig;
        }

        /// <summary>
        /// Retrieves item by Id
        /// </summary>        
        /// <param name="key"></param>
        /// <returns>Person that matches the Id, or initialized PersonDto for not found condition</returns>
        [HttpGet("{key}")]
        public IActionResult Get(string key)
        {
            if (string.IsNullOrEmpty(key)) return UnprocessableEntity();

            try
            {
                using (var reader = new EntityReader<TEntity>(Config.ConnectionString))
                {
                    var entity = reader.GetByIdOrKey(key);
                    if (entity.IsNew) return NotFound();
                    return Ok(entity);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"{ex.Message}. Inner Exception: {ex.InnerException}" });
            }
        }

        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody]TEntity entity)
        {
            try
            {
                using (var writer = new EntityWriter<TEntity>(Config))
                {
                    var returnEntity = await writer.SaveAsync();
                    if (returnEntity.IsNew) return BadRequest();
                    return Ok(returnEntity);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"{ex.Message}. Inner Exception: {ex.InnerException}" });
            }
        }

        /// <summary>
        /// Saves changes to a item
        /// </summary>
        /// <param name="entity">Full Person model worth of data with user changes</param>
        /// <returns>PersonDto containing Person data</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]TEntity entity)
        {
            try
            {
                using (var writer = new EntityWriter<TEntity>(Config))
                {
                    var returnEntity = await writer.SaveAsync();
                    if (returnEntity.IsNew) return BadRequest();
                    return Ok(returnEntity);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"{ex.Message}. Inner Exception: {ex.InnerException}" });
            }
        }

        /// <summary>
        /// Saves changes to a item
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteAsync(string key)
        {
            try
            {
                using (var reader = new EntityReader<TEntity>(Config.ConnectionString))
                {
                    var entity = reader.GetByIdOrKey(key);
                    TEntity returnEntity;
                    using (var writer = new EntityWriter<TEntity>(Config))
                    {
                        returnEntity = await writer.DeleteAsync();
                        if (!returnEntity.IsNew) return BadRequest();
                    }
                    return Ok(returnEntity);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"{ex.Message}. Inner Exception: {ex.InnerException}" });
            }
        }
    }
}