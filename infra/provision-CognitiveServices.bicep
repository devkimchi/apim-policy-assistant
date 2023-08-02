param name string
param location string = 'eastus'

param tags object = {}

param aoaiModels array = [
  {
    name: 'gpt-35-turbo-16k'
    deploymentName: 'model-gpt35turbo16k'
    version: '0613'
    skuName: 'Standard'
    skuCapacity: 120
  }
]

module aoai './openAI.bicep' = {
  name: 'CognitiveServices_AOAI'
  params: {
    name: name
    location: location
    tags: tags
    aoaiModels: aoaiModels
  }
}

output name string = aoai.outputs.name
output endpoint string = aoai.outputs.endpoint
output apiKey string = aoai.outputs.apiKey
output deploymentId string = aoaiModels[0].deploymentName
