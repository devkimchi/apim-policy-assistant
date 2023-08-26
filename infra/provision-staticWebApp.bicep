param name string
param location string = resourceGroup().location

param apiManagementName string
@secure()
param apiManagementSubscriptionKey string

module wrkspc './logAnalyticsWorkspace.bicep' = {
  name: 'StaticWebApp_LogAnalyticsWorkspace'
  params: {
    name: '${name}-web'
    location: location
  }
}

module appins 'applicationInsights.bicep' = {
  name: 'StaticWebApp_ApplicationInsights'
  params: {
    name: '${name}-web'
    location: location
    workspaceId: wrkspc.outputs.id
  }
}

module sttapp './staticWebApp.bicep' = {
  name: 'StaticWebApp_StaticWebApp'
  params: {
    name: '${name}-web'
    location: location
    appInsightsId: appins.outputs.id
    appInsightsInstrumentationKey: appins.outputs.instrumentationKey
    appInsightsConnectionString: appins.outputs.connectionString
    apiManagementName: apiManagementName
    apiManagementSubscriptionKey: apiManagementSubscriptionKey
  }
}

output id string = sttapp.outputs.id
output name string = sttapp.outputs.name
output hostname string = sttapp.outputs.hostname
