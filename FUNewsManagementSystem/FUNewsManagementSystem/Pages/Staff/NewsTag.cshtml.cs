using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Staff
{
    public class NewsTagModel : BasePageModel
    {
        private readonly ITagServices _tagService;
        public List<TagDTO> Tags { get; set; } = new();

        [BindProperty]
        public TagDTO newTag { get; set; } = new();

        [BindProperty]
        public int IdDelete { get; set; }

        public NewsTagModel(ITagServices tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Tags = await _tagService.GetTags();
            return Page();
        }

        public async Task<IActionResult> OnPostCreate()
        {
            if (newTag == null || string.IsNullOrWhiteSpace(newTag.TagName))
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }

            try
            {
                int id = await _tagService.CreateTag(newTag);
                return new JsonResult(new
                {
                    success = id > 0,
                    message = id > 0 ? "Tag added successfully" : "Failed to add tag!"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(new { success = false, message = "Error while creating tag." });
            }
        }

        public async Task<IActionResult> OnGetDelete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid tag ID." });
            }

            try
            {
                bool success = await _tagService.DeleteTag(id);
                return new JsonResult(new
                {
                    success,
                    message = success ? "Tag deleted successfully" : "Failed to delete tag!"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(new { success = false, message = "Error while deleting tag." });
            }
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            if (newTag == null || newTag.TagId <= 0 || string.IsNullOrWhiteSpace(newTag.TagName))
            {
                return BadRequest(new { success = false, message = "Invalid tag data." });
            }

            try
            {
                bool success = await _tagService.UpdateTag(newTag);
                return new JsonResult(new
                {
                    success,
                    message = success ? "Tag updated successfully" : "Update failed!"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(new { success = false, message = "Error while updating tag." });
            }
        }
    }
}
