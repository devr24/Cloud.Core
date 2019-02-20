#Build instructions
-------------------

Two build steps:

1. Build docker image
YAML:

steps:

- task: Docker@1

  displayName: 'Build Docker image'

  inputs:

    azureSubscriptionEndpoint: 'DEV-INTERNAL-NEW'

    azureContainerRegistry: clouddevregistry.azurecr.io

    dockerFile: Cloud.Core.BaseImage/Dockerfile

    useDefaultContext: false

    buildContext: Cloud.Core.BaseImage

    imageName: 'Cloud.Core.Base:$(Build.BuildNumber)'

    includeLatestTag: true



2. Push docker image
YAML:

steps:

- task: Docker@1

  displayName: 'Push Docker image'

  inputs:

    azureSubscriptionEndpoint: 'DEV-INTERNAL-NEW'

    azureContainerRegistry: clouddevregistry.azurecr.io

    command: 'Push an image'

    imageName: Cloud.Core.Base
