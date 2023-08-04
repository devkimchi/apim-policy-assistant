param name string
param suffix string
param location string = resourceGroup().location

param tags object = {}

param storageContainerName string

param aoaiApiVersion string = '2023-06-01-preview'
param aoaiDeploymentId string
param aoaiEndpoint string
@secure()
param aoaiAuthKey string

var shortname = '${name}${replace(suffix, '-', '')}'
var longname = '${name}-${suffix}'

module st './storageAccount.bicep' = {
  name: 'FunctionApp_${suffix}_StorageAccount'
  params: {
    name: shortname
    location: location
    tags: tags
    storageContainerName: storageContainerName
  }
}

module wrkspc './logAnalyticsWorkspace.bicep' = {
  name: 'FunctionApp_${suffix}_LogAnalyticsWorkspace'
  params: {
    name: longname
    location: location
    tags: tags
  }
}

module appins './applicationInsights.bicep' = {
  name: 'FunctionApp_${suffix}_ApplicationInsights'
  params: {
    name: longname
    location: location
    workspaceId: wrkspc.outputs.id
    tags: tags
  }
}

module csplan './consumptionPlan.bicep' = {
  name: 'FunctionApp_${suffix}_ConsumptionPlan'
  params: {
    name: longname
    location: location
    tags: tags
  }
}

module fncapp './functionApp.bicep' = {
  name: 'FunctionApp_${suffix}_FunctionApp'
  params: {
    name: name
    suffix: suffix
    location: location
    tags: tags
    storageAccountConnectionString: st.outputs.connectionString
    appInsightsInstrumentationKey: appins.outputs.instrumentationKey
    appInsightsConnectionString: appins.outputs.connectionString
    aoaiApiVersion: aoaiApiVersion
    aoaiDeploymentId: aoaiDeploymentId
    aoaiEndpoint: aoaiEndpoint
    aoaiAuthKey: aoaiAuthKey
    consumptionPlanId: csplan.outputs.id
  }
}
