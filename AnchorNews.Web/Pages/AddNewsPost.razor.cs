using AnchorNews.Models;
using AnchorNews.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection;

namespace AnchorNews.Web.Pages
{
    partial class AddNewsPost
    {

        [Inject]
        IAnchorNewsService AnchorNewsService { get; set; }

        [Parameter]
        public AddPostRequest addPostRequest { get; set; } = new AddPostRequest();

        private bool formInvalid = true;
        private EditContext? editContext;
        string error { get; set; }

        protected override void OnInitialized()
        {
            editContext = new(addPostRequest);
            editContext.OnFieldChanged += HandleFieldChanged;
        }

        private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
        {
            if (editContext is not null)
            {
                formInvalid = !editContext.Validate();
                StateHasChanged();
            }
        }

        protected async void OnSubmit()
        {
            try
            {
                await AnchorNewsService.AddPost(addPostRequest);
                addPostRequest = new();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                StateHasChanged();
            }

        }

        public void Dispose()
        {
            if (editContext is not null)
            {
                editContext.OnFieldChanged -= HandleFieldChanged;
            }
        }
    }
}
