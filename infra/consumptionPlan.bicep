param name string
param location string = resourceGroup().location

param tags object = {}

var consumption = {
  name: 'csplan-${name}'
  location: location
  tags: tags
}

resource csplan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: consumption.name
  location: consumption.location
  kind: 'functionApp'
  tags: consumption.tags
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

output id string = csplan.id
output name string = csplan.name
