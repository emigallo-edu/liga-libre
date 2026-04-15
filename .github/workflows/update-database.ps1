param(
    [string]$BaseUrl
)

try {
    $headers = @{ 'X-Api-Key' = 'tu-api-key' }
    $uri = $BaseUrl + '/deployment/update-database'

    $resp = Invoke-RestMethod -Method GET -Uri $uri -Headers $headers
    $resp | ConvertTo-Json -Depth 10 | Write-Host
}
catch {
    $status = $_.Exception.Response.StatusCode.value__
    $reason = $_.Exception.Response.StatusDescription
    $body = $_.ErrorDetails.Message
    Write-Error "HTTP $status $reason. Body: $body"
}