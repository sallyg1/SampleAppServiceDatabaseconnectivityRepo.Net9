using Azure;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;

namespace Pima.Api.Controllers;

/// <summary>
/// Controller for retrieving secrets from Azure Key Vault.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SecretsController : ControllerBase
{
    private readonly SecretClient _secretClient;
    private readonly ILogger<SecretsController> _logger;

    public SecretsController(SecretClient secretClient, ILogger<SecretsController> logger)
    {
        _secretClient = secretClient;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a secret value from Azure Key Vault by name.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <returns>The secret value wrapped in a JSON response.</returns>
    [HttpGet("{secretName}")]
    [ProducesResponseType(typeof(SecretResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSecret(string secretName)
    {
        try
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);

            _logger.LogInformation("Successfully retrieved secret '{SecretName}'", secretName);

            return Ok(new SecretResponse
            {
                Name = secret.Name,
                Value = secret.Value
            });
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            _logger.LogWarning("Secret '{SecretName}' not found in Key Vault", secretName);
            return NotFound(new { error = $"Secret '{secretName}' was not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving secret '{SecretName}'", secretName);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An error occurred while retrieving the secret." });
        }
    }
}

/// <summary>
/// Response model for a Key Vault secret.
/// </summary>
public class SecretResponse
{
    /// <summary>The name of the secret.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The value of the secret.</summary>
    public string Value { get; set; } = string.Empty;
}
