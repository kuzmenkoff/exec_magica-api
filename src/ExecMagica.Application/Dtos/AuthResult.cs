namespace ExecMagica.Application.Dtos;

/// <summary>Outcome of an auth operation: success with a response, or failure with errors.</summary>
public class AuthResult
{
    /// <summary>Whether the operation succeeded.</summary>
    public bool Succeeded { get; private set; }

    /// <summary>Error messages when the operation failed.</summary>
    public string[] Errors { get; private set; } = Array.Empty<string>();

    /// <summary>The auth response when the operation succeeded.</summary>
    public AuthResponse? Response { get; private set; }

    /// <summary>Creates a successful result.</summary>
    public static AuthResult Success(AuthResponse response) =>
        new() { Succeeded = true, Response = response };

    /// <summary>Creates a failed result with the given errors.</summary>
    public static AuthResult Fail(params string[] errors) =>
        new() { Succeeded = false, Errors = errors };
}
