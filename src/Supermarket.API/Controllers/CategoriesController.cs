using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Services;
using Supermarket.API.Resources;

namespace Supermarket.API.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService) => (_categoryService) = (categoryService);

        /// <summary>
        /// Lists all categories.
        /// </summary>
        /// <returns>List os categories.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryResource>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<CategoryResource>> ListAsync()
        {
            var categories = await _categoryService.ListAsync().ConfigureAwait(false);
            var resources = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);

            return resources;
        }

        /// <summary>
        /// Saves a new category.
        /// </summary>
        /// <param name="resource">Category data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryResource), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] SaveCategoryResource resource)
        {
            var category = _mapper.Map<SaveCategoryResource, Category>(resource);
            var result = await _categoryService.SaveAsync(category).ConfigureAwait(false);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Resource);
            return Ok(categoryResource);
        }

        /// <summary>
        /// Updates an existing category according to an identifier.
        /// </summary>
        /// <param name="id">Category identifier.</param>
        /// <param name="resource">Updated category data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryResource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveCategoryResource resource)
        {
            var category = _mapper.Map<SaveCategoryResource, Category>(resource);
            var result = await _categoryService.UpdateAsync(id, category).ConfigureAwait(false);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Resource);
            return Ok(categoryResource);
        }

        /// <summary>
        /// Deletes a given category according to an identifier.
        /// </summary>
        /// <param name="id">Category identifier.</param>
        /// <returns>Response for the request.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CategoryResource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResource), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _categoryService.DeleteAsync(id).ConfigureAwait(false);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var categoryResource = _mapper.Map<Category, CategoryResource>(result.Resource);
            return Ok(categoryResource);
        }
    }
}