<#
.Synopsis
   Sets the assembly version of all assemblies in the source directory.
.DESCRIPTION
   A build script that can be included in TFS 2015 or Visual Studio Online (VSO) vNevt builds that update the version of all assemblies in a workspace.
   It uses the name of the build to extract the version number and updates all AssemblyInfo.cs files to use the new version.
.EXAMPLE
   Set-AssemblyVersion
.EXAMPLE
   Set-AssemblyVersion -SourceDirectory $Env:BUILD_SOURCESDIRECTORY -BuildNumber $Env:BUILD_BUILDNUMBER
.EXAMPLE
   Set-AssemblyVersion -SourceDirectory ".\SourceDir" -BuildNumber "Dev_1.0.20150922.01" -VersionFormat "\d+\.\d+\.\d+\.\d+"
#>
 
[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact='Medium')]
[Alias()]
[OutputType([int])]
Param
(
    # The path to the source directory. Default $Env:BUILD_SOURCESDIRECTORY is set by TFS.
    [Parameter(Mandatory=$false, Position=0)]
    [ValidateNotNullOrEmpty()]
    [string]$SourceDirectory = $Env:BUILD_SOURCESDIRECTORY,
 
    # The build number. Default $Env:BUILD_BUILDNUMBER is set by TFS and must be configured according your regex.
    [Parameter(Mandatory=$false, Position=1)]
    [ValidateNotNullOrEmpty()]
    [string]$BuildNumber = $Env:BUILD_BUILDNUMBER,
 
    # The build number. Default $Env:BUILD_BUILDNUMBER is set by TFS and must be configured according your regex.
    [Parameter(Mandatory=$false, Position=2)]
    [ValidateNotNullOrEmpty()]
    [string]$VersionFormat = "\d+\.\d+\.\d+\.\d+",
 
    # Set the version number also in all AppManifest.xml files.
    [Parameter(Mandatory=$false)]
    [switch]$SetAppVersion
)
 
<#
.Synopsis
   Sets the assembly version of all assemblies in the source directory.
.DESCRIPTION
   A build script that can be included in TFS 2015 or Visual Studio Online (VSO) vNevt builds that update the version of all assemblies in a workspace.
   It uses the name of the build to extract the version number and updates all AssemblyInfo.cs files to use the new version.
.EXAMPLE
   Set-AssemblyVersion
.EXAMPLE
   Set-AssemblyVersion -SourceDirectory $Env:BUILD_SOURCESDIRECTORY -BuildNumber $Env:BUILD_BUILDNUMBER
.EXAMPLE
   Set-AssemblyVersion -SourceDirectory ".\SourceDir" -BuildNumber "Dev_1.0.20150922.01" -VersionFormat "\d+\.\d+\.\d+\.\d+"
#>
function Set-AssemblyVersion
{
    [CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact='Medium')]
    [Alias()]
    [OutputType([int])]
    Param
    (
        # The path to the source directory. Default $Env:BUILD_SOURCESDIRECTORY is set by TFS.
        [Parameter(Mandatory=$false, Position=0)]
        [ValidateNotNullOrEmpty()]
        [string]$SourceDirectory = $Env:BUILD_SOURCESDIRECTORY,
 
        # The build number. Default $Env:BUILD_BUILDNUMBER is set by TFS and must be configured according your regex.
        [Parameter(Mandatory=$false, Position=1)]
        [ValidateNotNullOrEmpty()]
        [string]$BuildNumber = $Env:BUILD_BUILDNUMBER,
 
        # The build number. Default $Env:BUILD_BUILDNUMBER is set by TFS and must be configured according your regex.
        [Parameter(Mandatory=$false, Position=2)]
        [ValidateNotNullOrEmpty()]
        [string]$VersionFormat = "\d+\.\d+\.\d+\.\d+",
 
        # Set the version number also in all AppManifest.xml files.
        [Parameter(Mandatory=$false)]
        [switch]$SetAppVersion
    )
 
    if (-not (Test-Path $SourceDirectory)) {
        throw "The directory '$SourceDirectory' does not exist."
    }
 
    $Version = Get-Version -BuildNumber $BuildNumber -VersionFormat $VersionFormat
 
    $files = Get-Files -SourceDirectory $SourceDirectory
 
    Set-FileContent -Files $files -Version $Version -VersionFormat $VersionFormat
 
    if ($SetAppVersion.IsPresent)
    {
        $files = Get-AppManifest -SourceDirectory $SourceDirectory
        Set-AppManifest -Files $files -Version $Version
    }
}
 
