# Azure API Management Policy Assistant

It's your friendly assistant to generate [Azure API Management policy documents](https://learn.microsoft.com/azure/api-management/api-management-howto-policies), using [Azure OpenAI Service](https://learn.microsoft.com/azure/ai-services/openai/overview).

## Acknowledgements

- This repository currently contains [bicep files](https://learn.microsoft.com/azure/azure-resource-manager/bicep/overview) only for [Azure API Management](https://learn.microsoft.com/azure/api-management/api-management-key-concepts) and [Azure OpenAI Service](https://learn.microsoft.com/azure/ai-services/openai/overview) instances.
- The assistant application is on the way.

## Prerequisites

- [Azure Subscription](https://azure.microsoft.com/free)
- [Azure OpenAI Service](https://learn.microsoft.com/azure/ai-services/openai/overview)
- [Azure CLI](https://learn.microsoft.com/cli/azure/what-is-azure-cli)
- [Azure Developer CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli/overview)
- [GitHub CLI](https://cli.github.com)
- [Azure Static Web Apps CLI](https://github.com/Azure/static-web-apps-cli)

## Getting Started

### Provisioning Azure Resources

1. Fork this repository to your GitHub account, `{{GITHUB_USERNAME}}`.
1. Run the commands below to set up a resource names:

   ```bash
   # PowerShell
   $AZURE_ENV_NAME="assistant$(Get-Random -Max 9999)"
   $GITHUB_USERNAME="{{GITHUB_USERNAME}}"

   # Bash
   AZURE_ENV_NAME="assistant$RANDOM"
   GITHUB_USERNAME="{{GITHUB_USERNAME}}"
   ```

1. Run the commands below to provision Azure resources:

   ```bash
   azd auth login
   azd init -e $AZURE_ENV_NAME
   azd up
   ```

   > You might be asked to input your GitHub username and repository name.

### Deploying Applications to Azure

1. Run the commands below to deploy apps to Azure:

   ```bash
   az login
   gh auth login
   azd pipeline config
   gh workflow run "Azure Dev" --repo $GITHUB_USERNAME/apim-policy-assistant
   ```

### Deprovisioning Azure Resources

1. To avoid unexpected billing shock, run the commands below to deprovision Azure resources:

   ```bash
   azd down --force --purge --no-prompt
   ```

## Local Development

### `local.settings.json` - `Assistant.ApiApp`

1. copy `local.settings.sample.json` to `local.settings.json`
1. Substitute the following values in the `local.settings.json` with the actual values:

   ```json
   "OpenAIApi__Endpoint": "https://aoai-{{AZURE_ENV_NAME}}.openai.azure.com/",
   "OpenAIApi__AuthKey": "{{AOAI_API_KEY}}",
   "OpenAIApi__DeploymentId": "{{DEPLOYMENT_ID}}",
   ```

   - `{{AZURE_ENV_NAME}}`: Azure environment name. It looks like `assistant****` where `****` is a random number.
   - `{{AOAI_API_KEY}}`: API Key of Azure OpenAI Service.
   - `{{DEPLOYMENT_ID}}`: Azure OpenAI Service deployment ID. It looks like `model-gpt35turbo16k`.

### `local.settings.json` - `Assistant.ApiApp.SwaFacade`

1. copy `local.settings.sample.json` to `local.settings.json`
1. Substitute the following values in the `local.settings.json` with the actual values:

   ```json
   "Apim__BaseUrl": "https://apim-{{AZURE_ENV_NAME}}.azure-api.net/aoai",
   "Apim__SubscriptionKey": "{{APIM_SUBSCRIPTION_KEY}}",
   ```

   - `{{AZURE_ENV_NAME}}`: Azure environment name. It looks like `assistant****` where `****` is a random number.
   - `{{APIM_SUBSCRIPTION_KEY}}`: Subscription Key of Azure API Management.

### Visual Studio

1. Open Visual Studio with `ApimPolicyAssistantWindows.sln`.
1. Make user that you have multiple applications set as startup projects.
1. Run debug mode by pressing `F5` key.

### Visual Studio Code

1. Make sure that the Debug mode is set to `Blazor & Facade`.
1. Run debug mode by pressing `F5` key.
1. Choose `Assistant.ApiApp.SwaFacade`.
