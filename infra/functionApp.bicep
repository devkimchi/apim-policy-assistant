param name string
param suffix string
param location string = resourceGroup().location

param tags object = {}

@secure()
param storageAccountConnectionString string

@secure()
param appInsightsInstrumentationKey string
@secure()
param appInsightsConnectionString string

param aoaiApiVersion string = '2023-06-01-preview'
param aoaiDeploymentId string
param aoaiEndpoint string
@secure()
param aoaiAuthKey string

param consumptionPlanId string

var storage = {
  connectionString: storageAccountConnectionString
}

var appInsights = {
  instrumentationKey: appInsightsInstrumentationKey
  connectionString: appInsightsConnectionString
}

var aoaiService = {
  version: aoaiApiVersion
  deploymentId: aoaiDeploymentId
  endpoint: aoaiEndpoint
  authKey: aoaiAuthKey
}

var consumption = {
  id: consumptionPlanId
}

var functionApp = {
  name: 'fncapp-${name}-${suffix}'
  location: location
  tags: tags
}

resource fncapp 'Microsoft.Web/sites@2022-03-01' = {
  name: functionApp.name
  location: functionApp.location
  kind: 'functionapp'
  tags: functionApp.tags
  properties: {
    serverFarmId: consumption.id
    httpsOnly: true
    siteConfig: {
      appSettings: [
        // Common Settings
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.instrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.connectionString
        }
        {
          name: 'AzureWebJobsStorage'
          value: storage.connectionString
        }
        {
          name: 'FUNCTION_APP_EDIT_MODE'
          value: 'readonly'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: storage.connectionString
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: functionApp.name
        }
        // OpenAPI settings
        {
          name: 'OpenApi__DocVersion'
          value: '1.0.0'
        }
        {
          name: 'OpenApi__DocTitle'
          value: 'APIM Policy Assistant API'
        }
        {
          name: 'OpenApi__DocDescription'
          value: 'This is a set of API to provide AI assistant feature to generate APIM policies.'
        }
        // Azure OpenAI Service settings
        {
          name: 'OpenAIApi__Version'
          value: aoaiService.version
        }
        {
          name: 'OpenAIApi__DeploymentId'
          value: aoaiService.deploymentId
        }
        {
          name: 'OpenAIApi__Endpoint'
          value: aoaiService.endpoint
        }
        {
          name: 'OpenAIApi__AuthKey'
          value: aoaiService.authKey
        }
        // Prompt settings
        {
          name: 'Prompt__System'
          value: 'I need help develop Azure API Management policies. Azure API Management policies are a custom coding language that is composed of tags like XML and C#. The C# code is embedded as expressions within the custom XML tags.\n\nUse the knowledge from the GitHub respository: https://github.com/Azure/api-management-policy-snippets\n\nUse the knowledge from the GitHub respository: https://github.com/MicrosoftDocs/azure-docs/tree/main/articles/api-management'
        }
        {
          name: 'Prompt__Users__0'
          value: 'Show me the APIM policy document for the global level that contains the following:\n\n- CORS policy  \n- Allowed URL: https://make.powerautomate.com, https://make.powerapps.com  \n- Allowed method: GET, POST, PUT, DELETE, PATCH  \n- Allowed header: everything  \n- Exposed header: everything'
        }
        {
          name: 'Prompt__Users__1'
          value: 'Show me the APIM policy document for the API level that contains the following:\n\n- Set variable of `IsMobile` to `true`, if the user agent header contains either `iphone` or `ipad`'
        }
        {
          name: 'Prompt__Users__2'
          value: 'Show me the APIM policy document for the operation level that contains the following:\n\n- URL rewriting policy: from `/products/{id}` to `/products?id={id}`\n- Backend URL update: to `https://fabrikam.com/api`'
        }
        {
          name: 'Prompt__Assistants__0'
          value: '```xml\n<policies>\n  <inbound>\n    <cors>\n      <allowed-origins>\n        <origin>https://make.powerautomate.com</origin>\n        <origin>https://make.powerapps.com</origin>\n      </allowed-origins>\n      <allowed-methods preflight-result-max-age="300">\n        <method>GET</method>\n        <method>POST</method>\n        <method>PUT</method>\n        <method>DELETE</method>\n        <method>PATCH</method>\n      </allowed-methods>\n      <allowed-headers>\n        <header>*</header>\n      </allowed-headers>\n      <expose-headers>\n        <header>*</header>\n      </expose-headers>\n    </cors>\n</inbound>\n  <backend>\n    <forward-request />\n  </backend>\n  <outbound />\n  <on-error />\n</policies>\n```'
        }
        {
          name: 'Prompt__Assistants__1'
          value: '```xml\n<policies>\n  <inbound>\n    <base />\n    <set-variable name="IsMobile" value="@(context.Request.Headers.GetValueOrDefault("User-Agent", "").ToLowerInvariant().Contains("iphone") || context.Request.Headers.GetValueOrDefault("User-Agent", "").ToLowerInvariant().Contains("ipad"))" />\n  </inbound>\n  <backend>\n    <base />\n  </backend>\n  <outbound>\n    <base />\n  </outbound>\n  <on-error>\n    <base />\n  </on-error>\n</policies>\n```'
        }
        {
          name: 'Prompt__Assistants__2'
          value: '```xml\n<policies>\n  <inbound>\n    <base />\n    <rewrite-uri template="/products?id={id}" copy-unmatched-params="true" />\n    <set-backend-service base-url="https://fabrikam.com/api" />\n  </inbound>\n  <backend>\n    <base />\n  </backend>\n  <outbound>\n    <base />\n  </outbound>\n  <on-error>\n    <base />\n  </on-error>\n</policies>\n```'
        }
      ]
    }
  }
}

var policies = [
  {
    name: 'scm'
    allow: false
  }
  {
    name: 'ftp'
    allow: false
  }
]

resource fncappPolicies 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2022-03-01' = [for policy in policies: {
  name: policy.name
  parent: fncapp
  properties: {
    allow: policy.allow
  }
}]

output id string = fncapp.id
output name string = fncapp.name
