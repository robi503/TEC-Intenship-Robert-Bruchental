using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class JwtDecodeMiddleware
{
    private readonly RequestDelegate _next;

    public JwtDecodeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the request contains an Authorization header
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            string authorizationHeader = context.Request.Headers["Authorization"];

            // Check if the Authorization header starts with "Bearer "
            if (authorizationHeader.StartsWith("Bearer "))
            {
                // Extract the JWT token from the Authorization header
                string token = authorizationHeader["Bearer ".Length..];

                // Decode the JWT token
                var handler = new JwtSecurityTokenHandler();
                var decodedToken = handler.ReadJwtToken(token);

                // Set the decoded token as a claim in the HttpContext
                var identity = new ClaimsIdentity(decodedToken.Claims, "jwt");
                context.User = new ClaimsPrincipal(identity);

                await _next(context);
                return;
            }
        }
        // If the request does not contain an Authorization header or it's not a Bearer token,
        // continue processing the request without decoding the token
        await _next(context);
    }
}