function Get-Version
{
    [CmdletBinding()]
    [Alias()]
    [OutputType([string])]
    Param
    (
        [Parameter(Mandatory=$true, Position=0)]
        [ValidateNotNullOrEmpty()]
        [string]$BuildNumber,
 
        [Parameter(Mandatory=$true, Position=1)]
        [ValidateNotNullOrEmpty()]
        [string]$VersionFormat
    )
 
    $VersionData = [regex]::matches($BuildNumber,$VersionFormat)
 
    if ($VersionData.Count -eq 0){
        throw "Could not find version number with format '$VersionFormat' in BUILD_BUILDNUMBER '$BuildNumber'."
    }
 
    return $VersionData[0]
}
 
function Get-Files
{
    [CmdletBinding()]
    [Alias()]
    [OutputType([System.IO.FileSystemInfo[]])]
    Param
    (
        [Parameter(Mandatory=$true, Position=0)]
        [ValidateNotNullOrEmpty()]
        [string]$SourceDirectory
    )
 
    $folders = Get-ChildItem $SourceDirectory -Recurse -Include "*Properties*" | Where-Object { $_.PSIsContainer }
 
    return $folders | ForEach-Object { Get-ChildItem -Path $_.FullName -Recurse -include AssemblyInfo.* }
}
 
function Get-AppManifest
{
    [CmdletBinding()]
    [Alias()]
    [OutputType([System.IO.FileSystemInfo[]])]
    Param
    (
        [Parameter(Mandatory=$true, Position=0)]
        [ValidateNotNullOrEmpty()]
        [string]$SourceDirectory
    )
 
    return Get-ChildItem -Path $SourceDirectory -Recurse -Filter "AppManifest.xml"
}
 
function Set-FileContent
{
    [CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact='Medium')]
    [OutputType([int])]
    Param
    (
        [Parameter(Mandatory=$true, Position=0)]
        [System.IO.FileSystemInfo[]]$Files,
 
        [Parameter(Mandatory=$true, Position=1)]
        [string]$Version,
 
        [Parameter(Mandatory=$true, Position=2)]
        [string]$VersionFormat
    )
 
    foreach ($file in $Files)
    {
        $filecontent = Get-Content $file
 
        if ($PSCmdlet.ShouldProcess("$file", "Set-AssemblyVersion"))
        {
            attrib $file -r
            $filecontent -replace $VersionFormat, $Version | Out-File $file
            Write-Verbose -Message "Applied Version '$Version' $($file.FullName) - version applied"
        }
    }
}
 
function Set-AppManifest
{
    [CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact='Medium')]
    [OutputType([int])]
    Param
    (
        [Parameter(Mandatory=$true, Position=0)]
        [System.IO.FileSystemInfo[]]$Files,
 
        [Parameter(Mandatory=$true, Position=1)]
        [string]$Version
    )
 
    foreach ($file in $Files)
    {
        [xml]$xml = Get-Content $file
 
        $xml.App.Version = $Version
 
        if ($PSCmdlet.ShouldProcess("$file", "Set-AppManifest")){
            $xml.Save($file.FullName)
        }
    }
}
 
if (-not ($myinvocation.line.Contains("`$here\`$sut"))) {
    Set-AssemblyVersion -SourceDirectory $SourceDirectory -BuildNumber $BuildNumber -VersionFormat $VersionFormat
}