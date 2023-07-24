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

1. Run the commands below to deploy apps to Azure:

   ```bash
   az login
   gh auth login
   azd pipeline config
   gh workflow run "Azure Dev" --repo $GITHUB_USERNAME/apim-policy-assistant
   ```
