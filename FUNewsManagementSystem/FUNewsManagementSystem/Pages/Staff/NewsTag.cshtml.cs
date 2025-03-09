using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Staff
{
    public class NewsTagModel : BasePageModel
    {
        private readonly ITagServices _tagServices;
        public List<TagDTO> TagDTOs { get; set; } = new List<TagDTO>();

        [BindProperty]
        public TagDTO tagDTO {  get; set; }

        public NewsTagModel(ITagServices tagServices)
        {
            _tagServices = tagServices;
        }
        public void OnGet()
        {

        }

        public void OnPost()
        {

        }

    }
}
