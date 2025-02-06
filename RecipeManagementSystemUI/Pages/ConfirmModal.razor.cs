using Microsoft.AspNetCore.Components;

namespace RecipeManagementSystemUI.Pages
{
    public partial class ConfirmModal : ComponentBase
    {
        [Parameter] public bool IsConfirmModalOpen { get; set; }
        [Parameter] public string Title { get; set; } = "Confirm Action";
        [Parameter] public string Message { get; set; } = "Are you sure you want to proceed?";
        [Parameter] public EventCallback OnConfirm { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        private async Task Close()
        {
            IsConfirmModalOpen = false;
            await OnClose.InvokeAsync();
        }

        private async Task Confirm()
        {
            await OnConfirm.InvokeAsync();
            await Close();
        }
    }
}

