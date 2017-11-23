param($installPath, $toolsPath, $package, $project)
Write-Host "Setting 'EmbedInteropTypes' to 'false' for all references..."
foreach ($reference in $project.Object.References) 
{
    if ($reference.EmbedInteropTypes) 
    {
        $reference.EmbedInteropTypes = $false;
        $reference.CopyLocal = $true;
    }
}

