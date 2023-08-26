param name string
param location string = 'eastasia'

@secure()
param appInsightsId string
@secure()
param appInsightsInstrumentationKey string
@secure()
param appInsightsConnectionString string

param apiManagementName string
@secure()
param apiManagementSubscriptionKey string

var staticApp = {
  name: 'sttapp-${name}'
  location: location
  appInsights: {
    id: appInsightsId
    instrumentationKey: appInsightsInstrumentationKey
    connectionString: appInsightsConnectionString
  }
  apim: {
    name: apiManagementName
    subscriptionKey: apiManagementSubscriptionKey
  }
}

resource sttapp 'Microsoft.Web/staticSites@2022-03-01' = {
  name: staticApp.name
  location: location
  sku: {
    name: 'Free'
  }
  properties: {
    allowConfigFileUpdates: true
    stagingEnvironmentPolicy: 'Enabled'
  }
}

resource sttappSettings 'Microsoft.Web/staticSites/config@2022-03-01' = {
  name: 'appsettings'
  parent: sttapp
  properties: {
    FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'

    APPINSIGHTS_INSTRUMENTATIONKEY: staticApp.appInsights.instrumentationKey
    APPLICATIONINSIGHTS_CONNECTION_STRING: staticApp.appInsights.connectionString

    OpenApi__DocVersion: '1.0.0'
    OpenApi__DocTitle: 'APIM Policy Assistant Facade API'
    OpenApi__DocDescription: 'This is a set of facade API to provide AI assistant feature to generate APIM policies.'

    Apim__BaseUrl: 'https://${staticApp.apim.name}.azure-api.net/aoai'
    Apim__SubscriptionKey: staticApp.apim.subscriptionKey

    MSGraph__TenantId: '{{AZURE_AD_TENANT_ID}}'
    MSGraph__ClientId: '{{AZURE_AD_CLIENT_ID}}'
    MSGraph__ClientSecret: '{{AZURE_AD_CLIENT_SECRET}}'
  }
}

output id string = sttapp.id
output name string = sttapp.name
output hostname string = sttapp.properties.defaultHostname
