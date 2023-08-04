# Generate OpenAPI document from Azure Functions app.
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $FunctionAppPath = "src/Assistant.ApiApp",

    [string]
    [Parameter(Mandatory=$false)]
    $Endpoint = "openapi/v3.json",

    [string]
    [Parameter(Mandatory=$false)]
    $OutputPath = "infra",

    [string]
    [Parameter(Mandatory=$false)]
    $OutputFilename = "openapi-aoai.json",

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This generates an OpenAPI document from the given Azure Functions code.

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-FunctionAppPath   <Function app project path>] ``
            [-Endpoint          <OpenAPI document endpoint>] ``
            [-OutputPath        <Generated output storing path>] ``
            [-OutputFilename    <Output filename>] ``

            [-Help]

    Options:
            -FunctionAppPath    Function app project path. Default is `src/Assistant.ApiApp`.
            -Endpoint           OpenAPI document endpoint. Default is `openapi/v3.json`.
            -OutputPath         Generated output storing path. Default is `infra`.
            -OutputFilename     Output filename. Default is `openapi-aoai.json`.

        -Help:          Show this message.
"

    Exit 0
}

# Show usage
$needHelp = $Help -eq $true
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

$repositoryRoot = $(pwd).Path

& $([Scriptblock]::Create($(Invoke-RestMethod https://aka.ms/azfunc-openapi/generate-openapi.ps1))) `
    -FunctionAppPath    "$repositoryRoot/$FunctionAppPath" `
    -Endpoint           $Endpoint `
    -OutputPath         "$repositoryRoot/$OutputPath" `
    -OutputFilename     $OutputFilename `
    -Delay              60
