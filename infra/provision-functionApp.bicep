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
  name: 'StorageAccount_FunctionApp_${suffix}'
  params: {
    name: shortname
    location: location
    tags: tags
    storageContainerName: storageContainerName
  }
}

module wrkspc './logAnalyticsWorkspace.bicep' = {
  name: 'LogAnalyticsWorkspace_FunctionApp_${suffix}'
  params: {
    name: longname
    location: location
    tags: tags
  }
}

module appins './applicationInsights.bicep' = {
  name: 'ApplicationInsights_FunctionApp_${suffix}'
  params: {
    name: longname
    location: location
    workspaceId: wrkspc.outputs.id
    tags: tags
  }
}

module csplan './consumptionPlan.bicep' = {
  name: 'ConsumptionPlan_FunctionApp_${suffix}'
  params: {
    name: longname
    location: location
    tags: tags
  }
}

module fncapp './functionApp.bicep' = {
  name: 'FunctionApp_FunctionApp_${suffix}'
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
