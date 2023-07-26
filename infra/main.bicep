targetScope = 'subscription'

param name string
param location string

param apiManagementPublisherName string = 'Dev Kimchi'
param apiManagementPublisherEmail string = 'apim@devkimchi.com'

// tags that should be applied to all resources.
var tags = {
  // Tag all resources with the environment name.
  'azd-env-name': name
}

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
