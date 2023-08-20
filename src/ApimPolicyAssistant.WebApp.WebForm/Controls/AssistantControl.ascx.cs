using System;
using System.Threading.Tasks;

using ApimPolicyAssistant.Services.Abstractions;

namespace ApimPolicyAssistant.WebApp.WebForm.Controls
{
    public partial class AssistantControl : System.Web.UI.UserControl
    {
        public IOpenApiClient Api { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Prompt.Text = "Show me the APIM policy in general.";
        }

        protected async void Complete_Click(object sender, EventArgs e)
        {
            this.Completion.Text = await Api.GetCompletionsAsync(this.Prompt.Text);
        }

        protected async void Clear_Click(object sender, EventArgs e)
        {
            this.Prompt.Text = default;
            this.Completion.Text = default;

            await Task.CompletedTask;
        }
    }
}
