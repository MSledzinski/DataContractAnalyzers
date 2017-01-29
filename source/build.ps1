Param (
    [string[]] $TaskListPsake = @(),
    [hashtable] $Parameters = @{},
    [hashtable] $Properties = @{}
)

# If external call - ensure PSake present and call real build script
if (-not $MyInvocation.PSScriptRoot) 
{
    if(-not (Get-Module -ListAvailable psake))
    {
        Install-Module psake -Force -Confirm:$false
    }

    Remove-Module psake -Force
    Import-Module psake -DisableNameChecking

    Invoke-PSake $PSCommandPath $TaskListPsake '4.0' $false $Parameters $Properties -nologo

    return
}

# Build script
Properties {
	$baseDir = Resolve-Path .
    $outputDir = Join-Path $baseDir 'build'
	$project = Split-Path $baseDir -Leaf
	$artifactDir = "$baseDir\Packages"
	$version = "1.0.0"
}

Task Default -depends Build

Task Clean {
    if (Test-Path $outputDir)
    {
        Remove-Item $outputDir -Recurse -Force -ErrorAction Stop
    }
}

Task RestorePackages {
    Exec { .\Tools\NuGet restore -PackagesDirectory .\packages }
}

Task UpdateAssemblyInfoVersion {
    [scriptblock]$updateVersion = {
					$_ -replace 'AssemblyVersion.+$',"AssemblyVersion(`"$version`")]" `
					-replace 'AssemblyFileVersion.+$',"AssemblyFileVersion(`"$version`")]"
			}

    Get-ChildItem -Path $baseDir -Filter AssemblyInfo.cs -Recurse |
		Foreach-Object {
			(Get-Content $_.FullName) | Foreach-Object $updateVersion | Out-File $_.FullName
		}
}

Task Build -Depends Clean,RestorePackages,UpdateAssemblyInfoVersion {
	Exec { msbuild 'DataContractAnalyzer.sln' /p:Configuration=Release /t:"Clean,Build" }
}
