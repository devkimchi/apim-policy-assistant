targetScope = 'subscription'

param name string
param location string

param apiManagementPublisherName string = 'Dev Kimchi'
param apiManagementPublisherEmail string = 'apim@devkimchi.com'

@secure()
param gitHubUsername string
param gitHubRepositoryName string
param gitHubBranchName string = 'main'

// tags that should be applied to all resources.
var tags = {
  // Tag all resources with the environment name.
  'azd-env-name': name
}
var apps = [
  {
    isFunctionApp: true
    functionAppSuffix: 'aoai'
    apiName: 'AOAI'
    apiPath: 'aoai'
    apiServiceUrl: 'https://fncapp-{{AZURE_ENV_NAME}}-{{SUFFIX}}.azurewebsites.net/api'
    apiReferenceUrl: 'https://raw.githubusercontent.com/${gitHubUsername}/${gitHubRepositoryName}/${gitHubBranchName}/infra/openapi-{{SUFFIX}}.{{EXTENSION}}'
    apiFormat: 'openapi-link'
    apiExtension: 'yaml'
    apiSubscription: true
    apiProduct: 'default'
    apiOperations: []
  }
]
var storageContainerName = 'openapis'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-${name}'
  location: location
  tags: tags
}

module cogsvc './provision-CognitiveServices.bicep' = {
  name: 'CognitiveServices'
  scope: rg
  params: {
    name: name
    tags: tags
  }
}

module apim './provision-ApiManagement.bicep' = {
  name: 'ApiManagement'
  scope: rg
  params: {
    name: name
    location: location
    tags: tags
    apiManagementPublisherName: apiManagementPublisherName
    apiManagementPublisherEmail: apiManagementPublisherEmail
    apiManagementPolicyFormat: 'xml-link'
    apiManagementPolicyValue: 'https://raw.githubusercontent.com/devkimchi/apim-policy-assistant/main/infra/apim-policy-global.xml'
  }
}

module fncapps './provision-functionApp.bicep' = [for (app, index) in apps: if (app.isFunctionApp == true) {
  name: 'FunctionApp_${app.apiName}'
  scope: rg
  dependsOn: [
    apim
  ]
  params: {
    name: name
    suffix: app.functionAppSuffix
    location: location
    tags: tags
    storageContainerName: storageContainerName
    aoaiApiVersion: '2023-06-01-preview'
    aoaiDeploymentId: cogsvc.outputs.deploymentId
    aoaiEndpoint: cogsvc.outputs.endpoint
    aoaiAuthKey: cogsvc.outputs.apiKey
  }
}]

module apis './provision-apiManagementApi.bicep' = [for (app, index) in apps: {
  name: 'ApiManagementApi_${app.apiName}'
  scope: rg
  dependsOn: [
    apim
  ]
  params: {
    name: name
    location: location
    apiManagementApiName: app.apiName
    apiManagementApiDisplayName: app.apiName
    apiManagementApiDescription: app.apiName
    apiManagementApiSubscriptionRequired: app.apiSubscription
    apiManagementApiServiceUrl: replace(replace(app.apiServiceUrl, '{{AZURE_ENV_NAME}}', name), '{{SUFFIX}}', app.functionAppSuffix)
    apiManagementApiPath: app.apiPath
    apiManagementApiFormat: app.apiFormat
    apiManagementApiValue: replace(replace(app.apiReferenceUrl, '{{SUFFIX}}', app.functionAppSuffix), '{{EXTENSION}}', app.apiExtension)
    apiManagementApiPolicyFormat: 'xml-link'
    apiManagementApiPolicyValue: 'https://raw.githubusercontent.com/${gitHubUsername}/${gitHubRepositoryName}/${gitHubBranchName}/infra/apim-policy-api-${replace(toLower(app.apiName), '-', '')}.xml'
    apiManagementApiOperations: app.apiOperations
    apiManagementProductName: app.apiProduct
  }
}]
